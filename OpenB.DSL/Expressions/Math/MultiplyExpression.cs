namespace OpenB.DSL.Expressions.Math
{

    internal class MultiplyExpression : IExpression
    {
        private double leftHand;
        private double rightHand;

        public MultiplyExpression(double leftHand, double rightHand)
        {
            this.leftHand = leftHand;
            this.rightHand = rightHand;
        }

        public object Evaluate()
        {
            return leftHand * rightHand;
        }
    }
}