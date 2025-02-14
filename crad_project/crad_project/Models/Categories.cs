using System.ComponentModel.DataAnnotations;

namespace crad_project.Models
{
    public class Categories
    {
        [Key]
        public int categoryId { get; set; }
        public string Name { get; set; }

        // Navigation Properties
        public ICollection<Product> Products { get; set; }

    }
}
