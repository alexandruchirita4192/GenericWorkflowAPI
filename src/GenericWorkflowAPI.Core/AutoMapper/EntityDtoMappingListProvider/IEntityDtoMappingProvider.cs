using System.Reflection;

namespace GenericWorkflowAPI.Core.AutoMapper
{
    public interface IEntityDtoMappingProvider
    {
        EntityDtoMapping GetEntityDtoMapping(Assembly assembly);
    }
}