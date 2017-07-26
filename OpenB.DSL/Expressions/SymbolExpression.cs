using System;

namespace OpenB.DSL.Expressions
{
    internal class SymbolExpression : IEQualityExpression
    {
        public object Evaluate()
        {
            throw new NotImplementedException();
        }

        public string GenerateCode()
        {
            throw new NotSupportedException();
        }
    }
}