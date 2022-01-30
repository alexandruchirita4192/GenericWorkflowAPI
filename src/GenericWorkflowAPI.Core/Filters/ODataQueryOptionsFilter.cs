using System.Linq;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace GenericWorkflowAPI.Core.Filters
{
    public class ODataQueryOptionsFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var queryAttribute = GetQueryAttributeFrom(context);

            var stringSchema = context.SchemaGenerator.GenerateSchema(typeof(string), context.SchemaRepository);

            if (queryAttribute != null)
            {
                AddOperationParameters(
                    operation, queryAttribute, stringSchema,
                    AllowedQueryOptions.Select,
                    "$select",
                    "Selects which properties to include in the response.");

                AddOperationParameters(
                    operation, queryAttribute, stringSchema,
                    AllowedQueryOptions.Expand,
                    "$expand",
                    "Expands related entities inline.");

                // Additional OData query options are available for collections of entities only
                AddOperationParameters(
                    operation, queryAttribute, stringSchema,
                    AllowedQueryOptions.Filter,
                    "$filter",
                    "Filters the results, based on a Boolean condition.");

                AddOperationParameters(
                    operation, queryAttribute, stringSchema,
                    AllowedQueryOptions.OrderBy,
                    "$orderby",
                    "Request resources in a particular order.");

                AddOperationParameters(
                    operation, queryAttribute, stringSchema,
                    AllowedQueryOptions.Top,
                    "$top",
                    "Requests the number of items to be included in the result.");

                AddOperationParameters(
                    operation, queryAttribute, stringSchema,
                    AllowedQueryOptions.Skip,
                    "$skip",
                    "Requests the number of items that are to be skipped.");

                AddOperationParameters(
                    operation, queryAttribute, stringSchema,
                    AllowedQueryOptions.Count,
                    "$count",
                    "Request the count of the resources.");
            }
        }

        private static EnableQueryAttribute? GetQueryAttributeFrom(OperationFilterContext context)
        {
            var customAttributesArray = context.MethodInfo.GetCustomAttributes(true);

            if (context.MethodInfo.DeclaringType != null)
            {
                customAttributesArray = customAttributesArray.Union(context.MethodInfo.DeclaringType.GetCustomAttributes(true)).ToArray();
            }

            var queryAttribute = customAttributesArray.OfType<EnableQueryAttribute>().FirstOrDefault();
            return queryAttribute;
        }

        private static void AddOperationParameters(
            OpenApiOperation operation,
            EnableQueryAttribute queryAttribute,
            OpenApiSchema stringSchema,
            AllowedQueryOptions allowedQueryOptions,
            string name,
            string description)
        {
            if (queryAttribute.AllowedQueryOptions.HasFlag(allowedQueryOptions))
            {
                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = name,
                    In = ParameterLocation.Query,
                    Description = description,
                    Schema = stringSchema
                });
            }
        }
    }
}