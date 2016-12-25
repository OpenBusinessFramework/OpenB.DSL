using System;

namespace OpenB.DSL
{
    internal class FieldExpression : IExpression
    {
        ParserContext parserContext;
        string fieldName;

        public FieldExpression(ParserContext context, string fieldName)
        {
            parserContext = context;
            this.fieldName = fieldName;
        }

        public object Evaluate()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return fieldName;
        }

        public string GenerateCode()
        {
            // TODO: All
            return $"field";
        }
    }
}