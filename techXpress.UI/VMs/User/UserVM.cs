using System.ComponentModel.DataAnnotations;

namespace techXpress.UI.VMs.User
{
    public class UserVM
    {
        public Guid Id { get; set; }
        
        [Display(Name = "Username")]
        public string UserName { get; set; }
        
        [Display(Name = "Email")]
        public string Email { get; set; }
        
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        
        [Display(Name = "Roles")]
        public string Roles { get; set; }
    }
} 