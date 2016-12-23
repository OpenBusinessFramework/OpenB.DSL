using System.Reflection;

namespace OpenB.DSL.Reflection
{
    public class ModelEvaluator
    {
        readonly object context;

        public ModelEvaluator(object context)
        {
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
    }   
}