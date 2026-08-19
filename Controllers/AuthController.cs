using Microsoft.AspNetCore.Mvc;
using UserAuthApi.DTOs.Auth;
using UserAuthApi.Services;

namespace UserAuthApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        try
        {
            var response = await _authService.RegisterAsync(request);
            return Ok(response);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("verify-email")]
    public async Task<IActionResult> VerifyEmail([FromQuery] string email, [FromQuery] string token)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(token))
        {
            return BadRequest(new { message = "Email and verification token are required." });
        }

        var isVerified = await _authService.VerifyEmailAsync(email, token);

        if (!isVerified)
        {
            return BadRequest(new { message = "Invalid or expired confirmation link." });
        }

        return Ok(new { message = "Your email has been verified successfully! You can now log in." });
    }
}