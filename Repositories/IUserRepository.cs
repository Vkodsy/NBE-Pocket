
using UserAuthApi.Models;

namespace NBEProject1.Repositories;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id);
    Task<User?> GetByNormalizedEmailAsync(string normalizedEmail);
    Task AddAsync(User user);
    Task UpdateAsync(User user);
}