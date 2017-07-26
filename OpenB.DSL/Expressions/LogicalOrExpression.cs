using System;

namespace OpenB.DSL.Expressions
{
    internal class LogicalOrExpression : IEQualityExpression
    {
        public IExpression Left { get; }
        public IExpression Right { get; }

        public LogicalOrExpression(IExpression leftHandExpression, IExpression rightHandExpression)
        {
            this.Left = leftHandExpression;
            this.Right = rightHandExpression;
        }

        public object Evaluate()
        {
            return (bool)Left.Evaluate() || (bool)Right.Evaluate();
        }

        public override string ToString()
        {
            return $"{Left} or {Right}";
        }

        public string GenerateCode()
        {
            return $"({Left.GenerateCode()}) || ({Right.GenerateCode()})";
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