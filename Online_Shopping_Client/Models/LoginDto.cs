using System.ComponentModel.DataAnnotations;

namespace Online_Shopping_WebAPPClient.Models
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Email is required")]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(20, MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        public string Role;

        //[Display(Name = "Remember me?")]
        //public bool RememberMe { get; set; }
    }
}
