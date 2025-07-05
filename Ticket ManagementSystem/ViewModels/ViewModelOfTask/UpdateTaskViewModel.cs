using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Ticket_ManagementSystem.CustomValidationAttribute;

namespace Ticket_ManagementSystem.ViewModels.ViewModelOfTask
{
    public class UpdateTaskViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public string Status { get; set; } = string.Empty;

        [Required]
        [FutureDate(ErrorMessage = "Due date must be in the future.")]
        public DateTime DueDate { get; set; }
        public string? UserId { get; set; } // FK to ApplicationUser
        public List<SelectListItem> Users { get; set; } = new List<SelectListItem>();
    }
}
