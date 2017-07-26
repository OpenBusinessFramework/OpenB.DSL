﻿namespace OpenB.DSL.Expressions.Math
{

    internal class MultiplyExpression : EqualityExpressionBase
    {

        public MultiplyExpression(IExpression leftHand, IExpression rightHand) : base(leftHand, rightHand)
        {
           
        }

        public override  object Evaluate()
        {
            return (double)Left.Evaluate() * (double)Right.Evaluate();
        }

        public override string ToString()
        {
            return $"{Left} * {Right}";
        }

        public override string GenerateCode()
        {
            return $"({Left.GenerateCode()}) * ({Right.GenerateCode()})";
        }
    }
}