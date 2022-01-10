using System;
using System.Collections.Generic;
using System.Reflection;

namespace GenericWorkflowAPI.Core.AutoMapper.Helpers
{
    public interface IEntityDtoMappingProvider
    {
        Dictionary<Type, Type> GetEntityDtoMapping(Assembly assembly);
    }
}