using System;

namespace OpenB.DSL.Expressions
{
    internal class NotEqualExpression : BaseEqualityExpression, IEQualityExpression
    {
        public IExpression Left { get; }
        public IExpression Right { get; }

        public NotEqualExpression(IExpression leftHand, IExpression rightHand)
        {
            this.Left = leftHand;
            this.Right = rightHand;
        }

        public object Evaluate()
        {
            double leftValue = Convert.ToDouble(Left.Evaluate());
            double rightValue = Convert.ToDouble(Right.Evaluate());

            return !nearlyEqual(leftValue, rightValue, 0.00000000001d);
        }

        public override string ToString()
        {
            return $"{Left} != {Right}";
        }

        public string GenerateCode()
        {
            return $"!({Left}).Equals({Right})";
        }
    }
}