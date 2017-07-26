using System;

namespace OpenB.DSL.Expressions
{
    internal class EqualExpression : BaseEqualityExpression, IEQualityExpression
    {
        public IExpression Left { get; }
        public IExpression Right { get; }

        public EqualExpression(IExpression leftSide, IExpression rightSide)
        {
            this.Left = leftSide;
            this.Right = rightSide;
        }

        public object Evaluate()
        {
            double leftValue = Convert.ToDouble(Left.Evaluate());
            double rightValue = Convert.ToDouble(Right.Evaluate());

            return nearlyEqual(leftValue, rightValue, 0.00000000001d);
        }

        public string GenerateCode()
        {
            return $"({Left.GenerateCode()}) == ({Right.GenerateCode()})";
        }

        public override string ToString()
        {
            return string.Concat(Left.ToString(), " = ", Right.ToString());
        }

    }
}