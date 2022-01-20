using System;
using System.Reflection;

namespace GenericWorkflowAPI.Core.AutoMapper
{
    public class ReflectionMappingInfo
    {
        public const string PropertyNameSuffixId = "Id";
        public const string PropertyNameSuffixCode = "Code";

        public ReflectionMappingInfo(Type entityType, string basePropertyName, PropertyInfo entityPropertyInfoId)
        {
            EntityType = entityType;
            BasePropertyName = basePropertyName;
            EntityPropertyInfoId = entityPropertyInfoId;
            DtoPropertyNameCode = $"{BasePropertyName}{PropertyNameSuffixCode}";
        }

        public Type EntityType { get; private set; }
        public string BasePropertyName { get; private set; }

        public string DtoPropertyNameCode { get; private set; }

        public PropertyInfo EntityPropertyInfoId { get; private set; }
        public PropertyInfo EntityPropertyInfoClass { get; set; }
        public PropertyInfo DtoPropertyInfoCode { get; set; }
    }
}