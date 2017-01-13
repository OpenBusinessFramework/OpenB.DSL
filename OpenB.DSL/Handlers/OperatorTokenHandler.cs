using System;
using System.Collections;
using System.Collections.Generic;

namespace OpenB.DSL.Handlers
{
    [HandlesTokens("OPERATOR")]
    public class OperatorTokenHandler : ITokenHandler
    {
        private Stack<IExpression> expressionStack;
        private Queue outputQueue;
        readonly OperatorExpressionFactory expressionFactory;

        public OperatorTokenHandler(OperatorExpressionFactory expressionFactory, Queue outputQueue, Stack<IExpression> expressionStack)
        {
            this.expressionFactory = expressionFactory;
            if (expressionStack == null)
                throw new ArgumentNullException(nameof(expressionStack));
            if (outputQueue == null)
                throw new ArgumentNullException(nameof(outputQueue));
            if (expressionFactory == null)
                throw new ArgumentNullException(nameof(expressionFactory));

            this.outputQueue = outputQueue;
            this.expressionStack = expressionStack;
        }

        public void Handle(Token currentToken)
        {
            IExpression rightHand = expressionStack.Pop();
            IExpression leftHand = expressionStack.Pop();
            IEQualityExpression expression = expressionFactory.GetExpression(leftHand, rightHand, currentToken.Contents);
            expressionStack.Push(expression);

            outputQueue.Dequeue();
        }
    }
}