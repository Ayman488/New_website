using System.ComponentModel.DataAnnotations;

namespace crad_project.Models
{
    public class Product
    {
        [Key]
        public int productId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int CategoryId { get; set; }
        public List<string> ImageUrls { get; set; } = new List<string>();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public Categories Category { get; set; }
        public ICollection<Reviews> Reviews { get; set; }
        public ICollection<OrderItems> OrderItems { get; set; }

    }
}
