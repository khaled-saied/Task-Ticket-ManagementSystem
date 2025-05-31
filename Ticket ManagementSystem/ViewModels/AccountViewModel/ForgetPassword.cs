using System.ComponentModel.DataAnnotations;

namespace Ticket_ManagementSystem.ViewModels.AccountViewModel
{
    public class ForgetPassword
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string Email { get; set; } = string.Empty;
    }
}
