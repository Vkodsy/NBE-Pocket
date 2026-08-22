using Microsoft.EntityFrameworkCore;
using NBEProject1.Models;
using UserAuthApi.Models;

namespace UserAuthApi.Data;

public sealed class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options)
    : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
public DbSet<Category> Categories => Set<Category>();

protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);

    // User Configuration
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

        // Email verification configurations
        entity.Property(x => x.EmailConfirmed)
            .HasDefaultValue(false)
            .IsRequired();

        entity.Property(x => x.EmailConfirmationTokenHash)
            .HasMaxLength(256)
            .IsRequired(false);

        entity.Property(x => x.EmailConfirmationExpiresAt)
            .HasColumnType("datetimeoffset(7)")
            .IsRequired(false);

        // Password reset configurations
        entity.Property(x => x.PasswordResetTokenHash)
            .HasMaxLength(256)
            .IsRequired(false);

        entity.Property(x => x.PasswordResetTokenExpiresAt)
            .HasColumnType("datetimeoffset(7)")
            .IsRequired(false);

        entity.Property(x => x.CreatedAt)
            .HasColumnType("datetimeoffset(7)")
            .IsRequired();

        entity.Property(x => x.UpdatedAt)
            .HasColumnType("datetimeoffset(7)")
            .IsRequired();
    });

    // RefreshToken Configuration
    modelBuilder.Entity<RefreshToken>(entity =>
    {
        entity.ToTable("RefreshTokens");
        entity.HasKey(x => x.Id);

        entity.Property(x => x.TokenHash)
            .HasMaxLength(256)
            .IsRequired();

        entity.HasIndex(x => x.TokenHash)
            .HasDatabaseName("IX_RefreshTokens_TokenHash");

        entity.Property(x => x.CreatedAt)
            .HasColumnType("datetimeoffset(7)")
            .IsRequired();

        entity.Property(x => x.ExpiresAt)
            .HasColumnType("datetimeoffset(7)")
            .IsRequired();

        entity.Property(x => x.RevokedAt)
            .HasColumnType("datetimeoffset(7)")
            .IsRequired(false);

        // Foreign Key Relationship
        entity.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    });

    // Category Configuration
    modelBuilder.Entity<Category>(entity =>
    {
        entity.ToTable("Categories");
        entity.HasKey(x => x.Id);
        // TODO: fill in Category property configuration once Category.cs is finalized
    });
}
}