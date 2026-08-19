using NBEProject1.DTOs.Auth;
namespace UserAuthApi.DTOs.Auth;
public class AuthResponse
{
    public string Message { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public UserResponse User { get; set; } = new();
}
