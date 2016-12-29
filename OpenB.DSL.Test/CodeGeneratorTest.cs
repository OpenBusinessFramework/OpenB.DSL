using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NUnit.Framework;

namespace OpenB.DSL.Test
{
    [TestFixture]
    public class CodeGeneratorTest
    {
        [Test]
        public void EvaluateCustomFunction_ComplexLogicalOperatorInExpression()
        {
            var person = new Person { Age = 23 };
            var expressionContext = new ParserContext(typeof(Person));
            var parser = ExpressionParser.GetInstance();

            string expression = "(12 + 3 = 15) and ((12 + 3 = 15) or (13 + 2 = 10))";

            CodeGenerator generator = new CodeGenerator(parser);
            string assigment = generator.GenerateExpressionAssignment(expressionContext, expression);
        }

        [Test]
        public void EvaluateCustomFunction_ComplexObjectEvalutionExpression()
        {
            var person = new Person { Age = 23 };
            var expressionContext = new ParserContext(typeof(Person));
            var parser = ExpressionParser.GetInstance();

            string expression = "[Key] = 'harry' or [Name] = 'john'";

            CodeGenerator generator = new CodeGenerator(parser);
            string assigment = generator.GenerateExpressionAssignment(expressionContext, expression);
        }

        [Test]
        public void CreateBusinessRule()
        {           
            Family family = new Family();

            ExpressionParser parser = ExpressionParser.GetInstance();
            ParserResult result = parser.Parse(new ParserContext(typeof(Family)), "all [Person] in [Family.Members] having [Age] > 18 and [Gender] = 'Male'");

            result.CompiledExpression.Evaluate();
        }

        public class ExpressionParsingService
        {
            Regex baseExpression;

            public ExpressionParsingService()
            {
                baseExpression = new Regex(@"(?<quantifier>ALL|ONE)\s+?\[(?<modeltype>\w+?)\]\s+?IN\s+\[(?<model>\w+?\.{0,1}\w+>?)\]\s+?HAVING");
            }

            public void Parse(string expression)
            {

            }
        }

        internal class Family
        {
        }

        public class ProcessDefintion
        {
            public string Key { get; private set; }
            public string Name { get; private set; }
            public string Description { get; private set; }
            public ProcessStateDefinition StartingState { get; internal set; }

            public ProcessDefintion(string key, string name, string description, ProcessStateDefinition startingState)
            {
                if (description == null)
                    throw new ArgumentNullException(nameof(description));
                if (name == null)
                    throw new ArgumentNullException(nameof(name));
                if (key == null)
                    throw new ArgumentNullException(nameof(key));
                if (startingState == null)
                    throw new ArgumentNullException(nameof(startingState));

                StartingState = startingState;

                Description = description;
                Name = name;
                Key = key;
            }
        }

        internal class ProcessService
        {
            IList<Process> runningInstances
            {
                get { return processDataService.GetRunningProcesses(); }

            }
            IProcessDataService processDataService;

            internal ProcessService(ProcessDataService processDataService)
            {
                if (processDataService == null)
                    throw new ArgumentNullException(nameof(processDataService));
                this.processDataService = processDataService;
            }

            internal static ProcessService GetInstance()
            {
                return new ProcessService(new ProcessDataService());
            }

            internal Process StartProcess(string processDefinitionKey)
            {
                ProcessDefintion definition = processDataService.GetDefinition(processDefinitionKey);

                Process process = new Process(definition);
                process.CurrentState = new ProcessState(definition.StartingState);
                runningInstances.Add(process);

                return process;

            }
        }

        internal interface IProcessDataService
        {
            ProcessDefintion GetDefinition(string processDefinitionKey);
            IList<Process> GetRunningProcesses();
        }

        internal class ProcessDataService : IProcessDataService
        {
            public ProcessDataService()
            {
            }

            public ProcessDefintion GetDefinition(string processDefinitionKey)
            {
                throw new NotImplementedException();
            }

            public IList<Process> GetRunningProcesses()
            {
                throw new NotImplementedException();
            }
        }

        internal class Process
        {
            private ExecutionState ExecutionState;

            public ProcessDefintion ProcessDefinition { get; private set; }
            public ProcessState CurrentState { get; internal set; }
            public ProcessDefintion StartingDefinition { get; set; }
            public IList<ProcessStateChange> StateChanges { get; private set; }

            public Process(ProcessDefintion startingDefinition)
            {
                if (startingDefinition == null)
                    throw new ArgumentNullException(nameof(startingDefinition));
                StartingDefinition = startingDefinition;
            }

            public void Start()
            {
                CurrentState = new ProcessState(ProcessDefinition.StartingState);
                StateChanges = new List<ProcessStateChange>();
            }

            public void Execute()
            {
                ProcessState lastState = CurrentState;

                foreach (Transition transition in CurrentState.ProcessStateDefinition.Transitions)
                {
                    var transitionResult = transition.Execute();
                    if (transitionResult.Success)
                    {
                        var eventExecutionResult = CurrentState.ProcessStateDefinition.ExecuteEvents();
                        if (eventExecutionResult.Success)
                        {
                            CurrentState = new ProcessState(transition.ResultingStateDefinition);
                            ExecutionState = ExecutionState.StateChanged;
                        }
                    }
                }

                if (lastState != CurrentState)
                {
                    this.ExecutionState = ExecutionState.NoTransitionsPossible;
                }
            }
        }

        public class ProcessStateChange
        {
            public DateTime Moment { get; private set; }
            public ProcessState PreviousState { get; private set; }
            public ProcessState NewState { get; private set; }

            public ProcessStateChange(ProcessState previousState, ProcessState newState)
            {
                NewState = newState;
                PreviousState = previousState;
                if (newState == null)
                    throw new ArgumentNullException(nameof(newState));
                if (previousState == null)
                    throw new ArgumentNullException(nameof(previousState));

                Moment = DateTime.Now;
            }
        }

        internal enum ExecutionState
        {
            Initialized,
            NoTransitionsPossible,
            StateChanged
        }

        public class ProcessState
        {
            public ProcessStateDefinition ProcessStateDefinition { get; private set; }

            public ProcessState(ProcessStateDefinition processStateDefinition)
            {
                if (processStateDefinition == null)
                    throw new ArgumentNullException(nameof(processStateDefinition));

                this.ProcessStateDefinition = processStateDefinition;
            }
        }

        public class BusinessRule : IRule
        {
            private Type type;
            private Family family;
            private string v;

            public EvaluationContext Context { get; private set; }

            public BusinessRule(EvaluationContext context)
            {
                if (context == null)
                    throw new ArgumentNullException(nameof(context));

                Context = context;
            }

            public BusinessRule(Type selectionType, object context, string rule)
            {
                this.type = type;
                this.family = family;
                this.v = v;
            }

            public RuleResult Evaluate()
            {
                throw new NotImplementedException();
            }


        }

        public class EvaluationContext
        {
        }

        public class ProcessStateDefinition
        {
            readonly IEnumerable<IEvent> events;
            readonly string name;
            readonly string description;
            readonly string key;
            public IList<Transition> Transitions { get; private set; }

            public ProcessStateDefinition(string key, string name, string description, IList<Transition> transitions, IEnumerable<IEvent> events)
            {
                if (transitions == null)
                    throw new ArgumentNullException(nameof(transitions));
                if (key == null)
                    throw new ArgumentNullException(nameof(key));
                if (description == null)
                    throw new ArgumentNullException(nameof(description));
                if (name == null)
                    throw new ArgumentNullException(nameof(name));
                if (events == null)
                    throw new ArgumentNullException(nameof(events));

                this.events = events;
                this.description = description;
                this.name = name;
                this.key = key;
                this.Transitions = transitions;
            }

            public EventCollectionResult ExecuteEvents()
            {
                EventCollectionResult eventCollectionResult = new EventCollectionResult();

                foreach (IEvent @event in events)
                {
                    eventCollectionResult.EventResults.Add(@event.Execute());
                }

                return eventCollectionResult;
            }
        }



        public class Transition
        {
            readonly IEnumerable<IRule> rules;
            public ProcessStateDefinition ResultingStateDefinition { get; private set; }

            public Transition(ProcessStateDefinition resultingStateDefinition, IEnumerable<IRule> rules)
            {

                if (resultingStateDefinition == null)
                    throw new ArgumentNullException(nameof(resultingStateDefinition));
                if (rules == null)
                    throw new ArgumentNullException(nameof(rules));

                this.rules = rules;
                this.ResultingStateDefinition = resultingStateDefinition;
            }

            public TransitionResult Execute()
            {
                TransitionResult result = new TransitionResult();
                result.RuleCollectionResult = ExecuteRules();

                return result;
            }



            private RuleCollectionResult ExecuteRules()
            {
                RuleCollectionResult ruleCollectionResult = new RuleCollectionResult();

                foreach (IRule rule in rules)
                {
                    RuleResult ruleResult = rule.Evaluate();
                    ruleCollectionResult.RuleResults.Add(ruleResult);
                }

                return ruleCollectionResult;
            }
        }

        public class EventCollectionResult
        {
            public IList<EventResult> EventResults { get; private set; }
            public EventCollectionResult()
            {
                EventResults = new List<EventResult>();
            }

            public bool Success
            {
                get
                {
                    return EventResults.All(e => e.Success);
                }
            }
        }

        public class TransitionResult
        {
            public RuleCollectionResult RuleCollectionResult { get; internal set; }
            public bool Success { get { return this.RuleCollectionResult.Success; } }
        }

        public class RuleCollectionResult
        {
            internal IList<RuleResult> RuleResults { get; private set; }

            public RuleCollectionResult()
            {
                RuleResults = new List<RuleResult>();
            }

            public bool Success
            {
                get
                {
                    return RuleResults.All(t => t.Sucess);
                }
            }
        }

        public class RuleResult
        {
            public bool Sucess { get; }
        }

        public interface IEvent
        {
            EventResult Execute();
        }

        public class EventResult
        {
            public bool Success { get; internal set; }
        }

        public interface IRule
        {
            RuleResult Evaluate();
        }
    }
}
