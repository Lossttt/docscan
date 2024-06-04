using System;
using System.IO;

namespace document_scanner.Helpers
{
    public static class LoggingHelper
    {
        private const string LogFilePath = "upload_logs.txt";

        public static void LogFileUpload(string fileName, long fileSize)
        {
            var logMessage = $"{DateTime.UtcNow}: Uploaded file '{fileName}' with size {fileSize} bytes.";
            File.AppendAllText(LogFilePath, logMessage + Environment.NewLine);
        }

        public static void LogInvalidFileUpload(string fileName, long fileSize)
        {
            var logMessage = $"{DateTime.UtcNow}: Invalid file uploaded - '{fileName}' ({fileSize} bytes).";
            File.AppendAllText(LogFilePath, logMessage + Environment.NewLine);
        }
    }
}
