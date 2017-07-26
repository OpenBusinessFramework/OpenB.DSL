using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using OpenB.DSL;
using OpenB.DSL.Handlers;
using OpenB.DSL.Expressions;

namespace OpenB.DSL.Handlers
{

    [HandlesTokens("SYMBOL")]
    public class SymbolTokenHandler : ITokenHandler
{
    readonly Queue outputQueue;
    readonly Stack<IExpression> expressionStack;
    readonly OperatorExpressionFactory expressionFactory;

    public SymbolTokenHandler(OperatorExpressionFactory expressionFactory, Queue outputQueue, Stack<IExpression> expressionStack)
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

        Type expressionType = expressionFactory.GetSymbolicExpression(currentToken.Contents);
        ConstructorInfo constructor = expressionType.GetConstructors().FirstOrDefault();

        List<object> arguments = new List<object>();

        for (int x = 0; x < constructor.GetParameters().Length; x++)
        {
            arguments.Add(expressionStack.Pop().Evaluate());
        }

        IEQualityExpression expression = (IEQualityExpression)Activator.CreateInstance(expressionType, arguments.ToArray());

        expressionStack.Push(expression);
        outputQueue.Dequeue();
    }
}
}