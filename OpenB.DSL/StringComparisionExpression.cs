using System;

namespace OpenB.DSL
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
    }

    internal class StringComparisionIsEqualExpression : IEQualityExpression
    {
        private IExpression left;
        private IExpression right;

        public StringComparisionIsEqualExpression(IExpression left, IExpression right)
        {
            this.left = left;
            this.right = right;

        }

        public object Evaluate()
        {
            return left.Evaluate().Equals(right.Evaluate());
        }
    }
}