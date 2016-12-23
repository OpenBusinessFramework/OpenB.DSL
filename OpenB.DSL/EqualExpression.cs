using System;

namespace OpenB.DSL
{
    internal class EqualExpression : BaseEqualityExpression, IEQualityExpression
    {
        private IExpression left;
        private IExpression right;

        public EqualExpression(IExpression left, IExpression right)
        {
            this.left = left;
            this.right = right;
        }

        public object Evaluate()
        {
            double leftValue = Convert.ToDouble(left.Evaluate());
            double rightValue = Convert.ToDouble(right.Evaluate());


            return nearlyEqual(leftValue, rightValue, 0.00000000001d);
        }

       
    }
}