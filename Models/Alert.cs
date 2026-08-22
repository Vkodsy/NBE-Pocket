using System.ComponentModel.DataAnnotations;
using UserAuthApi.Models;

namespace NBEProject1.Models
{
    public class Alert
    {
        public int Id { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        public int? CategoryId { get; set; }
        public Category? Category { get; set; }

        [Required]
        [MaxLength(50)]
        public string AlertType { get; set; } = string.Empty;

        [Required]
        [MaxLength(500)]
        public string Message { get; set; } = string.Empty;

        public DateTimeOffset CreatedAt { get; set; }

        public bool IsRead { get; set; } = false;
    }
}