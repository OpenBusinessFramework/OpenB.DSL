using OpenB.DSL.Expressions.Math;
using System;

namespace OpenB.DSL.Expressions
{
    internal class MoreThanExpression : EqualityExpressionBase
    {        public MoreThanExpression(IExpression leftHand, IExpression rightHand) : base(leftHand, rightHand)
        {
           
        }

        public override object Evaluate()
        {
            return (double)Left.Evaluate() > (double)Right.Evaluate();
        }

        public override string ToString()
        {
            return string.Concat(Left.ToString(), " > ", Right.ToString());
        }

        public override string GenerateCode()
        {
            return $"({Left.GenerateCode()}) > ({Right.GenerateCode()})";
        }
    }
}