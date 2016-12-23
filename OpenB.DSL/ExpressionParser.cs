using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using OpenB.DSL.Reflection;

namespace OpenB.DSL
{

    public class ExpressionParser : IParser
    {
        private static ExpressionParser expressionParser;

        private IDictionary<string, Queue> quequeCache = new Dictionary<string, Queue>();

        private CoreConfiguration coreConfiguration;
        private ExpressionFactory expressionFactory;
        readonly TokenEvaluator tokenEvaluator;

        private ExpressionParser(CoreConfiguration configuration, ExpressionFactory expressionFactory, TokenEvaluator tokenEvaluator)
        {
            if (tokenEvaluator == null)
                throw new ArgumentNullException(nameof(tokenEvaluator));

            if (expressionFactory == null)
                throw new ArgumentNullException(nameof(expressionFactory));
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            this.expressionFactory = expressionFactory;
            this.coreConfiguration = configuration;
            this.tokenEvaluator = tokenEvaluator;
        }

        public static ExpressionParser GetInstance(object context)
        {
            if (expressionParser == null)
            {
                expressionParser = new ExpressionParser(
                    new CoreConfiguration(),
                    new ExpressionFactory(new SymbolFactory(), new Reflection.TypeLoaderService(new Reflection.TypeLoaderServiceConfiguration())),
                    new TokenEvaluator(CultureInfo.InvariantCulture, new ModelEvaluator(context)));
            }
            return expressionParser;            
        }

        internal Queue ParseInternal(string expresion)
        {
            Tokenizer tokenizer = new Tokenizer(coreConfiguration.TokenDefinitions);
            var tokens = tokenizer.Tokenize(expresion);

            if (coreConfiguration.IgnoreWhiteSpace)
            {
                tokens = tokens.Where(t => !t.Type.Equals("SPACE")).ToList();
            }

            Stack expressionStack = new Stack();
            Queue outputQueue = new Queue();
            Stack operatorStack = new Stack();

            foreach (Token token in tokens)
            {
                // Try to get values from tokens.
                if (token.Type.Equals("INT") || token.Type.Equals("FLOAT") || token.Type.Equals("FIELD") || token.Type.Equals("QUOTED_STRING") || token.Type.Equals("BOOLEAN"))
                {
                    outputQueue.Enqueue(token);
                }

                if (token.Type.Equals("OPERATOR") || token.Type.Equals("EQUALITY_COMPARER") || token.Type.Equals("LOGICAL_OPERATOR"))
                {
                    // If the token is an operator, o1, then:
                    // while there is an operator token o2, at the top of the operator stack and either
                    // o1 is left - associative and its precedence is less than or equal to that of o2, or
                    // o1 is right associative, and has precedence less than that of o2,
                    // pop o2 off the operator stack, onto the output queue;
                    // at the end of iteration push o1 onto the operator stack.

                    if (operatorStack.Count > 0)
                    {
                        Token lastOperatorToken = (Token)operatorStack.Peek();

                        int currentOperatorIndex = coreConfiguration.OperatorPrecedance.Where(op => op.Operator.Equals(token.Contents)).Single().Precedance;
                        int lastOperatorIndex = coreConfiguration.OperatorPrecedance.Where(op => op.Operator.Equals(lastOperatorToken.Contents)).Single().Precedance;

                        if (currentOperatorIndex >= lastOperatorIndex)
                        {
                            outputQueue.Enqueue(operatorStack.Pop());
                        }
                    }
                    operatorStack.Push(token);
                }

                if (token.Type.Equals("SYMBOL"))
                {
                    operatorStack.Push(token);
                }

                if (token.Type.Equals("SUB_EXPR_START"))
                {
                    // If the token is a left parenthesis (i.e. "("), then push it onto the stack.
                    operatorStack.Push(token);
                }

                if (token.Type.Equals("SUB_EXPR_END"))
                {
                    // If the token is a right parenthesis (i.e. ")"):
                    // Until the token at the top of the stack is a left parenthesis, pop operators off the stack onto the output queue.
                    // Pop the left parenthesis from the stack, but not onto the output queue.
                    // If the token at the top of the stack is a function token, pop it onto the output queue.
                    // If the stack runs out without finding a left parenthesis, then there are mismatched parentheses

                    while (((Token)operatorStack.Peek()).Type != "SUB_EXPR_START")
                    {
                        outputQueue.Enqueue(operatorStack.Pop());
                    }
                    operatorStack.Pop();

                    if (operatorStack.Count > 0 && ((Token)operatorStack.Peek()).Type.Equals("SYMBOL"))
                    {
                        outputQueue.Enqueue(operatorStack.Pop());
                    }

                    // TODO: Expression excepition if no ( is found.
                }

                if (token.Type.Equals("SEPARATOR"))
                {
                    // Until the token at the top of the stack is a left parenthesis, pop operators off the stack onto the output queue.
                    // If no left parentheses are encountered, either the separator was misplaced or parentheses were mismatched.

                    while (((Token)operatorStack.Peek()).Type != "SUB_EXPR_START")
                    {
                        outputQueue.Enqueue(operatorStack.Pop());
                    }

                    // TODO: Expression excepition if no ( is found.
                }
            }

            while (operatorStack.Count > 0)
            {
                outputQueue.Enqueue(operatorStack.Pop());
            }

            if (outputQueue.Count > 0)
            {
                expressionStack.Push(outputQueue);
            }


            return outputQueue;
        }

        public ParserResult Parse(string expression)
        {
            Queue outputQueue;
            if (quequeCache.ContainsKey(expression))
            {
                outputQueue = quequeCache[expression];
            }
            else
            {
                outputQueue = ParseInternal(expression);
                Queue newQueue = (Queue)outputQueue.Clone();

                quequeCache.Add(expression, newQueue);
            }            
    
            if (outputQueue == null)
                throw new ArgumentNullException(nameof(outputQueue));

            Stack<object> argumentStack = new Stack<object>();
            Stack<IExpression> expressionStack = new Stack<IExpression>();

            while (outputQueue.Count > 0)
            {
                Token currentToken = (Token)outputQueue.Peek();
                switch (currentToken.Type)
                {
                    case "OPERATOR":
                        expressionStack.Push(HandleOperator(outputQueue, argumentStack, currentToken));
                        break;

                    case "EQUALITY_COMPARER":
                        expressionStack.Push(HandleEqualityComparer(outputQueue, argumentStack, currentToken));
                        break;

                    case "SYMBOL":
                        expressionStack.Push(HandleSymbol(outputQueue, argumentStack, currentToken));
                       
                        break;

                    case "LOGICAL_OPERATOR":
                        expressionStack.Push(HandleLogicalOperation(outputQueue, argumentStack, currentToken));
                        break;

                    default:
                        CreateArgument(outputQueue, argumentStack, currentToken);
                        break;
                }
            }
            return new ParserResult((bool)argumentStack.Pop());
        }

        private IExpression HandleLogicalOperation(Queue outputQueue, Stack<object> argumentStack, Token currentToken)
        {
            object rightHand = argumentStack.Pop();
            object leftHand = argumentStack.Pop();
            IEQualityExpression expression = expressionFactory.GetLogicalExpression(leftHand, rightHand, currentToken.Contents);
            argumentStack.Push(expression.Evaluate());

            outputQueue.Dequeue();

            return expression;
        }

        private IExpression HandleSymbol(Queue outputQueue, Stack<object> argumentStack, Token currentToken)
        {
            Type expressionType = expressionFactory.GetSymbolicExpression(currentToken.Contents);
            ConstructorInfo constructor = expressionType.GetConstructors().FirstOrDefault();

            List<object> arguments = new List<object>();

            for (int x = 0; x < constructor.GetParameters().Length; x++)
            {
                arguments.Add(argumentStack.Pop());
            }

            IEQualityExpression expression = (IEQualityExpression)Activator.CreateInstance(expressionType, arguments.ToArray());

            argumentStack.Push(expression.Evaluate());
            outputQueue.Dequeue();

            return (expression);
        }

        private IExpression HandleOperator(Queue outputQueue, Stack<object> argumentStack, Token currentToken)
        {
            object rightHand = argumentStack.Pop();
            object leftHand = argumentStack.Pop();
            IEQualityExpression expression = expressionFactory.GetExpression(leftHand, rightHand, currentToken.Contents);
            argumentStack.Push(expression.Evaluate());

            outputQueue.Dequeue();

            return (expression);
        }

        private IExpression HandleEqualityComparer(Queue outputQueue, Stack<object> argumentStack, Token currentToken)
        {
            object rightHand = argumentStack.Pop();
            object leftHand = argumentStack.Pop();
            IEQualityExpression expression = expressionFactory.GetExpression(leftHand, rightHand, currentToken.Contents);
            argumentStack.Push(expression.Evaluate());

            outputQueue.Dequeue();

            return (expression);
        }

        private IExpression CreateArgument(Queue outputQueue, Stack<object> argumentStack, Token currentToken)
        {
            object tokenValue = tokenEvaluator.Evaluate(currentToken);

            argumentStack.Push(tokenValue);

            outputQueue.Dequeue();

            return new ArgumentExpression(tokenValue);
        }
    }

    internal class ArgumentExpression : IExpression
    {
        private object tokenValue;

        public ArgumentExpression(object tokenValue)
        {
            this.tokenValue = tokenValue;
        }

        public object Evaluate()
        {
            throw new NotImplementedException();
        }
    }

    public interface IParser
    {
        ParserResult Parse(string expression);
    }
}