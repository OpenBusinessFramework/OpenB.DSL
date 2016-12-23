namespace OpenB.DSL
{

    internal class StringConstantExpression : IConstantExpression
    {       
        readonly string value;

        public StringConstantExpression(string value)
        {
            this.value = value;
        }

        public object Evaluate()
        {
            return (string)value;
        }
    }
}