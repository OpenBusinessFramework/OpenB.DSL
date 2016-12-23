using System;

namespace OpenB.DSL
{
    internal class LogicalAndExpression : IEQualityExpression
    {
        private IExpression leftHandBoolean;
        private IExpression rightHandBoolean;

        public LogicalAndExpression(IExpression leftHandBoolean, IExpression rightHandBoolean)
        {
            this.leftHandBoolean = leftHandBoolean;
            this.rightHandBoolean = rightHandBoolean;
        }

        public object Evaluate()
        {
            return (bool)leftHandBoolean.Evaluate() && (bool)rightHandBoolean.Evaluate();
        }
    }
}