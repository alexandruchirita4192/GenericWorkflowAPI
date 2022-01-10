using System;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace GenericWorkflowAPI.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class SerilogLoggingAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var diagnosticContext = context?.HttpContext?.RequestServices?.GetService<IDiagnosticContext>();
            diagnosticContext?.Set("ActionName", context?.ActionDescriptor?.DisplayName);
            diagnosticContext?.Set("ActionId", context?.ActionDescriptor?.Id);
        }
    }
}