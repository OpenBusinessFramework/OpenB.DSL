using NUnit.Framework;

namespace OpenB.DSL.Test
{
    [TestFixture]
    public class ExpressionParserTest
    {
        [Test]
        public void DoSomething()
        {
            ExpressionParser parser = ExpressionParser.GetInstance();

            string expression = "sin ( max ( 2, 3 ) / 3 * 3.1415 ) = (20 + sjakie(1,2,3) * 4)";

            parser.Parse(expression);
        }
    }
}
