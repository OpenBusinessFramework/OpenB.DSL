using System;

namespace OpenB.DSL.Expressions.Math
{

    internal class AdditionExpression : IEQualityExpression
    {
        private IExpression leftHand;
        private IExpression rightHand;

        public AdditionExpression(IExpression leftHand, IExpression rightHand)
        {
            this.leftHand = leftHand;
            this.rightHand = rightHand;
        }

        public object Evaluate()
        {
            return (double)leftHand.Evaluate() + (double)rightHand.Evaluate();
        }

        public string GenerateCode()
        {
            return $"({leftHand.GenerateCode()}) + ({rightHand.GenerateCode()})";
        }

        public override string ToString()
        {
            return $"{leftHand} + {rightHand}";
        }
    }
}