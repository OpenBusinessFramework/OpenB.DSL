using System;

namespace OpenB.DSL
{
    internal class LessThanExpression : IEQualityExpression
    {
        private double leftHand;
        private double rightHand;

        public LessThanExpression(double leftHand, double rightHand)
        {
            this.leftHand = leftHand;
            this.rightHand = rightHand;
        }

        public object Evaluate()
        {
            return leftHand < rightHand;
        }
    }
}