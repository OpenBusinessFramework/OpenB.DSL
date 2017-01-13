using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using OpenB.Core;

namespace OpenB.DSL.Reflection
{
    public class ModelEvaluator
    {
        object context;
        private Assembly[] referencedAssemblies;

        public ModelEvaluator(Assembly[] referencedAssemblies)
        {
            if (referencedAssemblies == null)
                throw new System.ArgumentNullException(nameof(referencedAssemblies));

            this.referencedAssemblies = referencedAssemblies;
        }

        public void SetModelContext(object context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            this.context = context;
        }

        public object Evaluate(string path)
        {
            string[] splittedPath = path.Split('.');
            object currentObject = context;
            object returnValue = null;

            for (int x = 0; x < splittedPath.Length; x++)
            {
                PropertyInfo relevantProperty = currentObject.GetType().GetProperty(splittedPath[x]);
                if (relevantProperty == null)
                {
                    throw new System.Exception($"Property {splittedPath[x]} does not exist.");
                }

                returnValue = relevantProperty.GetValue(context);
            }

            return returnValue;
        }

        public static ModelEvaluator GetInstance(string assemblyFolder)
        {
            if (assemblyFolder == null)
                throw new ArgumentNullException(nameof(assemblyFolder));

            DirectoryInfo assemblyDirectoryInfo = new DirectoryInfo(assemblyFolder);
            if (!assemblyDirectoryInfo.Exists)
            {
                throw new Exception($"Assembly folder {assemblyFolder} does not exists.");                
            }

            IList<Assembly> loadedAssemblies = new List<Assembly>();

            foreach(var dynamicLinkLibrairy in assemblyDirectoryInfo.GetFiles("*.dll"))
            {
                loadedAssemblies.Add(Assembly.LoadFile(dynamicLinkLibrairy.FullName));
            }

            return new ModelEvaluator(loadedAssemblies.ToArray());
        }

        internal object Evaluate(string modelType, string[] contextPath)
        {
            if (context == null)
            {
                throw new Exception("Cannot evaluate on null context.");
            }

            Type relevantType = LoadTypeFromAssembly(contextPath[0], typeof(IModel));
            if (relevantType == null)
            {
                throw new Exception($"Modeltype {modelType} could not be loaded");
            }

            Type repositoryType = typeof(Repository<>).MakeGenericType(new[] { relevantType });

            IList<string> memberPath = new List<string>();
            memberPath = contextPath.ToList();
            memberPath.RemoveAt(0);

            Object currentValue = context;
            Type currentType = relevantType;
            PropertyInfo currentProperty;
            foreach (var member in memberPath)
            {
                currentProperty = currentType.GetProperty(member);
                currentValue = currentProperty.GetValue(currentValue);
                currentType = currentProperty.DeclaringType;
            }

            return currentValue;
        }

        private Type LoadTypeFromAssembly(string typeName, Type interfaceType)
        {
            return referencedAssemblies.SelectMany(a => a.GetTypes()).SingleOrDefault(t => t.Name.Equals(typeName) && interfaceType.IsAssignableFrom(t) && t.IsPublic);

        }
    }
}