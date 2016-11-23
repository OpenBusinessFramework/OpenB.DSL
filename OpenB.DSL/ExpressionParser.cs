using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace OpenB.DSL
{
    public class ExpressionParser
    {
        private CoreConfiguration coreConfiguration;
        private ExpressionFactory expressionFactory;

        private ExpressionParser(CoreConfiguration configuration, ExpressionFactory expressionFactory)
        {
            this.expressionFactory = expressionFactory;
            if (expressionFactory == null)
                throw new ArgumentNullException(nameof(expressionFactory));
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            this.coreConfiguration = configuration;
        }

        public static ExpressionParser GetInstance()
        {
            return new ExpressionParser(new CoreConfiguration(), new ExpressionFactory(new SymbolFactory()));
        }

        public void Parse(string expresion)
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

                if (token.Type.Equals("OPERATOR"))
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

                        if (currentOperatorIndex < lastOperatorIndex)
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

                if (token.Type.Equals("EQUALITY_COMPARER"))
                {                    
                    expressionStack.Push(outputQueue);
                    outputQueue = new Queue();
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


            ProcessOutput(outputQueue);
        }

        private void ProcessOutput(Queue outputQueue)
        {
            if (outputQueue == null)
                throw new ArgumentNullException(nameof(outputQueue));

            IList<Token> tokenList = new List<Token>();

        }
    }
}