namespace OpenB.DSL.Expressions
{
    public abstract class EqualityExpressionBase : IEQualityExpression
    {
        public IExpression Left { get; }
        public IExpression Right { get; }

        protected EqualityExpressionBase(IExpression leftHand, IExpression rightHand)
        {
            this.Left = leftHand;
            this.Right = rightHand;
        }

        public abstract object Evaluate();
        public abstract string GenerateCode();       
    }
}