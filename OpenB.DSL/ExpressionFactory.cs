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

        internal IExpression GetExpression(object left, object right, string contents)
        {
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

            }
            throw new NotSupportedException();
        }

        internal IParserFunction GetSymbolicExpression(string name, List<object> arguments)
        {

            var functions = typeLoaderService.GetTypesImplementing(new[] { typeof(IParserFunction) });

            foreach (Type type in functions)
            {
                ParserFunctionAttribute attribute = type.GetCustomAttributes(typeof(ParserFunctionAttribute), true).Single() as ParserFunctionAttribute;
                if (attribute.Name == name)
                {
                    IParserFunction symbol = (IParserFunction)Activator.CreateInstance(type, arguments.ToArray());
                    return symbol;
                }
            }

            throw new NotSupportedException($"Function {name} is not available");

        }
    }
}


public interface IEQualityExpression : IExpression
{
}
