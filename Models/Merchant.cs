using System.ComponentModel.DataAnnotations;

namespace NBEProject1.Models
{
    public class Merchant
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string MerchantName { get; set; } = string.Empty;

        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;

        [MaxLength(10)]
        public string? MccCode { get; set; }

        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}