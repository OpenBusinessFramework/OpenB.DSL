namespace OpenB.DSL
{

    internal class BooleanConstantExpression : IConstantExpression
    {
        readonly bool value;

        public BooleanConstantExpression(bool value)
        {
            this.value = value;
        }

        public object Evaluate()
        {
            return value;
        }
    }
}