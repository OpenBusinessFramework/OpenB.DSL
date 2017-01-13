using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using OpenB.DSL.Reflection;
using OpenB.DSL.Test.Assemblies;

namespace OpenB.DSL.Test
{
    [TestFixture]
    public class CodeGeneratorTest
    {
        [Test]
        public void EvaluateCustomFunction_ComplexLogicalOperatorInExpression()
        {
            var person = new Person { Age = 23 };
            var personAssembly = typeof(Person).Assembly;
            var personRepository = new Repository<Person>();
            var modelEvaluator = new ModelEvaluator(new[] { personAssembly });

            var expressionContext = new ExpressionEvaluationContext(modelEvaluator);
            var parser = ExpressionParser.GetInstance();

            string expression = "(12 + 3 = 15) and ((12 + 3 = 15) or (13 + 2 = 10))";

            CodeGenerator generator = new CodeGenerator(parser);
            string assigment = generator.GenerateExpressionAssignment(expressionContext, expression);
        }

        [Test]
        public void EvaluateCustomFunction_ComplexObjectEvalutionExpression()
        {
            var person = new Person { Age = 23 };
            var personAssembly = typeof(Person).Assembly;
            var personRepository = new Repository<Person>();
            var modelEvaluator = new ModelEvaluator(new[] { personAssembly });


            var expressionContext = new ExpressionEvaluationContext(modelEvaluator);
            var parser = ExpressionParser.GetInstance();

            string expression = "[Key] = 'harry' or [Name] = 'john'";

            CodeGenerator generator = new CodeGenerator(parser);
            string assigment = generator.GenerateExpressionAssignment(expressionContext, expression);
        }

        //[Test]
        //public void CreateBusinessRule()
        //{
        //    var person = new Person { Age = 23 };
        //    var personAssembly = typeof(Person).Assembly;
        //    var personRepository = new Repository<Person>();
        //    var modelEvaluator = new ModelEvaluator(new[] { personAssembly });


        //    Family family = new Family();
        //    Repository<Family> familyRepository = new Repository<Family>();

        //    ExpressionParser parser = ExpressionParser.GetInstance();


        //    var parentDirectory = new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName;
        //    var assemblyFolderName = Path.Combine(parentDirectory, "Assemblies");

        //    ProcessEngineConfiguration configuration = new ProcessEngineConfiguration { AssemblyFolder = assemblyFolderName };
        //    EvaluationContext evaluationContext = new EvaluationContext();
        //    BusinessRule businessRule = new BusinessRule(configuration, parser, evaluationContext, "all [Person] in [Family.Members] having [NumberOfChildren] > 0");

        //    ProcessStateDefinition entryDataChecked = new ProcessStateDefinition
        //    {
        //        Key = "ENTRY_DATA_CHECKED",
        //        Name = "ENTRY_DATA_CHECKED",
        //        Description = "Entry data checked",
        //        Transitions = new List<StateTransistionDefinition>(),
        //        Events = new List<EventDefinition>()
        //    };
        //    StateTransistionDefinition transistion = new StateTransistionDefinition
        //    {
        //        ResultingStateDefinition = entryDataChecked,
        //        BusinessRules = new List<BusinessRuleDefinition>()
        //    };

        //    ProcessStateDefinition startingState = new ProcessStateDefinition
        //    {
        //        Key = "ENTRY_DATA_CHECKED",
        //        Name = "ENTRY_DATA_CHECKED",
        //        Description = "Entry data checked",
        //        Transitions = new List<StateTransistionDefinition>(),
        //        Events = new List<EventDefinition>()
        //    };

        //    ProcessDefinition myFirstProcess = new ProcessDefinition
        //    {
        //        Key = "FAMILY_ADDED",
        //        Name = "New family added",
        //        Description = "A new family has been added.",
        //        StartingStateReference = new ProcessStateDefinitionReference { Ref = "ENTRY_DATA_CHECKED" }
        //    };
        //    ProcessService processService = ProcessService.GetInstance();


        //    processService.StartProcess("SAMPLE_PROCESS");

        //    ParserResult result = parser.Parse(new ExpressionEvaluationContext(modelEvaluator), "all [Person] in [Family.Members] having [Age] > 18 and [Gender] = 'Male'");




        //    result.CompiledExpression.Evaluate();
        //}
    }
}
