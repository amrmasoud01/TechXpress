using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace techXpress.UI.VMs.User
{
    public class UpdateUserRoleVM
    {
        public Guid UserId { get; set; }
        
        [Display(Name = "Username")]
        public string UserName { get; set; }
        
        [Display(Name = "Email")]
        public string Email { get; set; }
        
        [Display(Name = "Roles")]
        public List<string> SelectedRoles { get; set; }
        
        public List<SelectListItem> AllRoles { get; set; }
    }
} 