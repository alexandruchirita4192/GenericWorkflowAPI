using System.Linq;
using GenericWorkflowAPI.Database;
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

            var assembly = typeof(ApplicationDbContext).Assembly; // GenericWorkflowAPI.Database
            if (assembly != null)
            {
                var validEntityTypes = assembly.GetTypes().Where(t => typeof(IBaseDto).IsAssignableFrom(t));

                foreach (var entityType in validEntityTypes)
                    builder.AddEntityType(entityType);
            }

            return builder.GetEdmModel();
        }
    }
}