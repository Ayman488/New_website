using System.ComponentModel.DataAnnotations;

namespace crad_project.Models
{
    public class login
    {
        [Key]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please enter a Password!")]
        public string Password { get; set; }
    }
}
