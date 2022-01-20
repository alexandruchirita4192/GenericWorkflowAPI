using System;
using System.Collections.Generic;
using AutoMapper;

namespace GenericWorkflowAPI.AutoMapper
{
    public class EntitiesToDtosProfile : Profile
    {
        public EntitiesToDtosProfile(Dictionary<Type,Type> mapping)
        {
            if (mapping == null || mapping.Count == 0)
            {
                throw new ArgumentException( $"Empty mapping argument passed to {nameof(EntitiesToDtosProfile)} profile", nameof(mapping));
            }

            foreach(var item in mapping)
            {
                // Create AutoMapper mapping both ways based on those types
                CreateMap(item.Key, item.Value); // Entity-DTO
                CreateMap(item.Value, item.Key); // DTO-Entity
            }
        }
    }
}
