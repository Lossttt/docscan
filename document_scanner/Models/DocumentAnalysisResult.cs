using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace document_scanner.Models
{
    public class DocumentAnalysisResult
    {
        [Required]
        public bool Success { get; set; }
        public string? Message { get; set; }
        public string? Quality { get; set; }
        public string? Colors { get; set; }
        public string? Image { get; set; }
        public bool Approved { get; set; }
        public List<CheckResult> Checks { get; set; }
        public List<CheckResult> PassedChecks => Checks.Where(check => check.Passed).ToList();
        public List<CheckResult> FailedChecks => Checks.Where(check => !check.Passed).ToList();
        // Constructor
        public DocumentAnalysisResult()
        {
            Checks = new List<CheckResult>(); // Initialisatie van Checks in de constructor
        }
    }
}