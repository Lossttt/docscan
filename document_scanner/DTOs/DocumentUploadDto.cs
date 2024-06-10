using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace document_scanner.DTOs
{
    public class DocumentUploadDto
    {
        public required string FileName { get; set; }
        public required byte[] FileData { get; set; }
        public string? ContentType { get;  set; }

        // Constructor
        public DocumentUploadDto()
        {
        }
    }
} 