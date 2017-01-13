using System;
using System.Linq;
using OpenB.DSL.Expressions.Math;
using OpenB.DSL.Functions;
using OpenB.DSL.Reflection;

namespace OpenB.DSL
{

    public class OperatorExpressionFactory
    {
        readonly SymbolFactory symbolFactory;
        readonly TypeLoaderService typeLoaderService;

        public OperatorExpressionFactory(SymbolFactory symbolFactory, TypeLoaderService typeLoaderService)
        {
            if (typeLoaderService == null)
                throw new ArgumentNullException(nameof(typeLoaderService));
            if (symbolFactory == null)
                throw new ArgumentNullException(nameof(symbolFactory));

            this.typeLoaderService = typeLoaderService;
            this.symbolFactory = symbolFactory;
        }

        internal IEQualityExpression GetExpression(IExpression left, IExpression right, string contents)
        {
            if (left is StringConstantExpression || right is StringConstantExpression)
            {
                if (contents == "=")
                {
                    return new StringComparisionIsEqualExpression(left, right);
                }

                if (contents == "!=")
                {
                    return new StringComparisionNotEqualExpression(left, right);
                }

                throw new NotSupportedException($"Equality operator {contents} is not supported for arguments {left} and {right}.");
                  
            }

            // TODO: Type checking for left and right.
           

            switch (contents)
            {
                case "+":
                    return new AdditionExpression(left, right);
                case "-":
                    return new SubstractionExpression(left, right);
                case "*":
                    return new MultiplyExpression(left, right);
                case "/":
                    return new DivisionExpression(left, right);

                case "=":
                    return new EqualExpression(left, right);
                case "<":
                    return new LessThanExpression(left, right);
                case ">":
                    return new MoreThanExpression(left, right);
                case "!=":
                    return new NotEqualExpression(left, right);

            }
            throw new NotSupportedException();
        }

        internal Type GetSymbolicExpression(string name)
        {
            var functions = typeLoaderService.GetTypesImplementing(new[] { typeof(IParserFunction) });

            foreach (Type type in functions)
            {
                ParserFunctionAttribute attribute = type.GetCustomAttributes(typeof(ParserFunctionAttribute), true).Single() as ParserFunctionAttribute;
                if (attribute.Name == name)
                {
                    return type;
                }
            }

            throw new NotSupportedException($"Function {name} is not available");

        }

        internal IEQualityExpression GetLogicalExpression(IExpression leftHand, IExpression rightHand, string contents)
        {          

            switch (contents)
            {
                case "and":
                    return new LogicalAndExpression(leftHand, rightHand);
                case "or":
                    return new LogicalOrExpression(leftHand, rightHand);
            }

            throw new NotSupportedException($"Logical operator {contents} is not supported.");
        }
    }
}


public interface IEQualityExpression : OpenB.DSL.IEQualityExpression
{
}
