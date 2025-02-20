using System.ComponentModel.DataAnnotations;

namespace crad_project.Models
{
    public class Categories
    {
        [Key]
        public int categoryId { get; set; }

        [Required(ErrorMessage = "Category Name is required.")]
        [Display(Name = "Category Name")]
        public string Name { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;

        // Navigation Properties
        public ICollection<Product> Products { get; set; }
        public ICollection<SubCategory> SubCategories { get; set; } // العلاقة مع SubCategory
    }
}
