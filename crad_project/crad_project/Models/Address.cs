using System.ComponentModel.DataAnnotations;

namespace crad_project.Models
{
    public class Address
    {
        [Key]
        public int addressId { get; set; }
        public int ProvinceId { get; set; }
        public string description { get; set; }

        // Navigation Properties
        public Province Province { get; set; }
        public ICollection<UserAddress> UserAddresses { get; set; }
    }
}
