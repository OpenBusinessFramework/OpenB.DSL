using System;
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
            Person person = new Person { Age = 23 };

            ExpressionParser parser = ExpressionParser.GetInstance(person);

            string expression = "8 * 3 / 2 > (12 / 3) + 2";

            //Queue queue = parser.Parse(expression);
            bool result = parser.Parse(expression).Outcome;

            Assert.That(result.Equals(true));
        }

        [Test]
        public void MathExpression_LessThan_MathExpression()
        {
            Person person = new Person { Age = 23 };
            ExpressionParser parser = ExpressionParser.GetInstance(person);

            string expression = "8 * 3 / 2 < (12 / 3) + 2";

            //Queue queue = parser.Parse(expression);
            bool result = parser.Parse(expression).Outcome;

            Assert.That(result.Equals(false));
        }

        [Test]
        public void MathExpression_NotEqual_MathExpression()
        {
            Person person = new Person { Age = 23 };
            ExpressionParser parser = ExpressionParser.GetInstance(person);

            string expression = "8 * 3 / 2 != (12 / 3) + 2";

            //Queue queue = parser.Parse(expression);
            bool result = parser.Parse(expression).Outcome;

            Assert.That(result.Equals(true));
        }

        [Test]
        public void MathExpression_Equals_MathExpression()
        {
            Person person = new Person { Age = 23 };
            ExpressionParser parser = ExpressionParser.GetInstance(person);

            string expression = "4 * 3 / 2 = (12 / 3) + 2";

            //Queue queue = parser.Parse(expression);
            bool result = parser.Parse(expression).Outcome;

            Assert.That(result.Equals(true));
        }

        [Test]
        public void MathExpression_Equals_ConstantExpression()
        {
            Person person = new Person { Age = 23 };
            ExpressionParser parser = ExpressionParser.GetInstance(person);

            string expression = "4 * 3 = 12";

            //Queue queue = parser.Parse(expression);
            bool result = parser.Parse(expression).Outcome;

            Assert.That(result.Equals(true));
        }

        [Test]
        public void MultiMathExpression_Equals_ConstantExpression()
        {
            Person person = new Person { Age = 23 };
            ExpressionParser parser = ExpressionParser.GetInstance(person);

            string expression = "4 * 3 - 2 = 12 * (6 - 3) - 26";

            //Queue queue = parser.Parse(expression);
            bool result = parser.Parse(expression).Outcome;

            Assert.That(result.Equals(true));
        }

        [Test]
        public void ConstantExpression_Equals_ConstantExpression()
        {
            Person person = new Person { Age = 23 };
            ExpressionParser parser = ExpressionParser.GetInstance(person);

            string expression = "12 = 12";

            //Queue queue = parser.Parse(expression);
            bool result = parser.Parse(expression).Outcome;

            Assert.That(result.Equals(true));
        }

        [Test]
        public void EvaluateCustomFunction_InExpression()
        {
            Person person = new Person { Age = 23 };
            ExpressionParser parser = ExpressionParser.GetInstance(person);

            string expression = "SQRT(144) = 12";

            //Queue queue = parser.Parse(expression);
            bool result = parser.Parse(expression).Outcome;

            Assert.That(result.Equals(true));
        }

        [Test]
        public void EvaluateComplexCustomFunction_InExpression()
        {
            Person person = new Person { Age = 23 };
            ExpressionParser parser = ExpressionParser.GetInstance(person);

            string expression = "(12 + 3) / SQRT(15 * 15) = 1";

            //Queue queue = parser.Parse(expression);
            bool result = parser.Parse(expression).Outcome;

            Assert.That(result.Equals(true));
        }

        [Test]
        public void EvaluateCustomFunction_LogicalOperatorInExpression_And()
        {
            Person person = new Person { Age = 23 };
            ExpressionParser parser = ExpressionParser.GetInstance(person);

            string expression = "(12 + 3 = 15) and (4 + 12 = 16)";

            //Queue queue = parser.Parse(expression);
            bool result = parser.Parse(expression).Outcome;

            Assert.That(result.Equals(true));
        }

        [Test]
        public void EvaluateCustomFunction_LogicalOperatorInExpression_Or()
        {
            Person person = new Person { Age = 23 };
            ExpressionParser parser = ExpressionParser.GetInstance(person);

            string expression = "(12 + 3 = 15) or (4 + 12 = 10)";

            //Queue queue = parser.Parse(expression);
            bool result = parser.Parse(expression).Outcome;

            Assert.That(result.Equals(true));
        }

        [Test]
        public void EvaluateCustomFunction_ComplexLogicalOperatorInExpression()
        {
            Person person = new Person { Age = 23 };
            ExpressionParser parser = ExpressionParser.GetInstance(person);

            string expression = "(12 + 3 = 15) and ((12 + 3 = 15) or (13 + 2 = 10))";

            //Queue queue = parser.Parse(expression);
            bool result = parser.Parse(expression).Outcome;

            Assert.That(result.Equals(true));
        }

        [Test]
        public void EvaluateCustomFunction_FieldInExpression()
        {
            Person person = new Person { Age = 2500 };
            ExpressionParser parser = ExpressionParser.GetInstance(person);

            string expression = "100 / SQRT([Age]) = 2";

            //Queue queue = parser.Parse(expression);
            bool result = parser.Parse(expression).Outcome;

            Assert.That(result.Equals(true));
        }

        [Test]
        public void EvaluateCustomFunction_FieldInExpressionRunsTwice_IsFaster()
        {
            Person person = new Person { Age = 2500 };
            ExpressionParser parser = ExpressionParser.GetInstance(person);

            string expression = "100 / SQRT([Age]) = 2";

            //Queue queue = parser.Parse(expression);
            bool firstResult = parser.Parse(expression).Outcome;
            bool secondResult = parser.Parse(expression).Outcome;

            Assert.That(firstResult == secondResult);           
        }     


        [Test]
        public void Just_Two_Expressions_BoundByALogicalExpression()
        {
            Person person = new Person { Age = 23 };
            ExpressionParser parser = ExpressionParser.GetInstance(person);

            string expression = "[Age] > 16 and [IsMarried] = true";

            //Queue queue = parser.Parse(expression);
            bool result = parser.Parse(expression).Outcome;

            Assert.That(result.Equals(false));
        }

        [Test]
        public void Evaluate_StringComparisation_ReturnsTrue()
        {
            Person person = new Person { Age = 23 };
            ExpressionParser parser = ExpressionParser.GetInstance(person);

            string expression = "'John Doe' = 'John Doe'";

            //Queue queue = parser.Parse(expression);
            bool result = parser.Parse(expression).Outcome;

            Assert.That(result.Equals(true));
        }

        [Test]
        public void Evaluate_StringComparisation_ReturnsFalse()
        {
            Person person = new Person { Age = 23 };
            ExpressionParser parser = ExpressionParser.GetInstance(person);

            string expression = "'Jane Doe' != 'John Doe'";

            //Queue queue = parser.Parse(expression);
            bool result = parser.Parse(expression).Outcome;

            Assert.That(result.Equals(true));
        }

        [Test]
        public void Evaluate_StringComparisation_ThrowsException()
        {
            Person person = new Person { Age = 23 };
            ExpressionParser parser = ExpressionParser.GetInstance(person);

            string expression = "'Jane Doe' > 'John Doe'";            

            Assert.Throws<NotSupportedException>(() => parser.Parse(expression));
        }
    }
}
