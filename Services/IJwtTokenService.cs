using System;
using UserAuthApi.Models;

namespace UserAuthApi.Services;

public interface IJwtTokenService
{
    (string Token, DateTimeOffset ExpiresAt) GenerateAccessToken(User user);
    string GenerateRefreshToken();
    string HashToken(string token);
}