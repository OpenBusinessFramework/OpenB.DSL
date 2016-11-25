using System;

namespace OpenB.DSL
{
    internal class EqualExpression : IEQualityExpression
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

        public static bool nearlyEqual(double a, double b, double epsilon)
        {
             double absA = Math.Abs(a);
             double absB = Math.Abs(b);
             double diff = Math.Abs(a - b);

            if (a == b)
            { // shortcut, handles infinities
                return true;
            }
            else if (a == 0 || b == 0 || diff < Double.MinValue)
            {
                // a or b is zero or both are extremely close to it
                // relative error is less meaningful here
                return diff < (epsilon * Double.MinValue);
            }
            else
            { // use relative error
                return diff / (absA + absB) < epsilon;
            }
        }
    }
}