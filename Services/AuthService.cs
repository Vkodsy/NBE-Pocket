using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NBEProject1.DTOs.Auth;
using NBEProject1.Services;
using System;
using UserAuthApi.Data;
using UserAuthApi.DTOs.Auth;
using UserAuthApi.Models;

namespace UserAuthApi.Services;

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IJwtTokenService _jwtTokenService;

    public AuthService(
        ApplicationDbContext dbContext,
        IPasswordHasher<User> passwordHasher,
        IJwtTokenService jwtTokenService)
    {
        _dbContext = dbContext;
        _passwordHasher = passwordHasher;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<UserResponse> RegisterAsync(RegisterRequest request)
    {
        var email = NormalizeEmail(request.Email);

        if (!string.Equals(
                request.Password,
                request.ConfirmPassword,
                StringComparison.Ordinal))
        {
            throw new ArgumentException(
                "Password and confirm password must match.");
        }

        var existingUser = await _dbContext.Users
            .AsNoTracking()
            .AnyAsync(u => u.Email == email);

        if (existingUser)
        {
            throw new DuplicateEmailException();
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            FirstName = request.FirstName.Trim(),
            LastName = request.LastName.Trim(),
            Email = email,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        user.PasswordHash = _passwordHasher.HashPassword(
            user,
            request.Password);

        _dbContext.Users.Add(user);

        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
            when (ex.InnerException?.Message.Contains(
                "UX_Users_Email",
                StringComparison.OrdinalIgnoreCase) == true)
        {
            throw new DuplicateEmailException();
        }

        return MapUser(user);
    }

    public async Task<AuthResponse?> LoginAsync(LoginRequest request)
    {
        var email = NormalizeEmail(request.Email);

        var user = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Email == email);

        if (user is null)
        {
            return null;
        }

        var verificationResult = _passwordHasher.VerifyHashedPassword(
            user,
            user.PasswordHash,
            request.Password);

        if (verificationResult == PasswordVerificationResult.Failed)
        {
            return null;
        }

        if (verificationResult == PasswordVerificationResult.SuccessRehashNeeded)
        {
            user.PasswordHash = _passwordHasher.HashPassword(
                user,
                request.Password);

            user.UpdatedAt = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();
        }

        var (token, expiresAt) = _jwtTokenService.GenerateToken(user);

        return new AuthResponse
        {
            Message = "Login successful",
            Token = token,
            ExpiresAt = expiresAt,
            User = MapUser(user)
        };
    }

    private static string NormalizeEmail(string email)
    {
        return email.Trim().ToLowerInvariant();
    }

    private static UserResponse MapUser(User user)
    {
        return new UserResponse
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email
        };
    }
}
