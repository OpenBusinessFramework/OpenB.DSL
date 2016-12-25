namespace OpenB.DSL.Expressions.Math
{

    internal class MultiplyExpression : IEQualityExpression
    {
        private IExpression leftHand;
        private IExpression rightHand;

        public MultiplyExpression(IExpression leftHand, IExpression rightHand)
        {
            this.leftHand = leftHand;
            this.rightHand = rightHand;
        }

        public object Evaluate()
        {
            return (double)leftHand.Evaluate() * (double)rightHand.Evaluate();
        }

        public override string ToString()
        {
            return $"{leftHand} * {rightHand}";
        }

        public string GenerateCode()
        {
            return $"({leftHand.GenerateCode()}) * ({rightHand.GenerateCode()})";
        }
    }
}