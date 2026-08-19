using Microsoft.EntityFrameworkCore;
using NBEProject1.Data; // Adjust this to match your DbContext namespace
using UserAuthApi.Data;
using UserAuthApi.Models;

namespace NBEProject1.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context; // Replace with your actual DbContext class name

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<User?> GetByNormalizedEmailAsync(string normalizedEmail)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.NormalizedEmail == normalizedEmail);
    }

    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }
}