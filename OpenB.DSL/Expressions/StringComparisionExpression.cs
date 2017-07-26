using System;

namespace OpenB.DSL.Expressions
{
    internal class StringComparisionNotEqualExpression : IEQualityExpression
    {
        private IExpression left;
        private IExpression right;

        public StringComparisionNotEqualExpression(IExpression left, IExpression right)
        {
            this.left = left;
            this.right = right;
         
        }

        public object Evaluate()
        {
            return !left.Evaluate().Equals(right.Evaluate());
        }

        public string GenerateCode()
        {
            return $"!({left.GenerateCode()}).Equals({right.GenerateCode()})";
        }

        public override string ToString()
        {
            return $"{left} != {right}";
        }
    }
}