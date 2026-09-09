using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Data;
using techXpress.DataAccess.Entities;
using techXpress.UI.Models;
using techXpress.UI.VMs.User;

namespace techXpress.UI.Controllers
{
    [Authorize(Roles = UserRole.Admin)]
    public class UserController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;

        public UserController(
            UserManager<User> userManager,
            RoleManager<IdentityRole<Guid>> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            var usersList = new List<UserVM>();
            
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                usersList.Add(new UserVM
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Roles = string.Join(", ", roles)
                });
            }
            
            return View(usersList);
        }
        
        [HttpGet]
        public IActionResult Create()
        {
            var roles = _roleManager.Roles.ToList();
            var model = new CreateUserVM
            {
                AllRoles = roles.Select(r => new SelectListItem
                {
                    Text = r.Name,
                    Value = r.Name
                }).ToList(),
                SelectedRoles = new List<string>()
            };
            
            return View(model);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserVM model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber
                };
                
                var result = await _userManager.CreateAsync(user, model.Password);
                
                if (result.Succeeded)
                {
                    if (model.SelectedRoles != null && model.SelectedRoles.Count > 0)
                    {
                        await _userManager.AddToRolesAsync(user, model.SelectedRoles);
                    }
                    
                    TempData["SuccessMessage"] = "User created successfully.";
                    return RedirectToAction(nameof(Index));
                }
                
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            
            var roles = _roleManager.Roles.ToList();
            model.AllRoles = roles.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Name,
                Selected = model.SelectedRoles != null && model.SelectedRoles.Contains(r.Name)
            }).ToList();
            
            return View(model);
        }
        
        [HttpGet]
        public async Task<IActionResult> UpdateRole(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return NotFound();
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            var roles = _roleManager.Roles.ToList();

            var model = new UpdateUserRoleVM
            {
                UserId = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                SelectedRoles = userRoles.ToList(),
                AllRoles = roles.Select(r => new SelectListItem
                {
                    Text = r.Name,
                    Value = r.Name,
                    Selected = userRoles.Contains(r.Name)
                }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateRole(UpdateUserRoleVM model)
        {
            if (!ModelState.IsValid)
            {
                var roles = _roleManager.Roles.ToList();
                model.AllRoles = roles.Select(r => new SelectListItem
                {
                    Text = r.Name,
                    Value = r.Name,
                    Selected = model.SelectedRoles != null && model.SelectedRoles.Contains(r.Name)
                }).ToList();
                
                return View(model);
            }

            var user = await _userManager.FindByIdAsync(model.UserId.ToString());
            if (user == null)
            {
                return NotFound();
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            await _userManager.RemoveFromRolesAsync(user, userRoles);

            if (model.SelectedRoles != null)
            {
                await _userManager.AddToRolesAsync(user, model.SelectedRoles);
            }

            TempData["SuccessMessage"] = "User roles updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpDelete("api/user/delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return Json(new { success = false, message = "User not found" });
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return Json(new { success = true, message = "User deleted successfully" });
            }

            return Json(new { success = false, message = "Error occurred while deleting user" });
        }
    }
} 
