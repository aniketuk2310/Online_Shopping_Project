using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Xml.Linq;

namespace Online_Shopping_WebAPICore.Models
{
    [Index(nameof(Email), IsUnique = true)]
    [Index(nameof(Phone), IsUnique = true)]
    public class User
    {
        [Key]
        public int UId { get; set; }

        [Required(ErrorMessage = "FirstName is required")]
        [Display(Name = "First Name")]
        [StringLength(20, MinimumLength = 2)]
        public string FName { get; set; }

        [Required(ErrorMessage = "LastNme is required")]
        [Display(Name = "Last Name")]
        [StringLength(20, MinimumLength = 2)]
        public string LName { get; set; }

        [Phone]
        [Display(Name = "Phone")]
        [Required(ErrorMessage = "Phone is required")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Phone must be 10 digits")]
        public string Phone { get; set; }

        [Display(Name = "Address1")]
        [Required(ErrorMessage = "Address is required")]
        [StringLength(100, MinimumLength = 3)]
        public string Address { get; set; }

        [Display(Name = "Role")]
        [DefaultValue("Customer")]
        public string role { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(20, MinimumLength = 6)]
        public string Password { get; set; }

        public virtual ICollection<Orders> Orders { get; set; }
    }
}
