using System;
using System.Collections;
using NUnit.Framework;

namespace OpenB.DSL.Test
{
    [TestFixture]
    public class ExpressionParserTest
    {
        Person person;
        ParserContext expressionContext;
        ExpressionParser parser;

        [OneTimeSetUp]
        public void SetupFixture()
        {
            person = new Person { Age = 23 };
            expressionContext =  new ParserContext(typeof(Person));
            parser = ExpressionParser.GetInstance();
        }

        [Test]
        public void MathExpression_MoreThan_MathExpression()
        {
           

            string expression = "8 * 3 / 2 > (12 / 3) + 2";

            //Queue queue = parser.Parse(expression);
            var parserResult = parser.Parse(expressionContext, expression);

            bool result = (bool)parserResult.CompiledExpression.Evaluate();

            Assert.That(result.Equals(true));
        }

        [Test]
        public void MathExpression_LessThan_MathExpression()
        {
           
            string expression = "8 * 3 / 2 < (12 / 3) + 2";

            //Queue queue = parser.Parse(expression);
            bool result = (bool)parser.Parse(expressionContext, expression).CompiledExpression.Evaluate();

            Assert.That(result.Equals(false));
        }

        [Test]
        public void MathExpression_NotEqual_MathExpression()
        {
            
            string expression = "8 * 3 / 2 != (12 / 3) + 2";

            //Queue queue = parser.Parse(expression);
            bool result = (bool)parser.Parse(expressionContext, expression).CompiledExpression.Evaluate();

            Assert.That(result.Equals(true));
        }

        [Test]
        public void MathExpression_Equals_MathExpression()
        {
            

            string expression = "4 * 3 / 2 = (12 / 3) + 2";

            //Queue queue = parser.Parse(expression);
            bool result = (bool)parser.Parse(expressionContext, expression).CompiledExpression.Evaluate();

            Assert.That(result.Equals(true));
        }

        [Test]
        public void MathExpression_Equals_ConstantExpression()
        {     

            string expression = "4 * 3 = 12";
           
            bool result = (bool)parser.Parse(expressionContext, expression).CompiledExpression.Evaluate();

            Assert.That(result.Equals(true));
        }

        [Test]
        public void MultiMathExpression_Equals_ConstantExpression()
        {
           

            string expression = "4 * 3 - 2 = 12 * (6 - 3) - 26";

            //Queue queue = parser.Parse(expression);
            bool result = (bool)parser.Parse(expressionContext, expression).CompiledExpression.Evaluate();

            Assert.That(result.Equals(true));
        }

        [Test]
        public void ConstantExpression_Equals_ConstantExpression()
        {           
            string expression = "12 = 12";
           
            bool result = (bool)parser.Parse(expressionContext, expression).CompiledExpression.Evaluate();

            Assert.That(result.Equals(true));
        }

        [Test]
        public void EvaluateCustomFunction_InExpression()
        {       

            string expression = "SQRT(144) = 12";

            var parserResult = parser.Parse(expressionContext, expression);
            bool result = (bool)parserResult.CompiledExpression.Evaluate();

            Assert.That(result.Equals(true));
        }

        [Test]
        public void EvaluateComplexCustomFunction_InExpression()
        {
           

            string expression = "(12 + 3) / SQRT(15 * 15) = 1";

            
            bool result = (bool)parser.Parse(expressionContext, expression).CompiledExpression.Evaluate();

            Assert.That(result.Equals(true));
        }

        [Test]
        public void EvaluateCustomFunction_LogicalOperatorInExpression_And()
        { 

            string expression = "(12 + 3 = 15) and (4 + 12 = 16)";
            
            bool result = (bool)parser.Parse(expressionContext, expression).CompiledExpression.Evaluate();

            Assert.That(result.Equals(true));
        }

        [Test]
        public void EvaluateCustomFunction_LogicalOperatorInExpression_Or()
        {         
            string expression = "(12 + 3 = 15) or (4 + 12 = 10)";
           
            bool result = (bool)parser.Parse(expressionContext, expression).CompiledExpression.Evaluate();

            Assert.That(result.Equals(true));
        }

        [Test]
        public void EvaluateCustomFunction_ComplexLogicalOperatorInExpression()
        {
          
            string expression = "(12 + 3 = 15) and ((12 + 3 = 15) or (13 + 2 = 10))";
       
            bool result = (bool)parser.Parse(expressionContext, expression).CompiledExpression.Evaluate();

            Assert.That(result.Equals(true));
        }

        [Test]
        public void EvaluateCustomFunction_FieldInExpression()
        {       

            string expression = "100 / SQRT([Age]) = 2";          
            bool result = (bool)parser.Parse(expressionContext, expression).CompiledExpression.Evaluate();

            Assert.That(result.Equals(true));
        }

        [Test]
        public void EvaluateCustomFunction_FieldInExpressionRunsTwice_IsFaster()
        {        

            string expression = "100 / SQRT([Age]) = 2";

            //Queue queue = parser.Parse(expression);
            bool firstResult = (bool)parser.Parse(expressionContext, expression).CompiledExpression.Evaluate();
            bool secondResult =  (bool)parser.Parse(expressionContext, expression).CompiledExpression.Evaluate();

            Assert.That(firstResult == secondResult);           
        }     


        [Test]
        public void Just_Two_Expressions_BoundByALogicalExpression()
        {            

            string expression = "[Age] > 16 and [IsMarried] = true";            
           

            bool result = (bool)parser.Parse(expressionContext, expression).CompiledExpression.Evaluate();

            Assert.That(result.Equals(false));
        }

        [Test]
        public void Evaluate_StringComparisation_ReturnsTrue()
        {
         

            string expression = "'John Doe' = 'John Doe'";

        
            bool result = (bool)parser.Parse(expressionContext, expression).CompiledExpression.Evaluate();

            Assert.That(result.Equals(true));
        }

        [Test]
        public void Evaluate_StringComparisation_ReturnsFalse()
        {
        

            string expression = "'Jane Doe' != 'John Doe'";

      
            bool result = (bool)parser.Parse(expressionContext, expression).CompiledExpression.Evaluate();

            Assert.That(result.Equals(true));
        }

        [Test]
        public void Evaluate_StringComparisation_ThrowsException()
        {
          
            string expression = "'Jane Doe' > 'John Doe'";            

            Assert.Throws<NotSupportedException>(() => parser.Parse(expressionContext, expression));
        }
    }

    public class CodeGenerator
    {
        readonly IParser parser;

        public CodeGenerator(IParser parser)
        {
           
            if (parser == null)
                throw new ArgumentNullException(nameof(parser));

            this.parser = parser;
        }

        public string GenerateExpressionAssignment(ParserContext context, string expression)
        {
            return $"businessRules.Add({parser.Parse(context, expression).CompiledExpression.GenerateCode()});";
        } 
    }
}
