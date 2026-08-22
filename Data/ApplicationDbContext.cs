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
    public DbSet<Budget> Budgets => Set<Budget>();
    public DbSet<SavingsGoal> SavingsGoals => Set<SavingsGoal>();
    public DbSet<Alert> Alerts => Set<Alert>();
    public DbSet<Merchant> Merchants => Set<Merchant>();
    public DbSet<Transaction> Transactions => Set<Transaction>();
    public DbSet<Offer> Offers => Set<Offer>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User Configuration
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");
            entity.HasKey(x => x.Id);

            entity.Property(x => x.FirstName).HasMaxLength(100).IsRequired();
            entity.Property(x => x.LastName).HasMaxLength(100).IsRequired();
            entity.Property(x => x.Email).HasMaxLength(320).IsRequired();
            entity.Property(x => x.NormalizedEmail).HasMaxLength(320).IsRequired();
            entity.HasIndex(x => x.NormalizedEmail).IsUnique().HasDatabaseName("UX_Users_NormalizedEmail");
            entity.Property(x => x.PasswordHash).HasMaxLength(500).IsRequired();

            entity.Property(x => x.EmailConfirmed).HasDefaultValue(false).IsRequired();
            entity.Property(x => x.EmailConfirmationTokenHash).HasMaxLength(256).IsRequired(false);
            entity.Property(x => x.EmailConfirmationExpiresAt).HasColumnType("datetimeoffset(7)").IsRequired(false);

            entity.Property(x => x.PasswordResetTokenHash).HasMaxLength(256).IsRequired(false);
            entity.Property(x => x.PasswordResetTokenExpiresAt).HasColumnType("datetimeoffset(7)").IsRequired(false);

            entity.Property(x => x.CreatedAt).HasColumnType("datetimeoffset(7)").IsRequired();
            entity.Property(x => x.UpdatedAt).HasColumnType("datetimeoffset(7)").IsRequired();

            entity.Property(x => x.Income).HasColumnType("decimal(18,2)").IsRequired(false);
        });

        // RefreshToken Configuration
        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.ToTable("RefreshTokens");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.TokenHash).HasMaxLength(256).IsRequired();
            entity.HasIndex(x => x.TokenHash).HasDatabaseName("IX_RefreshTokens_TokenHash");
            entity.Property(x => x.CreatedAt).HasColumnType("datetimeoffset(7)").IsRequired();
            entity.Property(x => x.ExpiresAt).HasColumnType("datetimeoffset(7)").IsRequired();
            entity.Property(x => x.RevokedAt).HasColumnType("datetimeoffset(7)").IsRequired(false);

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
            entity.Property(x => x.CatName).HasMaxLength(100).IsRequired();
            entity.Property(x => x.MccCode).HasMaxLength(10);
        });

        // Budget Configuration
        modelBuilder.Entity<Budget>(entity =>
        {
            entity.ToTable("Budgets");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.MonthlyLimit).HasColumnType("decimal(18,2)").IsRequired();
            entity.Property(x => x.Month).HasColumnType("date").IsRequired();

            entity.HasOne(x => x.User)
                .WithMany(u => u.Budgets)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(x => x.Category)
                .WithMany(c => c.Budgets)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // SavingsGoal Configuration
        modelBuilder.Entity<SavingsGoal>(entity =>
        {
            entity.ToTable("SavingsGoals");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.GoalName).HasMaxLength(150).IsRequired();
            entity.Property(x => x.TargetAmount).HasColumnType("decimal(18,2)").IsRequired();
            entity.Property(x => x.CurrentAmount).HasColumnType("decimal(18,2)").IsRequired();
            entity.Property(x => x.TargetDate).HasColumnType("date").IsRequired();
            entity.Property(x => x.CreatedAt).HasColumnType("datetimeoffset(7)").IsRequired();

            entity.HasOne(x => x.User)
                .WithMany(u => u.SavingsGoals)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Alert Configuration
        modelBuilder.Entity<Alert>(entity =>
        {
            entity.ToTable("Alerts");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.AlertType).HasMaxLength(50).IsRequired();
            entity.Property(x => x.Message).HasMaxLength(500).IsRequired();
            entity.Property(x => x.CreatedAt).HasColumnType("datetimeoffset(7)").IsRequired();
            entity.Property(x => x.IsRead).HasDefaultValue(false).IsRequired();

            entity.HasOne(x => x.User)
                .WithMany(u => u.Alerts)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(x => x.Category)
                .WithMany(c => c.Alerts)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Merchant Configuration
        modelBuilder.Entity<Merchant>(entity =>
        {
            entity.ToTable("Merchants");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.MerchantName).HasMaxLength(150).IsRequired();
            entity.Property(x => x.MccCode).HasMaxLength(10);

            entity.HasOne(x => x.Category)
                .WithMany(c => c.Merchants)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Transaction Configuration
        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.ToTable("Transactions");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Amount).HasColumnType("decimal(18,2)").IsRequired();
            entity.Property(x => x.TransactionType).HasMaxLength(50).IsRequired();
            entity.Property(x => x.TransactionDate).HasColumnType("datetimeoffset(7)").IsRequired();
            entity.Property(x => x.Description).HasMaxLength(500);
            entity.Property(x => x.CategorizationSource).HasMaxLength(50);
            entity.Property(x => x.IsManualOverride).HasDefaultValue(false).IsRequired();

            entity.HasOne(x => x.User)
                .WithMany(u => u.Transactions)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Merchant)
                .WithMany(m => m.Transactions)
                .HasForeignKey(x => x.MerchantId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Category)
                .WithMany(c => c.Transactions)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Offer Configuration (standalone, no FKs)
        modelBuilder.Entity<Offer>(entity =>
        {
            entity.ToTable("Offers");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.MerchantName).HasMaxLength(150).IsRequired();
            entity.Property(x => x.Title).HasMaxLength(200).IsRequired();
            entity.Property(x => x.Category).HasMaxLength(100);
            entity.Property(x => x.Type).HasMaxLength(50);
            entity.Property(x => x.Price).HasColumnType("decimal(18,2)");
            entity.Property(x => x.DiscountPercentage).HasColumnType("decimal(5,2)");
            entity.Property(x => x.StartDate).HasColumnType("date").IsRequired();
            entity.Property(x => x.EndDate).HasColumnType("date").IsRequired();
        });
    }
}