using System;

namespace OpenB.DSL.Expressions.Math
{
    internal class DivisionExpression : EqualityExpressionBase
    {
       

        public DivisionExpression(IExpression leftHand, IExpression rightHand) : base(leftHand, rightHand)
        {
            
        }

        public override object Evaluate()
        {
            return (double)Left.Evaluate() / (double)Right.Evaluate();
        }

        public override string GenerateCode()
        {
            return $"({Left.GenerateCode()} / ({Right.GenerateCode()})";
        }

        public override string ToString()
        {
            return $"{Left} / {Right}";
        }
    }
}