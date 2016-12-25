using System;

namespace OpenB.DSL
{
    internal class EqualExpression : BaseEqualityExpression, IEQualityExpression
    {
        private IExpression left;
        private IExpression right;

        public EqualExpression(IExpression leftSide, IExpression ightSide)
        {
            this.left = leftSide;
            this.right = ightSide;
        }

        public object Evaluate()
        {
            double leftValue = Convert.ToDouble(left.Evaluate());
            double rightValue = Convert.ToDouble(right.Evaluate());


            return nearlyEqual(leftValue, rightValue, 0.00000000001d);
        }

        public string GenerateCode()
        {
            return $"({left.GenerateCode()}) == ({right.GenerateCode()})";
        }

        public override string ToString()
        {
            return string.Concat(left.ToString(), " = ", right.ToString());
        }

    }
}