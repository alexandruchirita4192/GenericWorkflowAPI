using System;
using System.Reflection;

namespace GenericWorkflowAPI.Helpers
{
    /// <summary>
    /// A static class for reflection type functions
    /// </summary>
    public static class ReflectionHelper
    {
        /// <summary>
        /// Extension for 'Object' that copies the properties to a destination object.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        public static void CopyProperties(this object source, object destination)
        {
            // If any this null throw an exception
            if (source == null)
                throw new ArgumentNullException(nameof(source), "Cannot copy from a null object.");
            if (destination == null)
                throw new ArgumentNullException(nameof(destination), "Cannot copy to a null object.");

            // Getting the Types of the objects
            var typeDest = destination.GetType();
            var typeSrc = source.GetType();

            // Iterate the Properties of the source instance and
            // populate them from their desination counterparts
            PropertyInfo[] srcProps = typeSrc.GetProperties();
            foreach (PropertyInfo srcProp in srcProps)
            {
                if (!srcProp.CanRead)
                {
                    continue;
                }
                var targetProperty = typeDest.GetProperty(srcProp.Name);
                if (targetProperty == null)
                {
                    continue;
                }
                if (!targetProperty.CanWrite)
                {
                    continue;
                }
                if (targetProperty.GetSetMethod(true)?.IsPrivate ?? false)
                {
                    continue;
                }
                if ((targetProperty.GetSetMethod()?.Attributes & MethodAttributes.Static ?? 0) != 0)
                {
                    continue;
                }
                if (!targetProperty.PropertyType.IsAssignableFrom(srcProp.PropertyType))
                {
                    continue;
                }

                // Passed all tests, lets set the value
                targetProperty.SetValue(destination, srcProp.GetValue(source, null), null);
            }
        }
    }
}