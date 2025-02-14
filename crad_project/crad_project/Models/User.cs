using System.ComponentModel.DataAnnotations;

namespace crad_project.Models
{
    public class User
    {
        [Key]
        public int userId { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string asddress { get; set; }

        // Navigation Properties
        public ICollection<UserAddress> UserAddresses { get; set; }
        public ICollection<Order> Orders { get; set; }
        public ICollection<Reviews> Reviews { get; set; }

    }
}
