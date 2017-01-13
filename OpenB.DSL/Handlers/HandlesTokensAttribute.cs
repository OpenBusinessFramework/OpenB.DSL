using System;

namespace OpenB.DSL.Handlers
{

    internal class HandlesTokensAttribute : Attribute
    {
        public string[] TokenTypes { get; private set; }

        public HandlesTokensAttribute(params string[] tokenTypes)
        {
            TokenTypes = tokenTypes;
        }
    }
}