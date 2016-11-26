using System.Collections;
using NUnit.Framework;

namespace OpenB.DSL.Test
{
    [TestFixture]
    public class ExpressionParserTest
    {
        [Test]
        public void MathExpression_MoreThan_MathExpression()
        {
            ExpressionParser parser = ExpressionParser.GetInstance();

            string expression = "8 * 3 / 2 > (12 / 3) + 2";

            Queue queue =  parser.Parse(expression);
            bool result = parser.Evaluate(queue);

            Assert.That(result.Equals(true));
        }

        [Test]
        public void MathExpression_LessThan_MathExpression()
        {
            ExpressionParser parser = ExpressionParser.GetInstance();

            string expression = "8 * 3 / 2 < (12 / 3) + 2";

            Queue queue = parser.Parse(expression);
            bool result = parser.Evaluate(queue);

            Assert.That(result.Equals(false));
        }

        [Test]
        public void MathExpression_NotEqual_MathExpression()
        {
            ExpressionParser parser = ExpressionParser.GetInstance();

            string expression = "8 * 3 / 2 != (12 / 3) + 2";

            Queue queue = parser.Parse(expression);
            bool result = parser.Evaluate(queue);

            Assert.That(result.Equals(true));
        }

        [Test]
        public void MathExpression_Equals_MathExpression()
        {
            ExpressionParser parser = ExpressionParser.GetInstance();

            string expression = "4 * 3 / 2 = (12 / 3) + 2";

            Queue queue = parser.Parse(expression);
            bool result = parser.Evaluate(queue);

            Assert.That(result.Equals(true));
        }

        [Test]
        public void MathExpression_Equals_ConstantExpression()
        {
            ExpressionParser parser = ExpressionParser.GetInstance();

            string expression = "4 * 3 = 12";

            Queue queue = parser.Parse(expression);
            bool result = parser.Evaluate(queue);

            Assert.That(result.Equals(true));
        }

        [Test]
        public void MultiMathExpression_Equals_ConstantExpression()
        {
            ExpressionParser parser = ExpressionParser.GetInstance();

            string expression = "4 * 3 - 2 = 12 * (6 - 3) - 26";

            Queue queue = parser.Parse(expression);
            bool result = parser.Evaluate(queue);

            Assert.That(result.Equals(true));
        }

        [Test]
        public void ConstantExpression_Equals_ConstantExpression()
        {
            ExpressionParser parser = ExpressionParser.GetInstance();

            string expression = "12 = 12";

            Queue queue = parser.Parse(expression);
            bool result = parser.Evaluate(queue);

            Assert.That(result.Equals(true));
        }

        [Test]
        public void EvaluateCustomFunction_InExpression()
        {
            ExpressionParser parser = ExpressionParser.GetInstance();

            string expression = "SQRT(144) = 12";

            Queue queue = parser.Parse(expression);
            bool result = parser.Evaluate(queue);

            Assert.That(result.Equals(true));
        }

        [Test]
        public void EvaluateComplexCustomFunction_InExpression()
        {
            ExpressionParser parser = ExpressionParser.GetInstance();

            string expression = "(12 + 3) / SQRT(15 * 15) = 1";

            Queue queue = parser.Parse(expression);
            bool result = parser.Evaluate(queue);

            Assert.That(result.Equals(true));
        }

        [Test]
        public void EvaluateCustomFunction_LogicalOperatorInExpression_And()
        {
            ExpressionParser parser = ExpressionParser.GetInstance();

            string expression = "(12 + 3 = 15) and (4 + 12 = 16)";

            Queue queue = parser.Parse(expression);
            bool result = parser.Evaluate(queue);

            Assert.That(result.Equals(true));
        }

        [Test]
        public void EvaluateCustomFunction_LogicalOperatorInExpression_Or()
        {
            ExpressionParser parser = ExpressionParser.GetInstance();

            string expression = "(12 + 3 = 15) or (4 + 12 = 10)";

            Queue queue = parser.Parse(expression);
            bool result = parser.Evaluate(queue);

            Assert.That(result.Equals(true));
        }

        [Test]
        public void EvaluateCustomFunction_ComplexLogicalOperatorInExpression()
        {
            ExpressionParser parser = ExpressionParser.GetInstance();

            string expression = "(12 + 3 = 15) and ((12 + 3 = 15) or (13 + 2 = 10))";

            Queue queue = parser.Parse(expression);
            bool result = parser.Evaluate(queue);

            Assert.That(result.Equals(true));
        }

        [Test]
        public void EvaluateCustomFunction_FieldInExpression()
        {
            ExpressionParser parser = ExpressionParser.GetInstance();

            string expression = "100 / SQRT([Age]) = 2";

            Queue queue = parser.Parse(expression);
            bool result = parser.Evaluate(queue);

            Assert.That(result.Equals(true));
        }

        [Test]
        public void Evaluate_StringComparisation_ReturnsTrue()
        {
            ExpressionParser parser = ExpressionParser.GetInstance();

            string expression = "'John Doe' = 'John Doe'";

            Queue queue = parser.Parse(expression);
            bool result = parser.Evaluate(queue);

            Assert.That(result.Equals(true));
        }

        [Test]
        public void Evaluate_StringComparisation_ReturnsFalse()
        {
            ExpressionParser parser = ExpressionParser.GetInstance();

            string expression = "'Jane Doe' != 'John Doe'";

            Queue queue = parser.Parse(expression);
            bool result = parser.Evaluate(queue);

            Assert.That(result.Equals(true));
        }
    }
}
