using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Online_Shopping_WebAPICore.Models
{
    public class Product
    {
        [Key]
        public int PId { get; set; }

        [Required(ErrorMessage = "Product Name is required")]
        [Display(Name = "Product Name")]
        [StringLength(20, MinimumLength = 3)]
        public string PName { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [Display(Name = "Description")]
        [StringLength(200, MinimumLength = 1)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Image is required")]
        [Display(Name = "Image")]
        public string Image { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Display(Name = "Unit Price")]
        public decimal UnitPrice { get; set; }

        [Required(ErrorMessage = "Units In Stock is required")]
        [Display(Name = "Units In Stock")]
        public int UnitsInStock { get; set; }

        public PStatus Status { get; set; }

        [Required(ErrorMessage = "Category is required")]
        [Display(Name = "Category")]
        public Category Category { get; set; }

    }
    public enum PStatus
    {
        Available,Unavailable
    }
}
