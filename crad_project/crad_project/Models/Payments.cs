using System.ComponentModel.DataAnnotations;

namespace crad_project.Models
{
    public class Payments
    {
        [Key]
        public int paymentId { get; set; }
        public int OrderId { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentStatus { get; set; } // Paid, Failed, Pending
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public Order Order { get; set; }
    }
}
