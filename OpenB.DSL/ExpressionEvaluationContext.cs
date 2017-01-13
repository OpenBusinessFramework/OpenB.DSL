using System;
using OpenB.DSL.Reflection;

namespace OpenB.DSL
{
    public class ExpressionEvaluationContext
    {
        internal ModelEvaluator ModelEvaluator { get; private set; }

        public ExpressionEvaluationContext(ModelEvaluator modelEvaluator)
        {
            if (modelEvaluator == null)
                throw new ArgumentNullException(nameof(modelEvaluator));

            this.ModelEvaluator = modelEvaluator;
        }
    }
}