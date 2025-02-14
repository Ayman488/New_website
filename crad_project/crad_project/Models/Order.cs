namespace crad_project.Models
{
    public class Order
    {
        public int orderId { get; set; }
        public int UserId { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; } // Pending, Completed, Canceled
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public User User { get; set; }
        public ICollection<OrderItems> OrderItems { get; set; }
        public Payments Payment { get; set; }
    }
}
