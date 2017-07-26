namespace OpenB.DSL.Expressions
{

    internal class StringComparisionIsEqualExpression : EqualityExpressionBase
    {
        public StringComparisionIsEqualExpression(IExpression left, IExpression right) : base(left, right)
        {
        }

        public override object Evaluate()
        {
            return Left.Evaluate().Equals(Right.Evaluate());
        }

        public override string ToString()
        {
            return $"{Left} = {Right}";
        }

        public override string GenerateCode()
        {
            return $"({Left.GenerateCode()}).Equals({Right.GenerateCode()})";
        }

    }
}