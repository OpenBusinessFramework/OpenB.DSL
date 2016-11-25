namespace OpenB.DSL.Expressions.Math
{

    internal class AdditionExpression : IExpression
    {
        private double leftHand;
        private double rightHand;

        public AdditionExpression(double leftHand, double rightHand)
        {
            this.leftHand = leftHand;
            this.rightHand = rightHand;
        }

        public object Evaluate()
        {
            return leftHand + rightHand;
        }
    }
}