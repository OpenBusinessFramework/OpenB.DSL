using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace OpenB.DSL.Handlers
{

    internal class FieldTokenHandler : ITokenHandler
    {
        private Stack<IExpression> expressionStack;
        private ParserContext context;
        private Queue outputQueue;

        public FieldTokenHandler(ParserContext context, Queue outputQueue, Stack<IExpression> expressionStack)
        {
            if (expressionStack == null)
                throw new ArgumentNullException(nameof(expressionStack));
            if (outputQueue == null)
                throw new ArgumentNullException(nameof(outputQueue));
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            this.context = context;
            this.outputQueue = outputQueue;
            this.expressionStack = expressionStack;
        }

        public void Handle(Token currentToken)
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
    }
}