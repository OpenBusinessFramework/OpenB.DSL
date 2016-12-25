using System;

namespace OpenB.DSL
{
    internal class LogicalAndExpression : IEQualityExpression
    {
        private IExpression leftHandExpression;
        private IExpression rightHandExpression;

        public LogicalAndExpression(IExpression leftHandExpression, IExpression rightHandExpression)
        {
            this.leftHandExpression = leftHandExpression;
            this.rightHandExpression = rightHandExpression;
        }

        public object Evaluate()
        {
            return (bool)leftHandExpression.Evaluate() && (bool)rightHandExpression.Evaluate();
        }

        public override string ToString()
        {
            return $"{leftHandExpression} and ${rightHandExpression}";
        }

        public string GenerateCode()
        {
            return $"({leftHandExpression.GenerateCode()}) && ({rightHandExpression.GenerateCode()})";
        }
    }
}