using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace OpenB.DSL
{
    public class ExpressionParser
    {
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

        public static ExpressionParser GetInstance()
        {
            return new ExpressionParser(
                    new CoreConfiguration(),
                    new ExpressionFactory(new SymbolFactory(), new Reflection.TypeLoaderService(new Reflection.TypeLoaderServiceConfiguration())),
                    new TokenEvaluator(CultureInfo.InvariantCulture));
        }

        public Queue Parse(string expresion)
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
                if (token.Type.Equals("INT") || token.Type.Equals("FLOAT"))
                {
                    outputQueue.Enqueue(token);
                }

                if (token.Type.Equals("OPERATOR") || token.Type.Equals("EQUALITY_COMPARER"))
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

                        int currentOperatorIndex = coreConfiguration.OperatorPrecedance.IndexOf(token.Contents[0]);
                        int lastOperatorIndex = coreConfiguration.OperatorPrecedance.IndexOf(lastOperatorToken.Contents[0]);

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


                    if (((Token)operatorStack.Peek()).Type.Equals("SYMBOL"))
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

        public bool Evaluate(Queue outputQueue)
        {
            if (outputQueue == null)
                throw new ArgumentNullException(nameof(outputQueue));

            Stack<object> argumentStack = new Stack<object>();

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

                    default:
                        CreateArgument(outputQueue, argumentStack, currentToken);                        
                        break;
                }
            }

            return (bool)argumentStack.Pop();

            throw new NotSupportedException();
        }

        private void HandleSymbol(Queue outputQueue, Stack<object> argumentStack, Token currentToken)
        {
            List<object> arguments = new List<object>();
            
            while (argumentStack.Count > 0)
            {
                arguments.Add(argumentStack.Pop());
            }

           var expression = expressionFactory.GetSymbolicExpression(currentToken.Contents, arguments);
           
            argumentStack.Push(expression.Evaluate());
            outputQueue.Dequeue();
        }

        private void HandleOperator(Queue outputQueue, Stack<object> argumentStack, Token currentToken)
        {
            object rightHand = argumentStack.Pop();
            object leftHand = argumentStack.Pop();
            IExpression expression = expressionFactory.GetExpression(leftHand, rightHand, currentToken.Contents);
            argumentStack.Push(expression.Evaluate());

            outputQueue.Dequeue();
        }

        private void HandleEqualityComparer(Queue outputQueue, Stack<object> argumentStack, Token currentToken)
        {
            object rightHand = argumentStack.Pop();
            object leftHand = argumentStack.Pop();
            IExpression expression = expressionFactory.GetExpression(leftHand, rightHand, currentToken.Contents);
            argumentStack.Push(expression.Evaluate());

            outputQueue.Dequeue();
        }

        private void CreateArgument(Queue outputQueue, Stack<object> argumentStack, Token currentToken)
        {
            object tokenValue = tokenEvaluator.Evaluate(currentToken);

            argumentStack.Push(tokenValue);

            outputQueue.Dequeue();
        }
    }
}