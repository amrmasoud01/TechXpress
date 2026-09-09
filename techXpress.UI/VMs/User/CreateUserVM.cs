using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace techXpress.UI.VMs.User
{
    public class CreateUserVM
    {
        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }
        
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        
        [Display(Name = "Phone Number")]
        [Phone]
        public string PhoneNumber { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        
        [Display(Name = "Roles")]
        public List<string> SelectedRoles { get; set; }

        [ValidateNever]
        public List<SelectListItem> AllRoles { get; set; }
    }
} 