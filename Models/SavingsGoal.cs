using System.ComponentModel.DataAnnotations;
using UserAuthApi.Models;

namespace NBEProject1.Models
{
    public class SavingsGoal
    {
        public int Id { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        [Required]
        [MaxLength(150)]
        public string GoalName { get; set; } = string.Empty;

        [Range(0, double.MaxValue)]
        public decimal TargetAmount { get; set; }

        [Range(0, double.MaxValue)]
        public decimal CurrentAmount { get; set; }

        public DateTime TargetDate { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
    }
}