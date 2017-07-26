using System;
using OpenB.DSL.Handlers;
using OpenB.DSL.Reflection;

namespace OpenB.DSL.Expressions
{   
    class FieldExpression : IExpression
    {
        public ModelEvaluator ModelEvaluator { get; private set; }
        public string FieldName { get; private set; }

        public FieldExpression(ModelEvaluator modelEvalutor, string fieldName)
        {
            if (modelEvalutor == null)
                throw new ArgumentNullException(nameof(modelEvalutor));
            ModelEvaluator = modelEvalutor;
            FieldName = fieldName;
        }

        public object Evaluate()
        {
            return ModelEvaluator.Evaluate(FieldName);
        }

        public override string ToString()
        {
            return FieldName;
        }

        public string GenerateCode()
        {
            // TODO: All
            return $"field";
        }
    }
}