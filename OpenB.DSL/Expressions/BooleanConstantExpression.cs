namespace OpenB.DSL.Expressions
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

        public override string ToString()
        {
            return string.Concat("{Boolean: ", value ? "true" : "false", "}");
        }

        public string GenerateCode()
        {
            return value.ToString().ToLower();
        }
    }
}