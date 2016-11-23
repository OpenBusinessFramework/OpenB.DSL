using System.Collections.Generic;
using System.IO;

namespace OpenB.DSL
{

    internal class Tokenizer
    {
        readonly TokenDefinition[] definitions;

        public Tokenizer(TokenDefinition[] definitions)
        {
            this.definitions = definitions;
        }

        public IList<Token> Tokenize(string expression)
        {
            TextReader textReader = new StringReader(expression);
            Lexer l = new Lexer(textReader, definitions);

            List<Token> tokens = new List<Token>();

            while (l.Next())
            {
                tokens.Add(new Token(l.LineNumber, l.Position, l.Token, l.TokenContents));
            }

            return tokens;
        }
    }
}