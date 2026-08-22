using System.ComponentModel.DataAnnotations;

namespace NBEProject1.Models
{
    public class Category
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string CatName { get; set; } = string.Empty;

        [MaxLength(10)]
        public string? MccCode { get; set; }
    }
}