using System.ComponentModel.DataAnnotations;

namespace Ticket_ManagementSystem.ViewModels.AccountViewModel
{
    public class RegisterViewModel
    {

        [Required(ErrorMessage = "Full name is required")]
        [Display(Name = "Full Name")]
        [StringLength(100, ErrorMessage = "Full name must be less than 100 characters")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Username is required")]
        [Display(Name = "Username")]
        [StringLength(50, ErrorMessage = "Username must be less than 50 characters")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters")]
        [Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Confirm password is required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
