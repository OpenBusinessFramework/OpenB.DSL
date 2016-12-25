namespace OpenB.DSL
{

    internal class StringComparisionIsEqualExpression : IEQualityExpression
    {
        private IExpression left;
        private IExpression right;

        public StringComparisionIsEqualExpression(IExpression left, IExpression right)
        {
            this.left = left;
            this.right = right;

        }

        public object Evaluate()
        {
            return left.Evaluate().Equals(right.Evaluate());
        }

        public override string ToString()
        {
            return $"{left} = {right}";
        }

        public string GenerateCode()
        {
            return $"({left.GenerateCode()}).Equals({right.GenerateCode()})";
        }

    }
}