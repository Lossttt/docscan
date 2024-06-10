using Microsoft.AspNetCore.Mvc;
using document_scanner.DTOs;
using document_scanner.Services;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using document_scanner.Helpers;

namespace document_scanner.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UploadController : ControllerBase
    {
        private readonly IUploadService _uploadService;
        private readonly IAntiforgery _antiforgery;
        private readonly ILogger<UploadController> _logger;

        public UploadController(IUploadService uploadService, IAntiforgery antiforgery, ILogger<UploadController> logger)
        {
            _uploadService = uploadService;
            _antiforgery = antiforgery;
            _logger = logger;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                _logger.LogWarning("Empty file upload attempt.");
                return BadRequest("Invalid file.");
            }

            if (!FileValidationHelper.IsValidImage(file))
            {
                _logger.LogWarning($"Invalid file upload attempt: {file.FileName}");
                return BadRequest("Invalid file type or format.");
            }

            using var memoryStream = new System.IO.MemoryStream();
            await file.CopyToAsync(memoryStream);

            var document = new DocumentUploadDto
            {
                FileName = file.FileName,
                FileData = memoryStream.ToArray(),
                ContentType = file.ContentType
            };

            var uploadedDocument = await _uploadService.UploadDocumentAsync(document);

            _logger.LogInformation($"File uploaded and saved to database: {uploadedDocument.FileName}");
            return Ok(new { DocumentId = uploadedDocument.Id, FileName = uploadedDocument.FileName });
        }
    }
}
