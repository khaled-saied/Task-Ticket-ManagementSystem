using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ticket_ManagementSystem.ViewModels.ViewModelOfTicket
{
    public class UpdateTicketViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, ErrorMessage = "Title can't exceed 100 characters")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Type is required")]
        public string Type { get; set; } = string.Empty;

        [Required(ErrorMessage = "Status is required")]
        public string Status { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Description can't exceed 500 characters")]
        public string Description { get; set; } = string.Empty;

    }
}
