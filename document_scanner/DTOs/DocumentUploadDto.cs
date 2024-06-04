using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace document_scanner.DTOs
{
    public class DocumentUploadDto
    {
        public string FileName { get; set; }
        public byte[] FileData { get; set; }
    }
}