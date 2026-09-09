using techXpress.Services.Abstraction;
using techXpress.Services.DTOs.CategoryDTOs;
using techXpress.Services.Managers;
using Microsoft.AspNetCore.Mvc;
using techXpress.UI.ActionRequests;
using techXpress.UI.VMs.Category;
using Microsoft.AspNetCore.Authorization;
using techXpress.UI.Models;
using System.Threading.Tasks;

namespace techXpress.UI.Controllers
{
    [Authorize(Roles = UserRole.Admin)]
    public class CategoryController : Controller
    {
        private readonly ICategoryManager _categoryManager;
        public CategoryController(ICategoryManager categoryManager)
        {
            _categoryManager = categoryManager;
        }

        public IActionResult Index()
        {
            IEnumerable<CategoryVM> categoryVMs = _categoryManager.GetAll()
                .Select(c => c.ToVM());

            return View(categoryVMs);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCategoryActionRequest request) 
        {
            if(ModelState.IsValid)
            {
                await _categoryManager.CreateCategoryAsync(request.ToDto());
                TempData["successNotification"] = "Category Created Successfully";
                return RedirectToAction(nameof(Index));
            }
            TempData["errorNotification"] = "Category Creation Failed";
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            CategoryDTO? categoryDTO = await _categoryManager.GetByIdAsync(id);

            if(categoryDTO != null)
            {
                return View(categoryDTO.ToActionRequest());
            }
            TempData["errorNotification"] = "Category Not Found";
            return RedirectToAction(nameof(Index));
        } 

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UpdateCategoryActionRequest request)
        {
            if (ModelState.IsValid)
            {
                CategoryDTO categoryDTO = request.ToDto();
                await _categoryManager.UpdateCategoryAsync(categoryDTO);
                TempData["successNotification"] = "Category Updated Successfully";
                
                return RedirectToAction(nameof(Index));
            }
            TempData["errorNotification"] = "Category Update Failed";
            return View(request);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, bool confirm)
        {
            CategoryDTO? categoryDTO = await _categoryManager.GetByIdAsync(id);

            if (categoryDTO != null)
            {
                await _categoryManager.DeleteCategoryAsync(id);
                TempData["successNotification"] = "Category Deleted Successfully";
            }
            else
            {
                TempData["errorNotification"] = "Category Not Found";
            }
            
            return RedirectToAction(nameof(Index));
        }

        [HttpDelete("api/category/delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            CategoryDTO? category =  await _categoryManager.GetByIdAsync(id);
            
            if(category != null)
            {
                await _categoryManager.DeleteCategoryAsync(id);
                return Json(new { success = true, message = "Category deleted successfully" });
            }
            return Json(new { success = false, message = "Category deletion failed" });
        }

        public IActionResult CheckName(string name, int id)
        {
            CategoryDTO? category = _categoryManager.GetAll()
                .FirstOrDefault(c => c.Name.Trim() == name.Trim() && c.CategoryId != id);

            if (category == null)
            {
                return Json(true);
            }
            else
            {
                return Json(false);
            }
        }
    }
}
