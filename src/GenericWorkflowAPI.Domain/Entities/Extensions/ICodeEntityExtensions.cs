using System;

namespace GenericWorkflowAPI.Domain.Entities.Extensions
{
    public static class ICodeEntityExtensions
    {
        public static void FillEntity(this ICodeEntity codeEntity, long? ticks = null, string? suffix = null, string? typeName = null)
        {
            if (ticks == null)
                ticks = DateTime.Now.Ticks;
            if (typeName == null)
                typeName = codeEntity.GetType().Name;

            codeEntity.Code = $"{typeName}{nameof(codeEntity.Code)}{ticks}{suffix}";
        }
    }
}