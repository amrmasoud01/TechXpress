using System.ComponentModel.DataAnnotations;

public class UserSettingsVM
{
    public string UserId { get; set; }

    [Required]
    public string UserName { get; set; }

    [Required, EmailAddress]
    public string Email { get; set; }

    [Required]
    public string PhoneNumber { get; set; }

    [DataType(DataType.Password)]
    public string? CurrentPassword { get; set; }

    [DataType(DataType.Password)]
    [MinLength(6)]
    public string? NewPassword { get; set; }

    [DataType(DataType.Password)]
    [Compare("NewPassword", ErrorMessage = "Passwords do not match.")]
    public string? ConfirmNewPassword { get; set; }
}
