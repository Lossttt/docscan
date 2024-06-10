using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using document_scanner.Helpers;
using document_scanner.Services;
using document_scanner.DTOs;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.Extensions.Logging;
using document_scanner.Data;

namespace document_scanner.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentService _documentService;
        private readonly IAntiforgery _antiforgery;
        private readonly ILogger<DocumentController> _logger;
        private readonly FileDbContext _context;

        public DocumentController(IDocumentService documentService, IAntiforgery antiforgery, ILogger<DocumentController> logger, FileDbContext context)
        {
            _documentService = documentService;
            _antiforgery = antiforgery;
            _logger = logger;
            _context = context;
        }
        [HttpPost("analyze")]
        public async Task<IActionResult> Analyze(IFormFile file)
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

            var result = await _documentService.AnalyzeDocumentAsync(document);

            if (!result.Success)
            {
                _logger.LogWarning($"Document analysis failed for file: {file.FileName}");
                return BadRequest(result);
            }

            _logger.LogInformation($"Document approved: {file.FileName}");
            return Ok(result);
        }


        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file, [FromServices] IAntiforgery antiforgery, [FromServices] IHttpContextAccessor httpContextAccessor)
        {
            try
            {
                var csrfToken = CsrfHelper.GenerateCsrfToken(antiforgery, httpContextAccessor.HttpContext!);

                if (!FileValidationHelper.IsValidImage(file))
                {
                    _logger.LogWarning($"Invalid file upload attempt: {file.FileName}");
                    return BadRequest("Invalid file type or format.");
                }

                if (!CsrfHelper.VerifyCsrfToken(antiforgery, httpContextAccessor.HttpContext!, csrfToken))
                {
                    _logger.LogWarning("CSRF token validation failed.");
                    return StatusCode(403, "CSRF token validation failed.");
                }

                using var memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);

                var document = new DocumentUploadDto
                {
                    FileName = file.FileName,
                    FileData = memoryStream.ToArray(),
                    ContentType = file.ContentType
                };

                _logger.LogInformation($"File uploaded: {file.FileName}");

                var result = await _documentService.AnalyzeDocumentAsync(document);

                if (!result.Success)
                {
                    _logger.LogWarning($"Document analysis failed for file: {file.FileName}");
                    return BadRequest(result); // Return 400 status code if document is not approved
                }

                _logger.LogInformation($"Document approved: {file.FileName}");
                return Ok(result);
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the document.");
                return StatusCode(500, $"An error occurred while processing the document: {ex.Message}");
            }
        }

        [HttpGet("getDocument/{id}")]
        public IActionResult Get(int id)
        {
            var document = _context.Documents.FindAsync(id);

            if (document == null)
            {
                _logger.LogWarning($"Document not found: {id}");
                return NotFound();
            }

            return File(document.Result!.FileData, document.Result!.ContentType, document.Result!.FileName);
        }
    }
}
