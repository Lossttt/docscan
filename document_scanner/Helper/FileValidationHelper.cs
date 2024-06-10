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
        private static readonly int maxFileSizeInBytes = 5 * 1024 * 1024; // 5 MB

        public static bool IsValidImage(IFormFile file)
        {
            if (file == null || file.Length == 0 || file.Length > maxFileSizeInBytes)
            {
                return false;
            }

            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!AllowedImageTypes.Contains(fileExtension))
            {
                return false;
            }

            return true;
        }
    }
}
