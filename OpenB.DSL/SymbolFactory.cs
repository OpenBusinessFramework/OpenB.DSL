using System;
using System.Collections.Generic;

namespace OpenB.DSL
{
    public class SymbolFactory
    {
        internal IExpression GetExpression(string keyword, IEnumerable<Token> parameters)
        {
            return new SymbolExpression();
        }
    }
}