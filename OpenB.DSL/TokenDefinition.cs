using System;

namespace OpenB.DSL
{

    public sealed class TokenDefinition
    {
        public IMatcher Matcher { get; private set; }
        public string DefinitionType { get; private set; }

        public TokenDefinition(string regex, string definitionType)
        {
            if (regex == null)
                throw new ArgumentNullException(nameof(regex));

            Matcher = new RegexMatcher(regex);
            DefinitionType = definitionType;
        }
    }
}