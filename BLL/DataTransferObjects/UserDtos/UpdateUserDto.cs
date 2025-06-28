using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace BLL.DataTransferObjects.UserDtos
{
    public class UpdateUserDto
    {
        public string Id { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string ConcurrencyStamp { get; set; } = string.Empty;
        public string? ImageName { get; set; } = string.Empty;
        public IFormFile? Image { get; set; } = null!;
    }
}
