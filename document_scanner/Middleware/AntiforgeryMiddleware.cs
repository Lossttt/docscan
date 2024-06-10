using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace document_scanner.Middleware
{
    public class AntiforgeryMiddleware
    {
        private readonly RequestDelegate _next;

        public AntiforgeryMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IAntiforgery antiforgery)
        {
            if (context.Request.Path == "/" && context.Response.StatusCode == 200)
            {
                var tokens = antiforgery.GetAndStoreTokens(context);
                context.Response.Cookies.Append("X-CSRF-TOKEN", tokens.RequestToken!, new CookieOptions
                {
                    HttpOnly = true, // Ensure the cookie is HTTP only
                    SameSite = SameSiteMode.Strict, // Set appropriate SameSite policy
                    Secure = true // Ensure the cookie is only sent over HTTPS in production
                });
            }

            await _next(context);
        }
    }

    public static class AntiforgeryMiddlewareExtensions
    {
        public static IApplicationBuilder UseAntiforgeryTokens(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AntiforgeryMiddleware>();
        }
    }
}
