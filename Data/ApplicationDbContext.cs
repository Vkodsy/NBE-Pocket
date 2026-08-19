using Microsoft.EntityFrameworkCore;
using UserAuthApi.Models;

namespace UserAuthApi.Data;

public sealed class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options)
    : DbContext(options)
{
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.FirstName)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(x => x.LastName)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(x => x.Email)
                .HasMaxLength(320)
                .IsRequired();

            entity.Property(x => x.NormalizedEmail)
                .HasMaxLength(320)
                .IsRequired();

            entity.HasIndex(x => x.NormalizedEmail)
                .IsUnique()
                .HasDatabaseName("UX_Users_NormalizedEmail");

            entity.Property(x => x.PasswordHash)
                .HasMaxLength(500)
                .IsRequired();

            entity.Property(x => x.CreatedAt)
                .HasColumnType("datetimeoffset(7)")
                .IsRequired();

            entity.Property(x => x.UpdatedAt)
                .HasColumnType("datetimeoffset(7)")
                .IsRequired();
        });
    }
}