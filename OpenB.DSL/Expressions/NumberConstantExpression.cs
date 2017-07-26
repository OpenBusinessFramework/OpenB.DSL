using System.Globalization;

namespace OpenB.DSL.Expressions
{

    internal class NumberConstantExpression : IConstantExpression
    {
        readonly double value;

        public NumberConstantExpression(int value)
        {
            this.value = value;
        }

        public NumberConstantExpression(float value)
        {
            this.value = value;
        }

        public NumberConstantExpression(double value)
        {
            this.value = value;
        }

        public object Evaluate()
        {
            return value;
        }

        public override string ToString()
        {
            return value.ToString();
        }

        public string GenerateCode()
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }
    }
}