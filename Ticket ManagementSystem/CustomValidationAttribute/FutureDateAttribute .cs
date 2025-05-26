using System.ComponentModel.DataAnnotations;

namespace Ticket_ManagementSystem.CustomValidationAttribute
{
    public class FutureDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is DateTime dateValue)
            {
                return dateValue > DateTime.Now;
            }
            return true; // Let [Required] handle nulls
        }
    }
}
