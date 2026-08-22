using System.ComponentModel.DataAnnotations;
using UserAuthApi.Models;

namespace NBEProject1.Models
{
    public class Transaction
    {
        public int Id { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        public int MerchantId { get; set; }
        public Merchant Merchant { get; set; } = null!;

        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;

        [Range(0, double.MaxValue)]
        public decimal Amount { get; set; }

        [Required]
        [MaxLength(50)]
        public string TransactionType { get; set; } = string.Empty;

        public DateTimeOffset TransactionDate { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        [MaxLength(50)]
        public string? CategorizationSource { get; set; }

        public bool IsManualOverride { get; set; } = false;
    }
}