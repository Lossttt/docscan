using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using document_scanner.DTOs;
using document_scanner.Models;

namespace document_scanner.Services
{
    public interface IDocumentService
    {
        Task<DocumentAnalysisResult> AnalyzeDocumentAsync(DocumentUploadDto document);

    }
}