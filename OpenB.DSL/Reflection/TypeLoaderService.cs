using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace OpenB.DSL.Reflection
{

    public class TypeLoaderService
    {
        public TypeLoaderServiceConfiguration Configuration { get; set; }

        public TypeLoaderService(TypeLoaderServiceConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            Configuration = configuration;
        }
        public Type TryLoadType(string typeName)
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type foundType = assembly.GetTypes().Where(t => t.Name == typeName).SingleOrDefault();
                if (foundType != null)
                {
                    return foundType;
                }
            }

            throw new Exception($"Type {typeName} could not be loaded.");
        }

        public Type[] GetTypesImplementing(Type[] interfaces)
        {
            List<Type> result = new List<Type>();

            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                result.AddRange(assembly.GetTypes().Where(t => interfaces.All(i => i.IsAssignableFrom(t)) && !t.IsInterface));
            }

            return result.ToArray();
        }

        public Type TryLoadType(string typeName, Type[] interfaces)
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type foundType = assembly.GetTypes().Where(t => t.Name == typeName && interfaces.Any(i => i.IsAssignableFrom(t))).SingleOrDefault();
                if (foundType != null)
                {
                    return foundType;
                }
            }

            throw new Exception($"Type {typeName} could not be loaded.");
        }
    }
}