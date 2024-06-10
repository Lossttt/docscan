using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using document_scanner.Data;
using document_scanner.DTOs;
using document_scanner.Helpers;
using document_scanner.Models;

namespace document_scanner.Services
{
    public class UploadService : IUploadService
    {

        private readonly FileDbContext _context;

        public UploadService(FileDbContext context)
        {
            _context = context;
        }

        public async Task<Document> UploadDocumentAsync(DocumentUploadDto documentDTO)
        {
            // Encrypt the file data
            var encryptedData = EncryptionHelper.EncryptData(documentDTO.FileData);
            var sanitizedFileName = Path.GetFileNameWithoutExtension(documentDTO.FileName);
            // Generate a unique file name
            // This is to avoid overwriting files with the same name
            var uniqueFileName = $"{sanitizedFileName}_{Guid.NewGuid()}{Path.GetExtension(documentDTO.FileName)}";

            var document = new Document
            {
                FileName = uniqueFileName,
                OriginalFileName = documentDTO.FileName,
                FileData = encryptedData,
                ContentType = documentDTO.ContentType,
                UploadDate = DateTime.UtcNow
            };

            _context.Documents.Add(document);
            await _context.SaveChangesAsync();
            return document;

        }
    }
}