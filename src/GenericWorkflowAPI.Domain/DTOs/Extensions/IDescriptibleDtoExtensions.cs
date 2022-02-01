using System;

namespace GenericWorkflowAPI.Domain.DTOs.Extensions
{
    public static class IDescriptibleEntityExtensions
    {
        public static void FillDto(this IDescriptibleDto descriptibleDto, long? ticks = null, string? suffix = null, string? typeName = null)
        {
            if (ticks == null)
                ticks = DateTime.Now.Ticks;
            if (typeName == null)
                typeName = descriptibleDto.GetType().Name;

            ((ICodeDto)descriptibleDto).FillDto(ticks, suffix, typeName);

            descriptibleDto.Name = $"{typeName}{nameof(descriptibleDto.Name)}{ticks}{suffix}";
            descriptibleDto.Description = $"{typeName}{nameof(descriptibleDto.Description)}{ticks}{suffix}";
        }
    }
}