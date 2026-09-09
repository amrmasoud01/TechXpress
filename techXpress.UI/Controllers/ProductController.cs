using techXpress.Services.Abstraction;
using techXpress.Services.DTOs.CategoryDTOs;
using techXpress.Services.DTOs.Products;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using techXpress.UI.ActionRequests;
using techXpress.UI.VMs.Category;
using techXpress.UI.VMs.Products;
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using techXpress.UI.Models;
using System.Security.Claims;

namespace techXpress.UI.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductManager _productManager;
        private readonly ICategoryManager _categoryManager;
        private readonly IFilesService _filesService;
        private readonly IWebHostEnvironment _environment;

        public ProductController(IProductManager productManager,
            IFilesService filesService,
            ICategoryManager categoryManager,
            IWebHostEnvironment environment)
        {
            _productManager = productManager;
            _filesService = filesService;
            _categoryManager = categoryManager;
            _environment = environment;
        }

        [Authorize(Roles = UserRole.Admin)]
        public IActionResult Index()
        {
            IEnumerable<CategoryVM> categories = _categoryManager.GetAll()
                .Select(c => c.ToVM())
                .ToList();

            return View(categories);
        }

        [Authorize(Roles = UserRole.Admin)]
        [HttpGet("api/products")]
        public IActionResult GetAllProducts(int? categoryId)
        {
            IEnumerable<ProductVM> products = _productManager.GetAll()
                .Where(p => categoryId == null || p.CategoryId == categoryId)
                .Select(p => p.ToProductVM())
                .ToList();

            return Json(new {data= products });
        }

        [Authorize(Roles = UserRole.Admin)]
        [HttpGet]
        public IActionResult Create()
        {
            IEnumerable<SelectListItem> categoriesList = _categoryManager.GetAll()
                .Select(c => new SelectListItem(c.Name, c.CategoryId.ToString()))
                .ToList();

            CreateProductActionRequest productActionRequest = new CreateProductActionRequest()
            {
                CategoryList = categoriesList
            };

            return View(productActionRequest);
        }

        [Authorize(Roles = UserRole.Admin)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateProductActionRequest productActionRequest)
        {
            ValidateImage(productActionRequest.ImageUrl);

            if (ModelState.IsValid)
            {
                string uniqueFileName = CreateImageFileName(productActionRequest.ImageUrl.FileName);
                _filesService.Upload(GetImagePath(uniqueFileName), productActionRequest.ImageUrl);

                ProductDTO productDTO = productActionRequest.ToDto();
                productDTO.Image = uniqueFileName;

                await _productManager.CreateProductAsync(productDTO);

                TempData["successNotification"] = "Product created successfully";

                return RedirectToAction(nameof(Index));
            }
            productActionRequest.CategoryList = _categoryManager.GetAll()
                .Select(c => new SelectListItem(c.Name, c.CategoryId.ToString()))
                .ToList();
            TempData["errorNotification"] = "Product creation failed";
            return View(productActionRequest);
        }

        [HttpGet]
        [Authorize(Roles = UserRole.Admin)]
        public async Task<IActionResult> Update(int id)
        {
            ProductDTO? product = await _productManager.GetByIdAsync(id);
            if (product != null)
            {
                UpdateProductActionRequest productActionRequest = product.ToActionRequest();
                productActionRequest.CategoryList = _categoryManager.GetAll()
                    .Select(c => new SelectListItem(c.Name, c.CategoryId.ToString()))
                    .ToList();

                return View(productActionRequest);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Authorize(Roles = UserRole.Admin)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, UpdateProductActionRequest request)
        {
            request.Id = id;
            ProductDTO? existingProduct = await _productManager.GetByIdAsync(id);
            if (existingProduct == null)
            {
                return NotFound();
            }

            request.ImageUrl = existingProduct.Image;
            if (request.Image != null)
            {
                ValidateImage(request.Image);
            }

            if (ModelState.IsValid)
            {
                ProductDTO productDTO = request.ToDto();

                if (request.Image != null)
                {
                    string oldImagePath = GetImagePath(existingProduct.Image);
                    if(System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }

                    string uniqueFileName = CreateImageFileName(request.Image.FileName);
                    _filesService.Upload(GetImagePath(uniqueFileName), request.Image);
                    productDTO.Image = uniqueFileName;
                }
                else
                {
                    productDTO.Image = existingProduct.Image;
                }

                await _productManager.UpdateProductAsync(productDTO);
                TempData["successNotification"] = "Product updated successfully";
                return RedirectToAction(nameof(Index));
            }

            request.CategoryList = _categoryManager.GetAll()
                .Select(c => new SelectListItem(c.Name, c.CategoryId.ToString()))
                .ToList();
            TempData["errorNotification"] = "Product update failed";
            return View(request);
        }

        [Authorize(Roles = UserRole.Customer)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddReview(int id, CreateProductReviewActionRequest request)
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userId, out Guid currentUserId))
            {
                return Challenge();
            }

            if (!ModelState.IsValid)
            {
                TempData["errorNotification"] = request.Rating is < 1 or > 5
                    ? "Please select a rating between 1 and 5."
                    : "The review could not be submitted. Please check your comment and try again.";
                return RedirectToAction("Details", "Home", new { id });
            }

            ProductReviewDTO review = new ProductReviewDTO
            {
                Rating = request.Rating,
                Comment = request.Comment?.Trim(),
                UserId = currentUserId,
                CreatedAt = DateTime.UtcNow
            };
            try
            {
                await _productManager.AddReviewAsync(id, review);
                TempData["successNotification"] = "Review added successfully";
            }
            catch (InvalidOperationException exception)
            {
                TempData["errorNotification"] = exception.Message;
            }

            return RedirectToAction("Details", "Home", new { id = id });
        }

        [HttpDelete("api/delete/{id}")]
        [Authorize(Roles = UserRole.Admin)]
        public async Task<IActionResult> Delete(int id)
        {
            ProductDTO? product = await _productManager.GetByIdAsync(id);
            
            if(product != null)
            {
                await _productManager.DeleteProductAsync(id);

                string imagePath = GetImagePath(product.Image);
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }

                return Json(new { success = true, message = "Product deleted successfully" });
            }
            return Json(new { success = false, message = "Product deletion failed" });
        }

        private void ValidateImage(IFormFile? image)
        {
            if (image == null)
            {
                return;
            }

            string[] allowedExtensions = [".jpg", ".jpeg", ".png", ".webp"];
            string extension = Path.GetExtension(image.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(extension))
            {
                ModelState.AddModelError("ImageUrl", "Choose a JPG, PNG, or WebP image.");
            }

            if (image.Length > 5 * 1024 * 1024)
            {
                ModelState.AddModelError("ImageUrl", "The image must be smaller than 5 MB.");
            }
        }

        private static string CreateImageFileName(string originalFileName)
        {
            return $"{Guid.NewGuid():N}{Path.GetExtension(originalFileName).ToLowerInvariant()}";
        }

        private string GetImagePath(string fileName)
        {
            return Path.Combine(_environment.WebRootPath, "Images", Path.GetFileName(fileName));
        }

    }
}
