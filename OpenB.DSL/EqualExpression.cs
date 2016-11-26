using System;

namespace OpenB.DSL
{
    internal class EqualExpression : BaseEqualityExpression, IEQualityExpression
    {
        private object left;
        private object right;

        public EqualExpression(object left, object right)
        {
            this.left = left;
            this.right = right;
        }

        public object Evaluate()
        {
            double leftValue = Convert.ToDouble(left);
            double rightValue = Convert.ToDouble(right);


            return nearlyEqual(leftValue, rightValue, 0.00000000001d);
        }

       
    }
}