using System.ComponentModel.DataAnnotations;

namespace NBEProject1.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string CatName { get; set; } = string.Empty;

        [MaxLength(10)]
        public string? MccCode { get; set; }

        // New: from ERD (VARBINARY icon)
        public byte[]? Icon { get; set; }

        // Navigation properties
        public ICollection<Budget> Budgets { get; set; } = new List<Budget>();
        public ICollection<Alert> Alerts { get; set; } = new List<Alert>();
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
        public ICollection<Merchant> Merchants { get; set; } = new List<Merchant>();
    }
}