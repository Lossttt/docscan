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

namespace document_scanner.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentService _documentService;
        private readonly IAntiforgery _antiforgery;
        private readonly ILogger<DocumentController> _logger;

        public DocumentController(IDocumentService documentService, IAntiforgery antiforgery, ILogger<DocumentController> logger)
        {
            _documentService = documentService;
            _antiforgery = antiforgery;
            _logger = logger;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file, [FromServices] IAntiforgery antiforgery, [FromServices] IHttpContextAccessor httpContextAccessor)
        {
            try
            {
                var csrfToken = CsrfHelper.GenerateCsrfToken(antiforgery, httpContextAccessor.HttpContext);

                if (!FileValidationHelper.IsValidImage(file))
                {
                    _logger.LogWarning($"Invalid file upload attempt: {file.FileName}");
                    return BadRequest("Invalid file type or format.");
                }

                if (!CsrfHelper.VerifyCsrfToken(antiforgery, httpContextAccessor.HttpContext, csrfToken))
                {
                    _logger.LogWarning("CSRF token validation failed.");
                    return StatusCode(403, "CSRF token validation failed.");
                }

                using var memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);

                var document = new DocumentUploadDto
                {
                    FileName = file.FileName,
                    FileData = memoryStream.ToArray()
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
    }
}
