using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Services.AttachmentServices.AttachmentServices;
using Microsoft.AspNetCore.Http;
using static System.Net.Mime.MediaTypeNames;

namespace BLL.Services.AttachmentServices
{
    class AttachmentService : IAttachmentServices
    {

        List<string> AllowedExtenstions = [".jpg", ".jpeg", ".png"];
        const int MaxFileSize = 2 * 1024 * 1024; // 2 MB
        public string? Upload(IFormFile file, string folderName)
        {
            // Check if the file With AllowedExtenstions exists
            var extension = Path.GetExtension(file.FileName);
            if (!AllowedExtenstions.Contains(extension)) return null;

            // Check if the file size is within the limit
            if (file.Length > MaxFileSize || file.Length==0) return null;

            //Get Folder Path
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files",folderName);

            //Generate a unique file name
            string fileName = $"{Guid.NewGuid()}_{file.FileName}";

            //Get full file path
            string filePath = Path.Combine(folderPath, fileName);

            //Create File Stream To Save the file
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            //Return the file path
            return fileName;
        }

        public bool Delete(string filePath)
        {
            // Check if the file exists
            if (!File.Exists(filePath)) return false;
            try
            {
                // Delete the file
                File.Delete(filePath);
                return true;
            }
            catch (Exception)
            {
                // Handle exceptions (e.g., file in use, permission issues)
                return false;
            }
        }

    }
}
