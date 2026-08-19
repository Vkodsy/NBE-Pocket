using System.ComponentModel.DataAnnotations;
namespace UserAuthApi.DTOs.Auth;
public class RegisterRequest
{
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string FirstName { get; set; } = string.Empty;
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string LastName { get; set; } = string.Empty;
    [Required]
    [EmailAddress]
    [StringLength(320)]
    public string Email { get; set; } = string.Empty;
    [Required]
    [StringLength(128, MinimumLength = 8)]
    public string Password { get; set; } = string.Empty;
    [Required]
    public string ConfirmPassword { get; set; } = string.Empty;
}