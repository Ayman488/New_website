using System.ComponentModel.DataAnnotations;

namespace crad_project.Models
{
    public class Admins
    {
        [Key]
        public int adminId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
