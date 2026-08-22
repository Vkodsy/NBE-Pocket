namespace UserAuthApi.Models;
using NBEProject1.Models;

public sealed class User
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string NormalizedEmail { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public bool EmailConfirmed { get; set; } = false;
    public string? EmailConfirmationTokenHash { get; set; }
    public DateTimeOffset? EmailConfirmationExpiresAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public string? PasswordResetTokenHash { get; set; }
    public DateTimeOffset? PasswordResetTokenExpiresAt { get; set; }
    public decimal? Income { get; set; }

    // Navigation properties
    public ICollection<Budget> Budgets { get; set; } = new List<Budget>();
    public ICollection<SavingsGoal> SavingsGoals { get; set; } = new List<SavingsGoal>();
    public ICollection<Alert> Alerts { get; set; } = new List<Alert>();
    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}