using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace document_scanner.Models
{
    public class CheckResult
    {
        public bool Passed { get; set; }
        public required string Message { get; set; }
    }
}