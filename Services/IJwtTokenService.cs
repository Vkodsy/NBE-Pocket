using System;
using UserAuthApi.Models;

namespace NBEProject1.Services;

public interface IJwtTokenService
{
    (string Token, DateTime ExpiresAt) GenerateToken(User user);
}