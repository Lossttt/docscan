using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using document_scanner.DTOs;
using document_scanner.Helper;
using document_scanner.Helpers;
using document_scanner.Models;
using OpenCvSharp;

namespace document_scanner.Services
{
    public class DocumentService : IDocumentService
    {
        private const int RequiredBrightness = 150;
        private const double MinimumVisibilityThreshold = 0.8;
        private const int MaximumDeviation = 50;

        public async Task<DocumentAnalysisResult> AnalyzeDocumentAsync(DocumentUploadDto document)
        {
            using var memoryStream = new MemoryStream(document.FileData);
            var imageBytes = memoryStream.ToArray();

            var image = Mat.FromImageData(imageBytes);
            if (image.Empty())
            {
                return new DocumentAnalysisResult { Success = false, Message = "Ongeldig afbeeldingsbestand." };
            }

            var centerCheck = ImageProcessingHelper.IsDocumentCentered(image, MaximumDeviation);
            var visibilityCheck = ImageProcessingHelper.IsDocumentFullyVisible(image, MinimumVisibilityThreshold);
            var brightnessCheck = ImageProcessingHelper.IsBrightnessCorrect(image, RequiredBrightness);
            var colorCheck = ImageProcessingHelper.IsColorAcceptable(image);

            var isApproved = centerCheck && visibilityCheck && brightnessCheck && colorCheck;

            var sanitizedMessage = XssHelper.SanitizeHtml(isApproved ? "Document goedgekeurd." : "Document niet goedgekeurd.").ToString();

            var result = new DocumentAnalysisResult
            {
                Success = isApproved,
                Message = sanitizedMessage,
                Checks = new List<CheckResult>(), // initialiseren van de lijst
                Image = Convert.ToBase64String(imageBytes),
            };

            if (!centerCheck)
            {
                result.Checks.Add(new CheckResult { Passed = false, Message = "Please upload an image where all four corners are visible." });
            }

            if (!visibilityCheck)
            {
                result.Checks.Add(new CheckResult { Passed = false, Message = "Please upload a centered image." });
            }

            if (!brightnessCheck)
            {
                result.Checks.Add(new CheckResult { Passed = false, Message = "Please upload an image with sufficient brightness." });
            }

            if (!colorCheck)
            {
                result.Checks.Add(new CheckResult { Passed = false, Message = "Please upload an image with acceptable colors." });
            }

            return result;
        }
    }
}
