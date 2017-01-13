using System;
using System.Collections;
using System.Collections.Generic;

namespace OpenB.DSL.Handlers
{
    [HandlesTokens("CONSTANT", "INT", "QUOTED_STRING", "BOOLEAN")]
    public class ConstantTokenHander : ITokenHandler
    {
        readonly Queue outputQueue;
        readonly Stack<IExpression> expressionStack;
        readonly Token currentToken;
        readonly ConstantExpressionFactory constantExpressionFactory;

        public ConstantTokenHander(ConstantExpressionFactory constantExpressionFactory, Queue outputQueue, Stack<IExpression> expressionStack)
        {

            if (constantExpressionFactory == null)
                throw new ArgumentNullException(nameof(constantExpressionFactory));
            if (expressionStack == null)
                throw new ArgumentNullException(nameof(expressionStack));
            if (outputQueue == null)
                throw new ArgumentNullException(nameof(outputQueue));

            this.expressionStack = expressionStack;
            this.outputQueue = outputQueue;
            this.constantExpressionFactory = constantExpressionFactory;
        }

        public void Handle(Token currentToken)
        {
            if (currentToken == null)
                throw new ArgumentNullException(nameof(currentToken));

            IExpression tokenValue = constantExpressionFactory.Evaluate(currentToken);

            expressionStack.Push(tokenValue);

            outputQueue.Dequeue();
        }
    }
}