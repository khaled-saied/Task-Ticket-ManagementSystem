using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace BLL.Services.AttachmentServices.AttachmentServices
{
    public interface IAttachmentServices
    {
        //Upload
        string? Upload(IFormFile file, string folderName);

        //Delete
        bool Delete(string filePath);
    }
}
