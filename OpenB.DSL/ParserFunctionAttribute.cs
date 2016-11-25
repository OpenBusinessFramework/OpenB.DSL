using System;

namespace OpenB.DSL
{

    internal class ParserFunctionAttribute : Attribute
    {
        public string Name;

        public ParserFunctionAttribute(string name)
        {
            Name = name;
        }
    }
}