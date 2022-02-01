using System;

namespace GenericWorkflowAPI.Domain.Entities.Extensions
{
    public static class IDescriptibleEntityExtensions
    {
        public static void FillEntity(this IDescriptibleEntity descriptibleEntity, long? ticks = null, string? suffix = null, string? typeName = null)
        {
            if (ticks == null)
                ticks = DateTime.Now.Ticks;
            if (typeName == null)
                typeName = descriptibleEntity.GetType().Name;

            ((ICodeEntity)descriptibleEntity).FillEntity(ticks, suffix, typeName);

            descriptibleEntity.Name = $"{typeName}{nameof(descriptibleEntity.Name)}{ticks}{suffix}";
            descriptibleEntity.Description = $"{typeName}{nameof(descriptibleEntity.Description)}{ticks}{suffix}";
        }
    }
}