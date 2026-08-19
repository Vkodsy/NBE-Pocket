using NBEProject1.Repositories;
using NBEProject1.Services;
using System.Security.Cryptography;
using UserAuthApi.DTOs.Auth;
using UserAuthApi.Models;

namespace UserAuthApi.Services;


public class AuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailService _emailService;
    private readonly IPasswordHasher _passwordHasher;

    public AuthService(
        IUserRepository userRepository,
        IEmailService emailService,
        IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _emailService = emailService;
        _passwordHasher = passwordHasher;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        var normalizedEmail = request.Email.ToUpperInvariant();

        var existingUser = await _userRepository.GetByNormalizedEmailAsync(normalizedEmail);
        if (existingUser != null)
        {
            throw new InvalidOperationException("User with this email already exists.");
        }

        var rawToken = Convert.ToHexString(RandomNumberGenerator.GetBytes(32));
        var tokenHash = _passwordHasher.HashToken(rawToken);

        var user = new User
        {
            Id = Guid.NewGuid(),
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            NormalizedEmail = normalizedEmail,
            PasswordHash = _passwordHasher.HashPassword(request.Password),
            EmailConfirmed = false,
            EmailConfirmationTokenHash = tokenHash,
            EmailConfirmationExpiresAt = DateTimeOffset.UtcNow.AddHours(24),
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };

        await _userRepository.AddAsync(user);

        var confirmationLink = $"https://localhost:7085/api/Auth/verify-email?email={Uri.EscapeDataString(user.Email)}&token={rawToken}";

        Console.WriteLine("\n=======================================================");
        Console.WriteLine($"VERIFICATION LINK: {confirmationLink}");
        Console.WriteLine($"RAW TOKEN: {rawToken}");
        Console.WriteLine("=======================================================\n");

        try
        {
            await _emailService.SendConfirmationEmailAsync(user.Email, confirmationLink);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[SMTP Email Error]: {ex.Message}");
        }

        // MAKE SURE THIS RETURN STATEMENT IS AT THE END OUTSIDE OF ANY TRY/IF BLOCKS:
        return new AuthResponse
        {
            Message = "Registration successful. Please check your email to verify your account."
        };
    }

    public async Task<bool> VerifyEmailAsync(string email, string token)
    {
        var normalizedEmail = email.ToUpperInvariant();
        var user = await _userRepository.GetByNormalizedEmailAsync(normalizedEmail);

        if (user == null) return false;
        if (user.EmailConfirmed) return true;
        if (user.EmailConfirmationExpiresAt == null || user.EmailConfirmationExpiresAt < DateTimeOffset.UtcNow) return false;

        var incomingHash = _passwordHasher.HashToken(token);
        if (!string.Equals(user.EmailConfirmationTokenHash, incomingHash, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        user.EmailConfirmed = true;
        user.EmailConfirmationTokenHash = null;
        user.EmailConfirmationExpiresAt = null;
        user.UpdatedAt = DateTimeOffset.UtcNow;

        await _userRepository.UpdateAsync(user);
        return true;
    }

}
