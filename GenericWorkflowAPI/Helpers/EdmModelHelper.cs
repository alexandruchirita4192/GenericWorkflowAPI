using System.Linq;
using GenericWorkflowAPI.Domain.DTOs;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace GenericWorkflowAPI.Helpers
{
    public static class EdmModelHelper
    {
        /// <summary>
        /// Get <see cref="IEdmModel"/> with entities implementing <see cref="IBaseDto"/> from assembly "GenericWorkflowAPI.Database".
        /// </summary>
        public static IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();

            var assembly = typeof(WorkflowDto).Assembly;
            if (assembly != null)
            {
                var validDtoTypes = assembly.GetTypes().Where(t => typeof(IBaseDto).IsAssignableFrom(t));

                foreach (var dtoType in validDtoTypes)
                    builder.AddEntityType(dtoType);
            }

            return builder.GetEdmModel();
        }
    }
}