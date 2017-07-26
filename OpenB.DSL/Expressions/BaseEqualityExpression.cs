using System;
using System.Collections.Generic;

namespace OpenB.DSL.Expressions
{
    public abstract class BaseEqualityExpression
    {
        protected static bool nearlyEqual(double a, double b, double epsilon)
        {
            double absA = System.Math.Abs(a);
            double absB = System.Math.Abs(b);
            double diff = System.Math.Abs(a - b);

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

    internal class ExpressionCache
    {
        private static ExpressionCache instance;

        private IList<IExpression> expressions;

        private ExpressionCache()
        {
            expressions = new List<IExpression>();
        }

        public void Add(IExpression expression)
        {
            expressions.Add(expression);
        }

        internal static ExpressionCache GetInstance()
        {
           if (instance == null)
            {
                instance = new ExpressionCache();
            }
            return instance;
        }
    }
}