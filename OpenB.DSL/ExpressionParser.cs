using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace OpenB.DSL
{

    public class ExpressionParser : IParser
    {
        private static ExpressionParser expressionParser;

        private IDictionary<string, Queue> quequeCache = new Dictionary<string, Queue>();

        private CoreConfiguration coreConfiguration;
        private ExpressionFactory expressionFactory;
        readonly ConstantExpressionFactory constantExpressionFactory;

        private ExpressionParser(CoreConfiguration configuration, ExpressionFactory expressionFactory, ConstantExpressionFactory constantExpressionFactory)
        {        

            if (constantExpressionFactory == null)
                throw new ArgumentNullException(nameof(constantExpressionFactory));

            if (expressionFactory == null)
                throw new ArgumentNullException(nameof(expressionFactory));
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            this.expressionFactory = expressionFactory;
            this.coreConfiguration = configuration;
            this.constantExpressionFactory = constantExpressionFactory;
        }

        public static ExpressionParser GetInstance()
        {
            if (expressionParser == null)
            {
                expressionParser = new ExpressionParser(
                    new CoreConfiguration(),
                    new ExpressionFactory(new SymbolFactory(), new Reflection.TypeLoaderService(new Reflection.TypeLoaderServiceConfiguration())),
                    new ConstantExpressionFactory(CultureInfo.InvariantCulture));
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

        public ParserResult Parse(ParserContext context, string expression)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

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

            Stack<IExpression> argumentStack = new Stack<IExpression>();


            while (outputQueue.Count > 0)
            {
                Token currentToken = (Token)outputQueue.Peek();
                switch (currentToken.Type)
                {
                    case "OPERATOR":
                        HandleOperator(outputQueue, argumentStack, currentToken);
                        break;

                    case "EQUALITY_COMPARER":
                        HandleEqualityComparer(outputQueue, argumentStack, currentToken);
                        break;

                    case "SYMBOL":
                        HandleSymbol(outputQueue, argumentStack, currentToken);
                        break;

                    case "LOGICAL_OPERATOR":
                        HandleLogicalOperation(outputQueue, argumentStack, currentToken);
                        break;

                    case "FIELD":
                        HandleField(context, outputQueue, argumentStack, currentToken);
                        break;

                    default:
                        HandleConstant(outputQueue, argumentStack, currentToken);
                        break;
                }
            }
            return new ParserResult(argumentStack.Pop());
        }

        private void HandleField(ParserContext context, Queue outputQueue, Stack<IExpression> expressionStack, Token currentToken)
        {           

            Regex fieldExpression = new Regex(@"\[(?<contents>.*)\]");
            Match match = fieldExpression.Match(currentToken.Contents);
            if (!match.Success || match.Groups.Count == 0)
            {
                throw new NotSupportedException("Cannot parse field");               
            }           

            expressionStack.Push(new FieldExpression(context, match.Groups["contents"].Value));
            outputQueue.Dequeue();
        }

        private void HandleLogicalOperation(Queue outputQueue, Stack<IExpression> argumentStack, Token currentToken)
        {
            IExpression rightHand = argumentStack.Pop();
            IExpression leftHand = argumentStack.Pop();
            IEQualityExpression expression = expressionFactory.GetLogicalExpression(leftHand, rightHand, currentToken.Contents);
            argumentStack.Push(expression);

            outputQueue.Dequeue();


        }

        private void HandleSymbol(Queue outputQueue, Stack<IExpression> argumentStack, Token currentToken)
        {
            Type expressionType = expressionFactory.GetSymbolicExpression(currentToken.Contents);
            ConstructorInfo constructor = expressionType.GetConstructors().FirstOrDefault();

            List<object> arguments = new List<object>();

            for (int x = 0; x < constructor.GetParameters().Length; x++)
            {
                arguments.Add(argumentStack.Pop().Evaluate());
            }

            IEQualityExpression expression = (IEQualityExpression)Activator.CreateInstance(expressionType, arguments.ToArray());

            argumentStack.Push(expression);
            outputQueue.Dequeue();
        }

        private void HandleOperator(Queue outputQueue, Stack<IExpression> argumentStack, Token currentToken)
        {
            IExpression rightHand = argumentStack.Pop();
            IExpression leftHand = argumentStack.Pop();
            IEQualityExpression expression = expressionFactory.GetExpression(leftHand, rightHand, currentToken.Contents);
            argumentStack.Push(expression);

            outputQueue.Dequeue();
        }

        private void HandleEqualityComparer(Queue outputQueue, Stack<IExpression> expressionStack, Token currentToken)
        {
            IExpression rightHand = expressionStack.Pop();
            IExpression leftHand = expressionStack.Pop();
            IEQualityExpression expression = expressionFactory.GetExpression(leftHand, rightHand, currentToken.Contents);
            expressionStack.Push(expression);

            ExpressionCache.GetInstance().Add(expression);

            outputQueue.Dequeue();
        }

        private void HandleConstant(Queue outputQueue, Stack<IExpression> argumentStack, Token currentToken)
        {
            IExpression tokenValue = constantExpressionFactory.Evaluate(currentToken);

            argumentStack.Push(tokenValue);

            outputQueue.Dequeue();


        }
    }

    public interface IParser
    {
        ParserResult Parse(ParserContext parserContext, string expression);
    }
}