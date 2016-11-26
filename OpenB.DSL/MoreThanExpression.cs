using System;

namespace OpenB.DSL
{
    internal class MoreThanExpression : IEQualityExpression
    {
        private double leftHand;
        private double rightHand;

        public MoreThanExpression(double leftHand, double rightHand)
        {
            this.leftHand = leftHand;
            this.rightHand = rightHand;
        }

        public object Evaluate()
        {
            return leftHand > rightHand;
        }
    }
}