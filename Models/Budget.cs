using System.ComponentModel.DataAnnotations;
using UserAuthApi.Models;

namespace NBEProject1.Models
{
    public class Budget
    {
        public int Id { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;

        [Range(0, double.MaxValue)]
        public decimal MonthlyLimit { get; set; }

        public DateTime Month { get; set; }
    }
}