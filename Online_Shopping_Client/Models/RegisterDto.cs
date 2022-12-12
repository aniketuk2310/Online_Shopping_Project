using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Xml.Linq;

namespace Online_Shopping_WebAPPClient.Models
{
    public class RegisterDto
    {
        public int id { get; set; }

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

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(20, MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Role")]
        [DefaultValue("Customer")]
        public string role { get; set; }

        //[Required(ErrorMessage = "Please enter confirm password")]
        //[Compare("Password", ErrorMessage = "Password and confirm password should be the same")]
        //[Display(Name = "Confirm Password")]
        //public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Confirmation Password is required.")]
        [Compare("Password", ErrorMessage = "Password and Confirmation Password must match.")]
        public string ConfirmPassword { get; set; }

    }
}
