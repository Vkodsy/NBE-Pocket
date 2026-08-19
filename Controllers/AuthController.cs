using Microsoft.AspNetCore.Mvc;
using UserAuthApi.DTOs.Auth;
using UserAuthApi.Services;

namespace UserAuthApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(
        [FromBody] RegisterRequest request)
    {
        try
        {
            var user = await _authService.RegisterAsync(request);

            return StatusCode(
                StatusCodes.Status201Created,
                new
                {
                    message = "Registration successful.",
                    user
                });
        }
        catch (DuplicateEmailException)
        {
            return BadRequest(new
            {
                message = "The registration could not be completed."
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequest request)
    {
        var result = await _authService.LoginAsync(request);

        if (result is null)
        {
            return Unauthorized(new
            {
                message = "Invalid email or password."
            });
        }

        return Ok(result);
    }
}
