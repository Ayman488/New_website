using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace crad_project.Models
{
    public class Mobile
    {
        [Key]
        public int MobileId { get; set; }

        [Required(ErrorMessage = "Mobile name is required.")]
        [Display(Name = "Mobile Name")]
        public string Name { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        
        [NotMapped]

        [Display(Name = "Images")]
        public List<byte[]> Images { get; set; } = new List<byte[]>(); // تخزين الصور كـ byte[]

        [Required(ErrorMessage = "Price is required.")]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Price")]
        public decimal Price { get; set; }

        [Display(Name = "Color")]
        public string Color { get; set; }

        [Display(Name = "Internal Storage")]
        public string InternalStorage { get; set; } // مثال: 64GB, 128GB, إلخ

        [Display(Name = "Charging Type")]
        public string ChargingType { get; set; } // مثال: USB-C, Micro-USB, إلخ

        [Display(Name = "Battery Size (mAh)")]
        public int BatterySize { get; set; }

        [Display(Name = "Screen Size (inches)")]
        public double ScreenSize { get; set; }

        [Display(Name = "Operating System")]
        public string OperatingSystem { get; set; } // مثال: Android, iOS

        [Display(Name = "Year of Manufacture")]
        public int ManufactureYear { get; set; }

        // العلاقة مع SubCategory
        [Required]
        public int SubCategoryId { get; set; }
        public SubCategory SubCategory { get; set; }
    }
}
