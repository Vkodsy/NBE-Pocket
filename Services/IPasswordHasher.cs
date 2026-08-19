namespace UserAuthApi.Services;

public interface IPasswordHasher
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string hash);
    string HashToken(string token);
}