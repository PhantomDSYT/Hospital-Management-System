using System.ComponentModel.DataAnnotations;

namespace HMS.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        //[Required]
        //[DataType(DataType.Text)]
        //[Display(Name = "User Role")]
        //public string UserRole { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Password and confirmation password to not match.")]
        public string ConfirmPassword { get; set; }
    }
}
