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
            return $"'{value}'";
        }

        public string GenerateCode()
        {
            return $"\"{value}\"";
        }
    }
}