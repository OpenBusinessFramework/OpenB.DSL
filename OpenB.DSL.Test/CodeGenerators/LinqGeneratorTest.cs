using NUnit.Framework;
using OpenB.DSL.Expressions;
using OpenB.DSL.Reflection;
using OpenB.DSL.Test.Assemblies;

namespace OpenB.DSL.Test.CodeGenerators
{
    [TestFixture]
    public class LinqGeneratorTest
    {
        Person person;
        ExpressionEvaluationContext expressionContext;
        ExpressionParser parser;
        ModelEvaluator modelEvaluator;

        [OneTimeSetUp]
        public void SetupFixture()
        {
            person = new Person { Age = 23 };
            var personRepository = new Repository<Person>();

            var personAssembly = typeof(Person).Assembly;
            modelEvaluator = new ModelEvaluator(new[] { personAssembly });


            expressionContext = new ExpressionEvaluationContext(modelEvaluator);
            parser = ExpressionParser.GetInstance();
        }

        [Test]
        public void DoSomething()
        {
            string expression = "[Age] > 16 and [IsMarried] = true";

            var compiledExpression = parser.Parse(expressionContext, expression).CompiledExpression; ;
                   

            LinqCodeGenerator codeGenerator = new LinqCodeGenerator();


            codeGenerator.Generate(compiledExpression);
        }
    }

    internal class LinqCodeGenerator
    {
        internal void Generate(IExpression compiledExpression)
        {
           if (compiledExpression is IEQualityExpression)
            {

            }
        }
    }
}
