using System;

namespace OpenB.DSL.Expressions
{
    internal class SymbolExpression : IEQualityExpression
    {
        public IExpression Left => throw new NotImplementedException();

        public IExpression Right => throw new NotImplementedException();

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