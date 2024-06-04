using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;

namespace document_scanner.Helpers
{
    public static class CsrfHelper
    {
        public static string GenerateCsrfToken(IAntiforgery antiforgery, HttpContext httpContext)
        {
            var tokens = antiforgery.GetAndStoreTokens(httpContext);
            return tokens.RequestToken;
        }

        public static bool VerifyCsrfToken(IAntiforgery antiforgery, HttpContext httpContext, string receivedToken)
        {
            var tokens = antiforgery.GetTokens(httpContext);
            return tokens.RequestToken == receivedToken;
        }
    }
}
