using System;

namespace OpenB.DSL
{
    internal class FieldExpression : IExpression
    {
        private string contents;
      

        public FieldExpression(string contents)
        {          
            this.contents = contents;
        }

        public object Evaluate()
        {
            throw new NotImplementedException();
        }
    }
}