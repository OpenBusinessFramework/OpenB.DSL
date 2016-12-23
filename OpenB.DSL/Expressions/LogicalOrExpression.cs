using System;

namespace OpenB.DSL
{
    internal class LogicalOrExpression : IEQualityExpression
    {
        private IExpression leftHandBoolean;
        private IExpression rightHandBoolean;

        public LogicalOrExpression(IExpression leftHandExpression, IExpression rightHandExpression)
        {
            this.leftHandBoolean = leftHandExpression;
            this.rightHandBoolean = rightHandExpression;
        }

        public object Evaluate()
        {
            return (bool)leftHandBoolean.Evaluate() || (bool)rightHandBoolean.Evaluate();
        }
    }
}