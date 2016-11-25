using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using OpenB.DSL;
using OpenB.DSL.Expressions.Math;

namespace OpenB.DSL
{

    public class ExpressionFactory
    {
        readonly SymbolFactory symbolFactory;

        public ExpressionFactory(SymbolFactory symbolFactory)
        {
            this.symbolFactory = symbolFactory;
            if (symbolFactory == null)
                throw new ArgumentNullException(nameof(symbolFactory));


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
    }
}

public interface IEQualityExpression : IExpression
{
}
