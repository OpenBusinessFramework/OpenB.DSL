using System;

namespace OpenB.DSL
{
    internal class LogicalOrExpression : IEQualityExpression
    {
        private IExpression leftHandExpression;
        private IExpression rightHandExpression;

        public LogicalOrExpression(IExpression leftHandExpression, IExpression rightHandExpression)
        {
            this.leftHandExpression = leftHandExpression;
            this.rightHandExpression = rightHandExpression;
        }

        public object Evaluate()
        {
            return (bool)leftHandExpression.Evaluate() || (bool)rightHandExpression.Evaluate();
        }

        public override string ToString()
        {
            return $"{leftHandExpression} or {rightHandExpression}";
        }

        public string GenerateCode()
        {
            return $"({leftHandExpression.GenerateCode()}) || ({rightHandExpression.GenerateCode()})";
        }
    }
}