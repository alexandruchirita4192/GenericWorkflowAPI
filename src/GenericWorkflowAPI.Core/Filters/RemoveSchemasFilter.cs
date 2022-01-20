using System;
using System.Collections.Generic;
using GenericWorkflowAPI.Core.AutoMapper;
using GenericWorkflowAPI.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace GenericWorkflowAPI.Core.Filters
{
    public class RemoveSchemasFilter : IDocumentFilter
    {
        private List<string> _dtos;

        public RemoveSchemasFilter(List<EntityDtoMapping> mappingList)
        {
            if (mappingList == null)
                throw new ArgumentNullException(nameof(mappingList));

            _dtos = new List<string>();

            foreach (var mapping in mappingList)
            {
                foreach (var item in mapping.Mapping.Values)
                    _dtos.Add(item.Name);
            }

            _dtos.Add(typeof(ExecuteWorkflowRequestDto).Name);
            _dtos.Add(typeof(ProblemDetails).Name);
        }

        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            IDictionary<string, OpenApiSchema> schemas = swaggerDoc.Components.Schemas;
            foreach (KeyValuePair<string, OpenApiSchema> _item in schemas)
            {
                if (!_dtos.Contains(_item.Key))
                    swaggerDoc.Components.Schemas.Remove(_item.Key);
            }
        }
    }
}