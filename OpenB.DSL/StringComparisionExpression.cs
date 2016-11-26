using System;

namespace OpenB.DSL
{
    internal class StringComparisionNotEqualExpression : IEQualityExpression
    {
        private string left;
        private string right;

        public StringComparisionNotEqualExpression(string left, string right)
        {
            this.left = left;
            this.right = right;
         
        }

        public object Evaluate()
        {
            return !left.Equals(right);
        }
    }

    internal class StringComparisionIsEqualExpression : IEQualityExpression
    {
        private string left;
        private string right;

        public StringComparisionIsEqualExpression(string left, string right)
        {
            this.left = left;
            this.right = right;

        }

        public object Evaluate()
        {
            return left.Equals(right);
        }
    }
}