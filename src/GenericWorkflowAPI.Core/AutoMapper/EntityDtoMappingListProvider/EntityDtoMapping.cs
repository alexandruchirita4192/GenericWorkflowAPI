using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;

namespace GenericWorkflowAPI.Core.AutoMapper
{
    public class EntityDtoMapping
    {
        public Assembly Assembly { get; private set; }
        public Dictionary<Type, Type> Mapping { get; private set; }

        /// <summary>
        /// Empty mapping for an assembly
        /// </summary>
        public EntityDtoMapping(Assembly assembly)
        {
            if (assembly == null)
                throw new ArgumentNullException(nameof(assembly));

            Assembly = assembly;
            Mapping = new Dictionary<Type, Type>();
        }

        /// <summary>
        /// Mapping created from cache
        /// </summary>
        public EntityDtoMapping(ConcurrentDictionary<Assembly, Dictionary<Type, Type>> cache, Assembly assembly)
        {
            if (cache == null)
                throw new ArgumentNullException(nameof(cache));
            if (assembly == null)
                throw new ArgumentNullException(nameof(assembly));
            if (cache.Count == 0)
                throw new ArgumentException(nameof(cache), $"Empty cache passed to {nameof(EntityDtoMapping)}");
            if (!cache.ContainsKey(assembly))
                throw new ArgumentException(nameof(cache), $"Assembly {assembly.FullName} missing from cache argument passed to {nameof(EntityDtoMapping)}");

            Dictionary<Type, Type> mapping;
            
            if (!cache.TryGetValue(assembly, out mapping))
                throw new ArgumentException(nameof(cache), $"Couldn't get value for assembly {assembly.FullName} out of cache argument passed to {nameof(EntityDtoMapping)}");
            
            Mapping = mapping;
        }
    }
}