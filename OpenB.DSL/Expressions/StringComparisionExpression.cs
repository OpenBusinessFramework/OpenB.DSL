using System;

namespace OpenB.DSL.Expressions
{
    internal class StringComparisionNotEqualExpression : IEQualityExpression
    {
        public IExpression Left { get; }
        public IExpression Right { get; }

        public StringComparisionNotEqualExpression(IExpression left, IExpression right)
        {
            this.Left = left;
            this.Right = right;
         
        }

        public object Evaluate()
        {
            return !Left.Evaluate().Equals(Right.Evaluate());
        }

        public string GenerateCode()
        {
            return $"!({Left.GenerateCode()}).Equals({Right.GenerateCode()})";
        }

        public override string ToString()
        {
            return $"{Left} != {Right}";
        }
    }
}