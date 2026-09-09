using System.Diagnostics;
using techXpress.Services.Abstraction;
using techXpress.Services.DTOs.Products;
using Microsoft.AspNetCore.Mvc;
using techXpress.UI.Models;
using techXpress.UI.VMs.Category;
using techXpress.UI.VMs.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

namespace techXpress.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductManager _productManager;
        private readonly ICategoryManager _categoryManager;

        public HomeController(ILogger<HomeController> logger, IProductManager productManager , ICategoryManager categoryManager)
        {
            _logger = logger;
            _productManager = productManager;
            _categoryManager = categoryManager;
        }

        public IActionResult Index()
        {
            IEnumerable<CategoryWithProductsVM> categoryWithProductsVMs = _categoryManager.GetAll("Products.Reviews")
                .Select(c => new CategoryWithProductsVM
                {
                    CategoryId = c.CategoryId,
                    CategoryName = c.Name,
                    Products = c.ProductDTOs!.Select(p => p.ToProductVM()).Take(8).ToList()
                }).ToList();

            ViewData["TakeFullWidth"] = true;

            return View(categoryWithProductsVMs);
        }

        public IActionResult Shop(int? categoryId)
        {
            var categoriesSelectList = _categoryManager.GetAll()
                .Select(c => new SelectListItem
                {
                    Value = c.CategoryId.ToString(),
                    Text = c.Name,
                    Selected = c.CategoryId == categoryId
                })
                .ToList();

            ViewBag.Categories = categoriesSelectList;
            ViewData["TakeFullWidth"] = true;

            return View();
        }

        [HttpGet("api/productFilter")]
        public IActionResult GetProducts(string? searchTerm,int? categoryId,
            string? sortBy,int? pageNo,int? pageSize)
        {
            int currentPage = Math.Max(pageNo ?? 1, 1);
            int itemsPerPage = Math.Clamp(pageSize ?? 12, 1, 48);

            IEnumerable<ProductVM> products = _productManager.GetProductsWhere(new ProductQueryDTO
            {
                CategoryId = categoryId,
                SearchTerm = searchTerm,
                SortBy = sortBy
            }).Select(p => p.ToProductVM());

            PagedProductListVM pagedProductList = new PagedProductListVM
            {
                PageNo = currentPage,
                PageSize = itemsPerPage,
                TotalCount = products.Count(),
            };

            pagedProductList.Products = products
                .Skip((pagedProductList.PageNo - 1) * pagedProductList.PageSize)
                .Take(pagedProductList.PageSize)
                .ToList();

            return PartialView("_ProductListPartial", pagedProductList);
        }

        public async Task<IActionResult> Details(int id)
        {
            ProductDetailsDto? productDetails = await _productManager.GetProductDetailsAsync(id);
            if (productDetails == null)
            {
                return NotFound();
            }
            ProductDetailsVM productDetailsVM = productDetails.ToVM();
            return View(productDetailsVM);
        }

        public IActionResult AboutUs()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult FAQ()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
