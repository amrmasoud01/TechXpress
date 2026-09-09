using System.ComponentModel.DataAnnotations;

namespace techXpress.UI.ActionRequests
{
    public class CreateUserActionRequest
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string PasswordConfirmation { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        [Length(11, 11, ErrorMessage = "Phone number must be exactly 11 digits.")]
        public string PhoneNumber { get; set; }

    }
}
