using Microsoft.AspNetCore.Mvc;
using techXpress.Services.Abstraction;
using techXpress.Services.DTOs.Products;
using techXpress.UI.Extensions.Session;
using techXpress.UI.VMs.Products;

namespace techXpress.UI.Controllers
{
    public class CompareController : Controller
    {
        private const string SessionKey = "CompareProducts";
        private readonly IProductManager _productManager;

        public CompareController(IProductManager productManager)
        {
            _productManager = productManager;
        }

        public async Task<IActionResult> Index()
        {
            List<int> productIds = HttpContext.Session.Get<List<int>>(SessionKey) ?? [];
            List<ProductDetailsVM> products = [];

            foreach (int productId in productIds)
            {
                ProductDetailsDto? product = await _productManager.GetProductDetailsAsync(productId);
                if (product != null)
                {
                    products.Add(product.ToVM());
                }
            }

            return View(products);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(int id)
        {
            ProductDTO? product = await _productManager.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            List<int> productIds = HttpContext.Session.Get<List<int>>(SessionKey) ?? [];
            if (productIds.Contains(id))
            {
                TempData["Info"] = "This product is already in your comparison.";
                return RedirectToAction(nameof(Index));
            }

            if (productIds.Count >= 3)
            {
                TempData["Error"] = "You can compare up to three products at a time.";
                return RedirectToAction(nameof(Index));
            }

            productIds.Add(id);
            HttpContext.Session.Set(SessionKey, productIds);
            TempData["Success"] = "Product added to comparison.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Remove(int id)
        {
            List<int> productIds = HttpContext.Session.Get<List<int>>(SessionKey) ?? [];
            productIds.Remove(id);
            HttpContext.Session.Set(SessionKey, productIds);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Clear()
        {
            HttpContext.Session.Remove(SessionKey);
            return RedirectToAction(nameof(Index));
        }
    }
}
