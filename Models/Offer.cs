using System.ComponentModel.DataAnnotations;

namespace NBEProject1.Models
{
    public class Offer
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string MerchantName { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        [MaxLength(100)]
        public string? Category { get; set; }

        [MaxLength(50)]
        public string? Type { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? Price { get; set; }

        [Range(0, 100)]
        public decimal? DiscountPercentage { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}