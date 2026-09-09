using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rotativa.AspNetCore;
using Stripe.Checkout;
using System.Security.Claims;
using techXpress.DataAccess.Entities.Enums;
using techXpress.Services.Abstraction;
using techXpress.Services.DTOs.Orders;
using techXpress.UI.ActionRequests;
using techXpress.UI.Extensions.Session;
using techXpress.UI.VMs.Orders;
using techXpress.UI.VMs.ShoppingCart;
using techXpress.UI.Models;

namespace techXpress.UI.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderManger _orderManger;
        private readonly IProductManager _productManager;

        public OrderController(IOrderManger orderManger, IProductManager productManager)
        {
            _orderManger = orderManger;
            _productManager = productManager;
        }

        [Authorize(Roles = UserRole.Admin)]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = UserRole.Admin)]
        [HttpGet("api/orders")]
        public IActionResult GetAllOrders(string? orderStatus)
        {
            IEnumerable<GetAllOrdersDto> orders = _orderManger.GetAllOrdersWithUsers()
                .Where(o => string.IsNullOrEmpty(orderStatus) || o.OrderStatus == orderStatus);
            return Json(new { data = orders });
        }

        [Authorize(Roles = $"{UserRole.Seller},{UserRole.Customer}")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = $"{UserRole.Seller},{UserRole.Customer}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateOrderActionRequest request)
        {
            if (ModelState.IsValid)
            {
                ShoppingCartVM? cart = HttpContext.Session.Get<ShoppingCartVM>("Cart");
                if (cart == null || cart.CartItems.Count == 0)
                {
                    ModelState.AddModelError(string.Empty, "Your shopping cart is empty.");
                    return View(request);
                }

                string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (!Guid.TryParse(userId, out Guid currentUserId))
                {
                    return Challenge();
                }

                if (string.IsNullOrWhiteSpace(Stripe.StripeConfiguration.ApiKey))
                {
                    ModelState.AddModelError(string.Empty, "Payment is not configured. Please contact support.");
                    return View(request);
                }

                List<ShoppingCartItemVM> products = cart.CartItems;
                foreach (ShoppingCartItemVM item in products)
                {
                    var product = await _productManager.GetByIdAsync(item.ProductId);
                    if (product == null || item.Quantity > product.StockQuantity)
                    {
                        ModelState.AddModelError(string.Empty, $"{item.ProductName} is no longer available in the requested quantity.");
                        return View(request);
                    }

                    item.ProductName = product.Name;
                    item.Price = product.Price;
                    item.StockQuantity = product.StockQuantity;
                    item.SubTotal = product.Price * item.Quantity;
                }

                cart.Total = products.Sum(item => item.SubTotal);
                HttpContext.Session.Set("Cart", cart);

                CreateOrderDTO orderDto = request.ToDto();
                orderDto.UserId = currentUserId;
                orderDto.TotalAmount = cart.Total;
                orderDto.ProductQuantities = products
                    .ToDictionary(p => p.ProductId, p => p.Quantity);

                int orderId = await _orderManger.PlaceOrderAsync(orderDto);

                var domain = $"{Request.Scheme}://{Request.Host}";
                var options = new SessionCreateOptions
                {

                    LineItems = products.Select(p => new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = p.ProductName,
                                //Images = new List<string> { p.ImageUrl },
                            },
                            UnitAmountDecimal = (decimal)p.Price * 100,
                        },
                        Quantity = p.Quantity,
                    }).ToList(),

                    Mode = "payment",
                    SuccessUrl = $"{domain}/Order/Success?id={orderId}&sessionId={{CHECKOUT_SESSION_ID}}",
                    CancelUrl = $"{domain}/ShoppingCart/ViewCart",
                };

                var service = new SessionService();
                Session session = service.Create(options);
                if (string.IsNullOrWhiteSpace(session.Url))
                {
                    ModelState.AddModelError(string.Empty, "The payment session could not be started.");
                    return View(request);
                }

                await _orderManger.UpdateOrderAsync(new UpdateOrderDTO
                {
                    Id = orderId,
                    SessionId = session.Id
                });

                return Redirect(session.Url);
            }
            ModelState.AddModelError("Order Data Error", "Can't Place Order");
            return View(request);
        }

        [HttpGet]
        [Authorize(Roles = UserRole.Admin)]
        public IActionResult Update(int id)
        {
            UpdateOrderActionRequest? order = _orderManger.GetOrderByIdWithDetails(id)
                ?.ToDto();

            ViewBag.OrderStatusOptions = new SelectList(new[]
            {
                "Pending", "Approved", "InProgress", "Canceled", "Shipped"
            });
            ViewBag.orderId = id;

            return View(order);
        }

        [HttpPost]
        [Authorize(Roles = UserRole.Admin)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id,UpdateOrderActionRequest request)
        {
            if (!Enum.TryParse<OrderStatus>(request.OrderStatus, true, out OrderStatus orderStatus))
            {
                ModelState.AddModelError(nameof(request.OrderStatus), "Choose a valid order status.");
            }

            if (ModelState.IsValid)
            {
                UpdateOrderDTO orderDto = request.ToUpdateOrderDto();
                orderDto.Id = id;
                orderDto.OrderStatus = orderStatus;

                await _orderManger.UpdateOrderAsync(orderDto);

                TempData["successNotification"] = "Order updated successfully";
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError("Order Data Error", "Can't Update Order");
            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> Success(int id, string sessionId)
        {
            OrderDto? orderDto = await _orderManger.GetOrderById(id);
            if (orderDto == null)
            {
                return NotFound();
            }

            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userId, out Guid currentUserId) || orderDto.UserId != currentUserId)
            {
                return Forbid();
            }

            if (string.IsNullOrWhiteSpace(sessionId) || orderDto.SessionId != sessionId)
            {
                return BadRequest("Invalid payment session.");
            }

            SessionService service = new SessionService();
            Session session = service.Get(sessionId);
            if (!string.Equals(session.PaymentStatus, "paid", StringComparison.OrdinalIgnoreCase) ||
                string.IsNullOrWhiteSpace(session.PaymentIntentId))
            {
                return BadRequest("Payment has not been completed.");
            }
            
            if (string.IsNullOrWhiteSpace(orderDto.PaymentId))
            {
                await _orderManger.UpdateOrderAsync(new UpdateOrderDTO
                {
                    Id = id,
                    OrderStatus = OrderStatus.Approved,
                    PaymentId = session.PaymentIntentId
                });
            }

            HttpContext.Session.Remove("Cart");

            return View(id);
        }

        [HttpGet]
        [Authorize(Roles = UserRole.Admin)]
        public IActionResult Reports()
        {
            var allOrders = _orderManger.GetAllOrdersWithUsers();
            
            var reportData = new OrderReportsVM
            {
                TotalOrders = allOrders.Count(),
                TotalRevenue = allOrders.Sum(o => o.TotalAmount),
                PendingOrders = allOrders.Count(o => o.OrderStatus == "Pending"),
                CompletedOrders = allOrders.Count(o => o.OrderStatus == "Shipped"),
                CanceledOrders = allOrders.Count(o => o.OrderStatus == "Canceled"),
                
                MonthlyRevenue = allOrders
                    .Where(o => o.OrderDate >= DateTime.Now.AddMonths(-12))
                    .GroupBy(o => new { o.OrderDate.Year, o.OrderDate.Month })
                    .Select(g => new MonthlyRevenueData
                    {
                        Month = $"{g.Key.Year}-{g.Key.Month:D2}",
                        Revenue = g.Sum(o => o.TotalAmount)
                    })
                    .OrderBy(x => x.Month)
                    .ToList(),

                OrderStatusDistribution = allOrders
                    .GroupBy(o => o.OrderStatus)
                    .Select(g => new OrderStatusData
                    {
                        Status = g.Key,
                        Count = g.Count()
                    })
                    .ToList(),

                RecentOrders = allOrders
                    .OrderByDescending(o => o.OrderDate)
                    .Take(5)
                    .ToList()
            };

            return View(reportData);
        }

        [HttpGet]
        [Authorize(Roles = UserRole.Admin)]
        public IActionResult GetReportPDF()
        {
            var allOrders = _orderManger.GetAllOrdersWithUsers();

            var reportData = new OrderReportsVM
            {
                TotalOrders = allOrders.Count(),
                TotalRevenue = allOrders.Sum(o => o.TotalAmount),
                PendingOrders = allOrders.Count(o => o.OrderStatus == "Pending"),
                CompletedOrders = allOrders.Count(o => o.OrderStatus == "Shipped"),
                CanceledOrders = allOrders.Count(o => o.OrderStatus == "Canceled"),

                MonthlyRevenue = allOrders
                    .Where(o => o.OrderDate >= DateTime.Now.AddMonths(-12))
                    .GroupBy(o => new { o.OrderDate.Year, o.OrderDate.Month })
                    .Select(g => new MonthlyRevenueData
                    {
                        Month = $"{g.Key.Year}-{g.Key.Month:D2}",
                        Revenue = g.Sum(o => o.TotalAmount)
                    })
                    .OrderBy(x => x.Month)
                    .ToList(),

                OrderStatusDistribution = allOrders
                    .GroupBy(o => o.OrderStatus)
                    .Select(g => new OrderStatusData
                    {
                        Status = g.Key,
                        Count = g.Count()
                    })
                    .ToList(),

                RecentOrders = allOrders
                    .OrderByDescending(o => o.OrderDate)
                    .Take(5)
                    .ToList()
            };

            return new ViewAsPdf("ReportsPDF", reportData, ViewData)
            {
                PageMargins = new Rotativa.AspNetCore.Options.Margins
                {
                    Top = 20,
                    Bottom = 20,
                    Left = 20,
                    Right = 20
                }
            };
        }
    }
}
