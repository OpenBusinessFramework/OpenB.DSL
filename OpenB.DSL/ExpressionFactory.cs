using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using OpenB.DSL;
using OpenB.DSL.Expressions.Math;
using OpenB.DSL.Functions;
using OpenB.DSL.Reflection;

namespace OpenB.DSL
{

    public class ExpressionFactory
    {
        readonly SymbolFactory symbolFactory;
        readonly TypeLoaderService typeLoaderService;

        public ExpressionFactory(SymbolFactory symbolFactory, TypeLoaderService typeLoaderService)
        {
            if (typeLoaderService == null)
                throw new ArgumentNullException(nameof(typeLoaderService));
            if (symbolFactory == null)
                throw new ArgumentNullException(nameof(symbolFactory));

            this.typeLoaderService = typeLoaderService;
            this.symbolFactory = symbolFactory;
        }

        internal IEQualityExpression GetExpression(object left, object right, string contents)
        {
            if (left is string || right is string)
            {
                if (contents == "=")
                {
                    return new StringComparisionIsEqualExpression(left.ToString(), right.ToString());
                }

                if (contents == "!=")
                {
                    return new StringComparisionNotEqualExpression(left.ToString(), right.ToString());
                }

                throw new NotSupportedException($"Equality operator {contents} is not supported for arguments {left} and {right}.");
                  
            }

            // TODO: Type checking for left and right.
            double leftHand = Convert.ToDouble(left);
            double rightHand = Convert.ToDouble(right);

            switch (contents)
            {
                case "+":
                    return new AdditionExpression(leftHand, rightHand);
                case "-":
                    return new SubstractionExpression(leftHand, rightHand);
                case "*":
                    return new MultiplyExpression(leftHand, rightHand);
                case "/":
                    return new DivisionExpression(leftHand, rightHand);

                case "=":
                    return new EqualExpression(leftHand, rightHand);
                case "<":
                    return new LessThanExpression(leftHand, rightHand);
                case ">":
                    return new MoreThanExpression(leftHand, rightHand);
                case "!=":
                    return new NotEqualExpression(leftHand, rightHand);

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

        internal IEQualityExpression GetLogicalExpression(object leftHand, object rightHand, string contents)
        {
            bool leftHandBoolean = (bool)leftHand;
            bool rightHandBoolean = (bool)rightHand;

            switch (contents)
            {
                case "and":
                    return new LogicalAndExpression(leftHandBoolean, rightHandBoolean);
                case "or":
                    return new LogicalOrExpression(leftHandBoolean, rightHandBoolean);
            }

            throw new NotSupportedException($"Logical operator {contents} is not supported.");
        }
    }
}


public interface IEQualityExpression : OpenB.DSL.IEQualityExpression
{
}
