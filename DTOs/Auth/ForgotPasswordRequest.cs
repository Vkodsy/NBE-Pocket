using System.ComponentModel.DataAnnotations;

namespace NBEProject1.DTOs.Auth;

public class ForgotPasswordRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
}