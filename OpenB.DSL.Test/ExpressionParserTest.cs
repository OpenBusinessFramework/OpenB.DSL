using System.Collections;
using NUnit.Framework;

namespace OpenB.DSL.Test
{
    [TestFixture]
    public class ExpressionParserTest
    {
        [Test]
        public void MathExpression_NotEquals_MathExpression()
        {
            ExpressionParser parser = ExpressionParser.GetInstance();

            string expression = "4 * 3 / 2 = (12 / 3) + 2";

            Queue queue =  parser.Parse(expression);
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
    }
}
