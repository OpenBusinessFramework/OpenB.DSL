using System;

namespace OpenB.DSL.Expressions
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

    

    public interface ICodeGenerator
    {
        // LinQ : .Where((left) || (right))
        // SQL: where (left) or (right)
        // XML: [(left) or (right)]

        string GenerateCode();
    }
}