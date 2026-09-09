using System.ComponentModel.DataAnnotations;

namespace techXpress.UI.ActionRequests
{
    public class CreateRoleActionRequest
    {
        [Required]
        public string RoleName { get; set; }
        
    }
}
