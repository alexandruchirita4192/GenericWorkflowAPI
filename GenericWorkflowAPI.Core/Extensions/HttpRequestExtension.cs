using System;
using Microsoft.AspNetCore.Http;

namespace GenericWorkflowAPI.IdentityServer4.Extensions
{
    public static class HttpRequestExtension
    {
        /// <summary>
        /// Ajax request check.
        /// 
        /// Checks if a request has a header "X-Requested-With" with value "XMLHttpRequest".
        /// </summary>
        /// <remarks>
        /// This doesn't demonstrate a request is Ajax or not, it just require Ajax requests to be decorated with the header "X-Requested-With" with value "XMLHttpRequest".
        /// </remarks>
        public static bool IsAjax(this HttpRequest request, string httpVerb = "")
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "Request object is null.");
            }

            if (!string.IsNullOrEmpty(httpVerb) && string.Compare(request.Method, httpVerb, true) != 0)
            {
                return false;
            }
            
            return request.Headers != null ? string.Compare(request.Headers["X-Requested-With"], "XMLHttpRequest", true) == 0 : false;
        }
    }
}
