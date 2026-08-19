using System.ComponentModel.DataAnnotations;

namespace NBEProject1.DTOs.Auth;

public sealed class VerifyEmailRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Token { get; set; } = string.Empty;
}