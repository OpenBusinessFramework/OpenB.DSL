namespace OpenB.DSL.Expressions.Math
{

    internal class SubstractionExpression : IExpression
    {
        private double leftHand;
        private double rightHand;

        public SubstractionExpression(double leftHand, double rightHand)
        {
            this.leftHand = leftHand;
            this.rightHand = rightHand;
        }

        public object Evaluate()
        {
            return leftHand - rightHand;
        }
    }
}