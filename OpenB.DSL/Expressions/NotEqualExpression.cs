using System;

namespace OpenB.DSL.Expressions
{
    internal class NotEqualExpression : BaseEqualityExpression, IEQualityExpression
    {
        private IExpression leftHand;
        private IExpression rightHand;

        public NotEqualExpression(IExpression leftHand, IExpression rightHand)
        {
            this.leftHand = leftHand;
            this.rightHand = rightHand;
        }

        public object Evaluate()
        {
            double leftValue = Convert.ToDouble(leftHand.Evaluate());
            double rightValue = Convert.ToDouble(rightHand.Evaluate());

            return !nearlyEqual(leftValue, rightValue, 0.00000000001d);
        }

        public override string ToString()
        {
            return $"{leftHand} != {rightHand}";
        }

        public string GenerateCode()
        {
            return $"!({leftHand}).Equals({rightHand})";
        }
    }
}