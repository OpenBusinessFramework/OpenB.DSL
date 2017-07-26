using System;

namespace OpenB.DSL.Expressions.Math
{

    internal class AdditionExpression : IEQualityExpression
    {
        public IExpression Left { get; }
        public IExpression Right { get; }

        public AdditionExpression(IExpression leftHand, IExpression rightHand)
        {
            this.Left = leftHand;
            this.Right = rightHand;
        }

        public object Evaluate()
        {
            return (double)Left.Evaluate() + (double)Right.Evaluate();
        }

        public string GenerateCode()
        {
            return $"({Left.GenerateCode()}) + ({Right.GenerateCode()})";
        }

        public override string ToString()
        {
            return $"{Left} + {Right}";
        }
    }
}