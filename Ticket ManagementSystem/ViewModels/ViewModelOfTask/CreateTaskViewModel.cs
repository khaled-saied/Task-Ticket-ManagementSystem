using System.ComponentModel.DataAnnotations;
using BLL.DataTransferObjects.TaskDtos;
using Microsoft.AspNetCore.Mvc.Rendering;
using Ticket_ManagementSystem.CustomValidationAttribute;

namespace Ticket_ManagementSystem.ViewModels.ViewModelOfTask
{
    public class CreateTaskViewModel
    {
        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Due date is required.")]
        [DataType(DataType.DateTime)]
        [FutureDate(ErrorMessage = "Due date must be in the future.")]
        public DateTime DueDate { get; set; }

        [Required(ErrorMessage = "Project selection is required.")]
        public int ProjectId { get; set; }

        public List<SelectListItem> Projects { get; set; } = new();

        // New
        public string? ReturnUrl { get; set; }
    }

}
