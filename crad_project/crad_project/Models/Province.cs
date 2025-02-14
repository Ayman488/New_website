using System.ComponentModel.DataAnnotations;

namespace crad_project.Models
{
    public class Province
    {
        [Key]
        public int provinceId { get; set; }
        public string Name { get; set; }

        // Navigation Properties
        public ICollection<Address> Addresses { get; set; }
    }
}
