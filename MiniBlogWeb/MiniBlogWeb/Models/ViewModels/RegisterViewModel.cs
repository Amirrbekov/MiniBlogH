using System.ComponentModel.DataAnnotations;

namespace MiniBlogWeb.Models.ViewModels;

public class RegisterViewModel
{
    [Required]
    public string Username { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    [MinLength(0, ErrorMessage = "Password has to be at least 6 characters")]
    public string Password { get; set; }
}
