using System.ComponentModel.DataAnnotations;
namespace UserAuthApi.DTOs.Auth;
public class LoginRequest
{
    [Required]
    [EmailAddress]
    [StringLength(320)]
    public string Email { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty;
}