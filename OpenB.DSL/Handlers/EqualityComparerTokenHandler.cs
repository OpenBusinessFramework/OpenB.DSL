using System;
using System.Collections;
using System.Collections.Generic;
using OpenB.DSL;
using OpenB.DSL.Expressions;

namespace OpenB.DSL.Handlers
{
    [HandlesTokens("EQUALITY_COMPARER")]
    public class EqualityComparerTokenHandler : ITokenHandler
    {
        readonly OperatorExpressionFactory expressionFactory;
        readonly Queue outputQueue;
        readonly Stack<IExpression> expressionStack;

        public EqualityComparerTokenHandler(OperatorExpressionFactory expressionFactory, Queue outputQueue, Stack<IExpression> expressionStack)
        {

            if (expressionFactory == null)
                throw new ArgumentNullException(nameof(expressionFactory));
            if (expressionStack == null)
                throw new ArgumentNullException(nameof(expressionStack));
            if (outputQueue == null)
                throw new ArgumentNullException(nameof(outputQueue));

            this.expressionStack = expressionStack;
            this.outputQueue = outputQueue;
            this.expressionFactory = expressionFactory;
        }       

        public void Handle(Token currentToken)
        {
            if (currentToken == null)
                throw new ArgumentNullException(nameof(currentToken));

            IExpression rightHand = expressionStack.Pop();
            IExpression leftHand = expressionStack.Pop();
            IEQualityExpression expression = expressionFactory.GetExpression(leftHand, rightHand, currentToken.Contents);
            expressionStack.Push(expression);

            ExpressionCache.GetInstance().Add(expression);

            outputQueue.Dequeue();
        }
    }

}