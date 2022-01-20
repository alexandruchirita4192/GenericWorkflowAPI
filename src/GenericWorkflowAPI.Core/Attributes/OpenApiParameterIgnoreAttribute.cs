using System;

namespace GenericWorkflowAPI.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.GenericParameter)]
    public class OpenApiParameterIgnoreAttribute : Attribute
    {
    }
}