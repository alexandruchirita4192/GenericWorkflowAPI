namespace GenericWorkflowAPI.Domain.Constants
{
    public static class LogConstants
    {
        public const string SerilogTemplateExceptionWithoutParameter = "{className}.{methodName}() exception occured";
        public const string SerilogTemplateExceptionWithParameter = "{className}.{methodName}({parameterValue}) exception occured";
        public const string SerilogTemplateExceptionWithParameterAndUser = "{className}.{methodName}({parameterValue}) from {userId} exception occured";
        public const string SerilogTemplateExceptionMediatorHelper = "{methodName} exception with parameters " +
            "{requestType}, {responseType}, {interfaceType}, {implementedType}, {mappings}";
    }
}