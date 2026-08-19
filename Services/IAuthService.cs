using NBEProject1.DTOs.Auth;
using UserAuthApi.DTOs.Auth;
namespace UserAuthApi.Services;
public interface IAuthService
{
    Task<UserResponse> RegisterAsync(RegisterRequest request);
    Task<AuthResponse?> LoginAsync(LoginRequest request);
}
