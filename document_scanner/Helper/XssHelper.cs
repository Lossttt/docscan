using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Text.Encodings.Web;

namespace document_scanner.Helpers
{
    public static class XssHelper
    {
        public static IHtmlContent SanitizeHtml(string htmlContent)
        {
            if (string.IsNullOrEmpty(htmlContent))
            {
                return HtmlString.Empty;
            }

            var htmlEncoder = HtmlEncoder.Default;
            var sanitizedHtml = htmlEncoder.Encode(htmlContent);
            return new HtmlString(sanitizedHtml);
        }

        public static IHtmlContent SanitizeModel<TModel>(IHtmlHelper<TModel> htmlHelper, TModel model)
        {
            var htmlContent = htmlHelper.DisplayForModel(model).ToString();
            return SanitizeHtml(htmlContent);
        }
    }
}
