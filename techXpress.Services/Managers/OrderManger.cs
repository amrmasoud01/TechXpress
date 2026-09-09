using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using techXpress.DataAccess.Abstraction;
using techXpress.DataAccess.Entities;
using techXpress.DataAccess.Entities.Enums;
using techXpress.Services.Abstraction;
using techXpress.Services.DTOs.Orders;

namespace techXpress.Services.Managers
{
    public class OrderManger : IOrderManger
    {
        private readonly IUnitOfWork _unitOfWork;
        public OrderManger(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<GetAllOrdersDto> GetAllOrdersWithUsers()
        {
            IEnumerable<Order> orders = _unitOfWork.OrderRepository.GetAll("User");

            return orders
                .Select(o => o.GetAllOrdersDto())
                .ToList();
        }

        public IEnumerable<GetAllOrdersDto> GetAllOrdersByUserId(Guid userId)
        {
            IEnumerable<Order> orders = _unitOfWork.OrderRepository.GetOrdersByUser(userId);
            return orders
                .Select(o => o.GetAllOrdersDto())
                .ToList();
        }

        public async Task<OrderDto?> GetOrderById(int id)
        {
            Order? order = await _unitOfWork.OrderRepository.GetByIdAsync(o => o.OrderId == id);
            if(order != null)
            {
                return order.ToDto();
            }
            return null;
        }

        public OrderWithDetailsDto? GetOrderByIdWithDetails(int id)
        {
            Order? order = _unitOfWork.OrderRepository.GetOrderWithDetails(id);
            if (order != null)
            {
                return order.ToOrderWithDetailsDto();
            }
            return null;
        }

        public async Task<int> PlaceOrderAsync(CreateOrderDTO orderDto)
        {
            Order order = orderDto.ToOrder();

            order.OrderDate = DateTime.Now;
            order.OrderStatus = OrderStatus.Pending;

            await _unitOfWork.OrderRepository.CreateAsync(order);

            var orderDetails = new List<OrderDetail>();
            decimal totalAmount = 0;

            foreach (var item in orderDto.ProductQuantities)
            {
                if (item.Value <= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(orderDto), "Product quantities must be greater than zero.");
                }

                var product = await _unitOfWork.ProductRepository.GetByIdAsync(p => p.Id == item.Key);
                if (product == null)
                {
                    throw new Exception($"Product with ID {item.Key} not found.");
                }

                if (product.StockQuantity < item.Value)
                {
                    throw new Exception($"Insufficient stock for product {product.Name}.");
                }

                product.StockQuantity -= item.Value;
                _unitOfWork.ProductRepository.Update(product);
                totalAmount += product.Price * item.Value;

                orderDetails.Add(new OrderDetail
                {
                    ProductId = item.Key,
                    Quantity = item.Value,
                    UnitPrice = product.Price
                });
            }

            order.OrderDetails = orderDetails;
            order.TotalAmount = totalAmount;

            await _unitOfWork.SaveAsync();
            return order.OrderId;
        }


        public async Task UpdateOrderAsync(UpdateOrderDTO orderDto)
        {
            Order? order = await _unitOfWork.OrderRepository.GetByIdAsync(o => o.OrderId == orderDto.Id);
            if (order == null)
            {
                throw new Exception("Order not found");
            }

            order.OrderStatus = orderDto.OrderStatus ?? order.OrderStatus;
            order.SessionId = orderDto.SessionId ?? order.SessionId;
            order.Carrier = orderDto.Carrier ?? order.Carrier;
            order.TrackingNumber = orderDto.TrackingNumber ?? order.TrackingNumber;
            order.ShippingDate = orderDto.ShippingDate ?? order.ShippingDate;
            order.Address = orderDto.Address ?? order.Address;
            order.City = orderDto.City ?? order.City;
            order.RecipientPhoneNumber = orderDto.RecipientPhoneNumber ?? order.RecipientPhoneNumber;
            order.CouponId = orderDto.CouponId ?? order.CouponId;

            if (orderDto.PaymentId != null)
            {
                order.Payment = new Payment
                {
                    PaymentId = orderDto.PaymentId,
                    Amount = order.TotalAmount,
                    PaymentDate = DateTime.Now
                };
                order.PaymentId = orderDto.PaymentId;
            }
            await _unitOfWork.SaveAsync();
        }
    }
}
