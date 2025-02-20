using System.ComponentModel.DataAnnotations;

namespace crad_project.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int CategoryId { get; set; }

        // تغيير قائمة النصوص إلى قائمة بيانات الصور الثنائية
        public byte[] Image { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public Categories Category { get; set; }
        public ICollection<Reviews> Reviews { get; set; }
        public ICollection<OrderItems> OrderItems { get; set; }

        public Product()
        {
            Reviews = new HashSet<Reviews>();
            OrderItems = new HashSet<OrderItems>();
        }
    }
}
