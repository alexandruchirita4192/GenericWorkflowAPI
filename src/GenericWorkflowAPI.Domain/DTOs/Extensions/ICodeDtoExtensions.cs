using System;

namespace GenericWorkflowAPI.Domain.DTOs.Extensions
{
    public static class ICodeEntityExtensions
    {
        public static void FillDto(this ICodeDto codeDto, long? ticks = null, string? suffix = null, string? typeName = null)
        {
            if (ticks == null)
                ticks = DateTime.Now.Ticks;
            if (typeName == null)
                typeName = codeDto.GetType().Name;

            codeDto.Code = $"{typeName}{nameof(codeDto.Code)}{ticks}{suffix}";
        }
    }
}