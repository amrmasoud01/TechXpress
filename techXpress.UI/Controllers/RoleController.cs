using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using techXpress.UI.ActionRequests;
using techXpress.UI.Models;

namespace techXpress.UI.Controllers
{
    [Authorize(Roles = UserRole.Admin)]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;

        public RoleController(RoleManager<IdentityRole<Guid>> roleManager)
        {
            _roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            IEnumerable<IdentityRole<Guid>> roles = _roleManager.Roles.ToList();
            return View(roles);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateRoleActionRequest request)
        {
            if (ModelState.IsValid)
            {
                IdentityRole<Guid> newRole = new IdentityRole<Guid>
                {
                    Id = Guid.NewGuid(),
                    Name = request.RoleName
                };

                var result = await _roleManager.CreateAsync(newRole);

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("RoleCreationError", error.Description);
                    }
                }
            }

            return View(request);
        }

        [HttpGet]
        public IActionResult Update(Guid id)
        {
            IdentityRole<Guid>? role = _roleManager.Roles.FirstOrDefault(r => r.Id == id);
            if (role != null)
            {
                return View(role.ToActionRequest());
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UpdateRoleActionRequest request)
        {
            if (ModelState.IsValid)
            {
                IdentityRole<Guid>? role = _roleManager.Roles.FirstOrDefault(r => r.Id == request.Id);
                if (role != null)
                {
                    role.Name = request.RoleName;
                    var result = await _roleManager.UpdateAsync(role);
                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("RoleUpdateError", error.Description);
                        }
                    }
                }
            }
            return View(request);
        }

        [HttpDelete("api/role/delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            IdentityRole<Guid>? role = _roleManager.Roles.FirstOrDefault(r => r.Id == id);

            if (role != null)
            {
                await _roleManager.DeleteAsync(role);
                return Json(new { success = true, message = "Role deleted successfully" });
            }

            return Json(new {success = false , message = "Error happened while deleting the role"});
        }
    }
}
