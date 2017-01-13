using System;
using System.Collections;
using System.Collections.Generic;
using OpenB.DSL;

namespace OpenB.DSL.Handlers
{
    [HandlesTokens("LOGICAL_OPERATOR")]
    public class LogicalOperatorTokenHandler : ITokenHandler
    {
        readonly Queue outputQueue;      
        readonly OperatorExpressionFactory expressionFactory;
        private Stack<IExpression> expressionStack;

        public LogicalOperatorTokenHandler(OperatorExpressionFactory expressionFactory, Queue outputQueue, Stack<IExpression> expressionStack)
        {
            if (expressionFactory == null)
                throw new ArgumentNullException(nameof(expressionFactory));
            if (expressionStack == null)
                throw new ArgumentNullException(nameof(expressionStack));
            if (outputQueue == null)
                throw new ArgumentNullException(nameof(outputQueue));

            this.expressionFactory = expressionFactory;
            this.expressionStack = expressionStack;
            this.outputQueue = outputQueue;
        }      

        public void Handle(Token currentToken)
        {
            if (currentToken == null)
                throw new ArgumentNullException(nameof(currentToken));

            IExpression rightHand = expressionStack.Pop();
            IExpression leftHand = expressionStack.Pop();
            IEQualityExpression expression = expressionFactory.GetLogicalExpression(leftHand, rightHand, currentToken.Contents);
            expressionStack.Push(expression);

            outputQueue.Dequeue();
        }
    }
}