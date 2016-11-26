using System;

namespace OpenB.DSL
{
    internal class NotEqualExpression : BaseEqualityExpression, IEQualityExpression
    {
        private double leftHand;
        private double rightHand;

        public NotEqualExpression(double leftHand, double rightHand)
        {
            this.leftHand = leftHand;
            this.rightHand = rightHand;
        }

        public object Evaluate()
        {
            double leftValue = Convert.ToDouble(leftHand);
            double rightValue = Convert.ToDouble(rightHand);

            return !nearlyEqual(leftValue, rightValue, 0.00000000001d);
        }

        public override string ToString()
        {
            return $"{leftHand} != {rightHand}";
        }
    }
}