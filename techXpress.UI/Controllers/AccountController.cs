using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using techXpress.DataAccess.Entities;
using techXpress.Services.Abstraction;
using techXpress.Services.DTOs.Orders;
using techXpress.UI.ActionRequests;
using techXpress.UI.Models;
using techXpress.UI.VMs.Account;

namespace techXpress.UI.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly IOrderManger _orderManger;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager
            , RoleManager<IdentityRole<Guid>> roleManager, IOrderManger orderManger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _orderManger = orderManger;
        }

        [HttpGet]
        public IActionResult Login([FromQuery] string? ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginUserActionRequest request, string? ReturnUrl)
        {
            if (ModelState.IsValid)
            {
                User? user = await _userManager.FindByEmailAsync(request.Email);

                if(user != null)
                {
                    bool isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);

                    if (isPasswordValid)
                    {
                        await _signInManager.SignInAsync(user, request.RememberMe);

                        if (!string.IsNullOrWhiteSpace(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                        {
                            return Redirect(ReturnUrl);
                        }

                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            ModelState.AddModelError("InvalidCredentials", "Invalid email or password.");
            return View(request);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(CreateUserActionRequest request)
        {
            if (ModelState.IsValid) 
            {
                User user = new User
                {
                    Id = Guid.NewGuid(),
                    UserName = request.UserName,
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber,
                };
                IdentityResult result = await _userManager.CreateAsync(user, request.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, UserRole.Customer);
                    return RedirectToAction(nameof(Login));
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                }
            }
            return View(request);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();  
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = UserRole.Customer)]
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            string? userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return Challenge();
            }

            User? user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            IEnumerable<GetAllOrdersDto> orderHistory = _orderManger.GetAllOrdersByUserId(user.Id);

            UserProfileVM userProfileVM = new UserProfileVM
            {
                UserId = userId,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                OrderHistory = orderHistory.Select(o => new GetAllOrdersDto
                {
                    Address = o.Address,
                    Carrier = o.Carrier,
                    City = o.City,
                    OrderDate = o.OrderDate,
                    OrderId = o.OrderId,
                    OrderStatus = o.OrderStatus,
                    RecipientPhoneNumber = o.RecipientPhoneNumber,
                    ShippingDate = o.ShippingDate,
                    TotalAmount = o.TotalAmount,
                    TrackingNumber = o.TrackingNumber,
                    UserEmail = o.UserEmail,
                    UserId = o.UserId,
                    PaymentId = o.PaymentId,
                    SessionId = o.SessionId,
                    CouponId = o.CouponId
                }).ToList()
            };

            return View(userProfileVM);
        }

        [Authorize(Roles = UserRole.Customer)]
        [HttpGet]
        public async Task<IActionResult> Settings()
        {
            string? userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return Challenge();
            }

            User? user = await _userManager.FindByIdAsync(userId);
            if(user == null)
            {
                return NotFound();
            }
            UserSettingsVM userSettingsVM = new UserSettingsVM
            {
                UserId = userId,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };

            return View(userSettingsVM);
        }

        [HttpPost]
        [Authorize(Roles = UserRole.Customer)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Settings(UserSettingsVM userSettingsVM)
        {
            string? currentUserId = _userManager.GetUserId(User);
            if (currentUserId == null)
            {
                return Challenge();
            }

            if (ModelState.IsValid)
            {
                User? user = await _userManager.FindByIdAsync(currentUserId);
                if (user == null)
                {
                    return NotFound();
                }
                user.UserName = userSettingsVM.UserName;
                user.Email = userSettingsVM.Email;
                user.PhoneNumber = userSettingsVM.PhoneNumber;
                IdentityResult result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    if (userSettingsVM.NewPassword != null)
                    {
                        IdentityResult identityResult = await _userManager.ChangePasswordAsync(user, userSettingsVM.CurrentPassword!, userSettingsVM.NewPassword!);
                        if (identityResult.Succeeded)
                        {
                            TempData["Success"] = "Password changed successfully.";
                        }
                        else
                        {
                            foreach (var error in identityResult.Errors)
                            {
                                ModelState.AddModelError(error.Code, error.Description);
                            }
                            return View(userSettingsVM);
                        }
                    }
                    return RedirectToAction(nameof(Profile));
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
            }
            return View(userSettingsVM);
        }

        [HttpPost]
        [Authorize(Roles = UserRole.Customer)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(UserSettingsVM userSettingsVM)
        {
            if (!ModelState.IsValid)
            {
                return View("Settings", userSettingsVM);
            }

            string? currentUserId = _userManager.GetUserId(User);
            if (currentUserId == null)
            {
                return Challenge();
            }

            User? user = await _userManager.FindByIdAsync(currentUserId);
            if (user == null)
            {
                return NotFound();
            }

            IdentityResult result = await _userManager.ChangePasswordAsync(user, userSettingsVM.CurrentPassword!, userSettingsVM.NewPassword!);
            if (result.Succeeded)
            {
                TempData["Success"] = "Password changed successfully.";
                return RedirectToAction(nameof(Profile));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View("Settings", userSettingsVM);
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

    }

}
