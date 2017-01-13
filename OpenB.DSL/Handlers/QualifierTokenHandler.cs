using System;
using System.Collections;
using System.Collections.Generic;

namespace OpenB.DSL.Handlers
{
    [HandlesTokens("QUALIFIER")]
    public class QualifierTokenHandler : ITokenHandler
    {
        readonly ExpressionEvaluationContext context;
        readonly Queue outputQueue;
        readonly Stack<IExpression> expressionStack;

        public QualifierTokenHandler(ExpressionEvaluationContext context, Queue outputQueue, Stack<IExpression> expressionStack)
        {
            if (expressionStack == null)
                throw new ArgumentNullException(nameof(expressionStack));
            if (outputQueue == null)
                throw new ArgumentNullException(nameof(outputQueue));
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            this.expressionStack = expressionStack;
            this.outputQueue = outputQueue;
            this.context = context;
        }

        public void Handle(Token currentToken)
        {
            if (currentToken == null)
                throw new ArgumentNullException(nameof(currentToken));

            var whereExpression = expressionStack.Pop();
            var contextList = expressionStack.Pop();
            var itemType = expressionStack.Pop();

            outputQueue.Dequeue();
            outputQueue.Dequeue();

            var quantifierToken = (Token)outputQueue.Dequeue();

            expressionStack.Push(new ContextSelectionExpression(context, quantifierToken.Contents, whereExpression, contextList, itemType));
        }
    }
}