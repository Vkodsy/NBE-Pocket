using System.ComponentModel.DataAnnotations;

namespace UserAuthApi.DTOs.Auth;

public class RefreshTokenRequest
{
    [Required]
    public string RefreshToken { get; set; } = string.Empty;
}