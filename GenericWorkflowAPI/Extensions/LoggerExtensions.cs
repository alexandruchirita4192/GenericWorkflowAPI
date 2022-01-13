using System;
using GenericWorkflowAPI.Domain;
using GenericWorkflowAPI.Domain.Constants;
using Serilog;

namespace GenericWorkflowAPI.Extensions
{
    public static class LoggerExtensions
    {
        public static void ErrorEx(
            this ILogger logger,
            Exception ex,
            string? className,
            string methodName)
        {
            logger.Error(ex,
                LogConstants.SerilogTemplateExceptionWithoutParameter,
                className,
                methodName);
        }

        public static void ErrorEx(
            this ILogger logger,
            Exception ex,
            string? className,
            string methodName,
            object? parameterValue)
        {
            logger.Error(ex,
                LogConstants.SerilogTemplateExceptionWithParameter,
                className,
                methodName,
                parameterValue);
        }

        public static void ErrorEx(
            this ILogger logger,
            Exception ex,
            string? className,
            string methodName,
            object? parameterValue,
            long? userId)
        {
            logger.Error(ex,
                LogConstants.SerilogTemplateExceptionWithParameterAndUser,
                className,
                methodName,
                parameterValue,
                userId);
        }

        public static void ErrorEx(
            this ILogger logger,
            Exception ex,
            string? className,
            string methodName,
            object? parameterValue,
            IdentityUser? user)
        {
            logger.ErrorEx(ex,
                className,
                methodName,
                parameterValue,
                user?.Id);
        }
    }
}
