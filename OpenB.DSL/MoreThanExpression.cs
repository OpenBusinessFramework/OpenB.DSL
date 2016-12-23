using System;

namespace OpenB.DSL
{
    internal class MoreThanExpression : IEQualityExpression
    {
        private IExpression leftHand;
        private IExpression rightHand;

        public MoreThanExpression(IExpression leftHand, IExpression rightHand)
        {
            this.leftHand = leftHand;
            this.rightHand = rightHand;
        }

        public object Evaluate()
        {
            return (double)leftHand.Evaluate() > (double)rightHand.Evaluate();
        }
    }
}