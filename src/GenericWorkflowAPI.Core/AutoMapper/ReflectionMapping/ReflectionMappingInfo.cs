using System;
using System.Reflection;
using GenericWorkflowAPI.Core.Services;

namespace GenericWorkflowAPI.Core.AutoMapper
{
    /// <summary>
    /// Class describing a mapping between an TEntity entity of type <see cref="EntityType"/> and a TDto using a mapping between
    /// - TEntity property id {Property}Id (<see cref="BasePropertyName"/> <see cref="PropertyNameSuffixId"/> with property info <see cref="EntityPropertyInfoId"/>),
    /// - TDto property code {Property}Code (<see cref="DtoPropertyNameCode"/> with property info <see cref="DtoPropertyInfoCode"/>),
    /// using TEntity property instance {Property} (<see cref="BasePropertyName"/> with property info <see cref="EntityPropertyInfoClass"/>).
    /// 
    /// This class helps map:
    /// - TDto code property {Property}Code value based on TEntity property instance {Property}.Code
    /// - TEntity id {Property}Id value based on TDto property {Property}.Id using <see cref="IGenericCodeRepository"/> instance created for property info <see cref="EntityPropertyInfoClass"/>
    /// </summary>
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
        public PropertyInfo? EntityPropertyInfoClass { get; set; }
        public PropertyInfo? DtoPropertyInfoCode { get; set; }
    }
}