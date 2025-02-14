using System.ComponentModel.DataAnnotations;

namespace crad_project.Models
{
    public class UserAddress
    {
        [Key]
        public int userAddressId { get; set; }
        public int UserId { get; set; }
        public int AddressId { get; set; }

        // Navigation Properties
        public User User { get; set; }
        public Address Address { get; set; }
    }
}
