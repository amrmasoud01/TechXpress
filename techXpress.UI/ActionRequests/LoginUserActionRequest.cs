using System.ComponentModel.DataAnnotations;

namespace techXpress.UI.ActionRequests
{
    public class LoginUserActionRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; } = false;
    }
}
