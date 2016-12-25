using System;

namespace OpenB.DSL
{

    internal class ArgumentExpression : IExpression
    {
        private object tokenValue;

        public ArgumentExpression(object tokenValue)
        {
            this.tokenValue = tokenValue;
        }

        public object Evaluate()
        {
            throw new NotImplementedException();
        }

        public string GenerateCode()
        {
            throw new NotImplementedException();
        }
    }
}