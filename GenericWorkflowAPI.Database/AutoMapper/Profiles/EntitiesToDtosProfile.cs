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
                throw new InvalidOperationException("Empty mapping for EntitiesToDtosProfile profile");
            }

            foreach(var item in mapping)
            {
                CreateMap(item.Key, item.Value);
                CreateMap(item.Value, item.Key);
            }
        }
    }
}
