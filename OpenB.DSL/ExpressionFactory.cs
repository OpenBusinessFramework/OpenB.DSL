using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

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

        public IExpression CreateExpression(IList<Token> tokens)
        {
            var lastToken = tokens.Last();

            if (lastToken.Type.Equals("OPERATOR"))
            {
                if (lastToken.Contents.Equals("+"))
                {
                    double leftHand = double.Parse(tokens[0].Contents, CultureInfo.InvariantCulture);
                    double rightHand = double.Parse(tokens[1].Contents, CultureInfo.InvariantCulture);

                    return new AdditionExpression(leftHand, rightHand);
                }

                if (lastToken.Contents.Equals("/"))
                {
                    double leftHand = double.Parse(tokens[0].Contents, CultureInfo.InvariantCulture);
                    double rightHand = double.Parse(tokens[1].Contents, CultureInfo.InvariantCulture);

                    return new DivisionExpression(leftHand, rightHand);
                }
            }

            if (lastToken.Type.Equals("SYMBOL"))
            {
                return symbolFactory.GetExpression(lastToken.Contents, tokens.Except(new List<Token> { tokens.Last() }));
            }

            throw new NotSupportedException("Cannot create expression");
        }
    }
    
}