using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace document_scanner.Helpers
{
    public static class FileValidationHelper
    {
        private static readonly HashSet<string> AllowedImageTypes = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            ".jpg", ".jpeg", ".png"
        };

        private const long MaxFileSize = 10 * 1024 * 1024; // 10 MB

        public static bool IsValidImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return false;
            }

            var extension = Path.GetExtension(file.FileName);
            return AllowedImageTypes.Contains(extension) && file.Length <= MaxFileSize;
        }
    }
}
