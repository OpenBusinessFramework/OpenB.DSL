using System;
using System.Collections;
using System.Collections.Generic;
using OpenB.DSL.Handlers;
using OpenB.DSL.Expressions;

namespace OpenB.DSL
{
    public class TokenHandlerFactory
    {
        private ConstantExpressionFactory constantExpressionFactory;      
        private OperatorExpressionFactory expressionFactory;       
     
        public TokenHandlerFactory(TokenHandlerData tokenHandlerData)
        {
            if (tokenHandlerData == null)
                throw new ArgumentNullException(nameof(tokenHandlerData));
           
            ExpressionStack = tokenHandlerData.ExpressionStack;
            constantExpressionFactory = tokenHandlerData.ConstantExpressionFactory;
            expressionFactory = tokenHandlerData.ExpressionFactory;
        }

        public Stack<IExpression> ExpressionStack { get; internal set; }

        public ITokenHandler GetHandler(Queue outputQueue, ExpressionEvaluationContext parserContext, Token token)
        {
            if (parserContext == null)
                throw new ArgumentNullException(nameof(parserContext));
            if (outputQueue == null)
                throw new ArgumentNullException(nameof(outputQueue));
            if (token == null)
                throw new ArgumentNullException(nameof(token));

            switch (token.Type)
            {
                case "OPERATOR":
                    return new OperatorTokenHandler(expressionFactory, outputQueue, ExpressionStack);

                case "EQUALITY_COMPARER":
                    return new EqualityComparerTokenHandler(expressionFactory, outputQueue, ExpressionStack);

                case "SYMBOL":
                    return new SymbolTokenHandler(expressionFactory, outputQueue, ExpressionStack);

                case "LOGICAL_OPERATOR":
                    return new LogicalOperatorTokenHandler(expressionFactory, outputQueue, ExpressionStack);

                case "FIELD":
                    return new FieldTokenHandler(parserContext, outputQueue, ExpressionStack);

                case "QUALIFIER":
                    return new QualifierTokenHandler(parserContext, outputQueue, ExpressionStack);

                case "CONSTANT":
                case "INT":
                case "QUOTED_STRING":
                case "BOOLEAN":
                    return new ConstantTokenHander(constantExpressionFactory, outputQueue, ExpressionStack);

                default:
                    throw new NotSupportedException($"Token of type {token.Type} is not supported.");

            }
        }
    }
}