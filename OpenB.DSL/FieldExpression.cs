using System;

namespace OpenB.DSL
{
    internal class FieldExpression : IExpression
    {
        ParserContext parserContext;
        public string FieldName { get; private set; }

        public FieldExpression(ParserContext context, string fieldName)
        {
            parserContext = context;
            FieldName = fieldName;
        }

        public object Evaluate()
        {
            throw new NotImplementedException();
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