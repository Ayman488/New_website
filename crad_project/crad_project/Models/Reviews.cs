using System.ComponentModel.DataAnnotations;

namespace crad_project.Models
{
    public class Reviews
    {
        [Key]
        public int reviewId { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int Rating { get; set; } // 1-5
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public User User { get; set; }
        public Product Product { get; set; }
    }
}
