using System.ComponentModel.DataAnnotations;

namespace crad_project.Models
{
    public class OrderItems
    {
        [Key]
        public int orederItemId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        // Navigation Properties
        public Order Order { get; set; }
        public Product Product { get; set; }
    }
}
