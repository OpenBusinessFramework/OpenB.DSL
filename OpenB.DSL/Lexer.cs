using System;
using System.IO;

namespace OpenB.DSL
{
    public sealed class Lexer : IDisposable
    {
        private readonly TextReader reader;
        private readonly TokenDefinition[] tokenDefinitions;

        private string lineRemaining;

        public Lexer(TextReader reader, TokenDefinition[] tokenDefinitions)
        {
            if (tokenDefinitions == null)
                throw new ArgumentNullException(nameof(tokenDefinitions));
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));

            this.reader = reader;
            this.tokenDefinitions = tokenDefinitions;
            nextLine();
        }

        private void nextLine()
        {
            do
            {
                lineRemaining = reader.ReadLine();
                ++LineNumber;
                Position = 0;
            } while (lineRemaining != null && lineRemaining.Length == 0);
        }

        public bool Next()
        {
            if (lineRemaining == null)
                return false;
            foreach (TokenDefinition tokenDefinition in tokenDefinitions)
            {
                var matched = tokenDefinition.Matcher.Match(lineRemaining);

                if (matched > 0)
                {
                    Position += matched;
                    Token = tokenDefinition.DefinitionType;
                    TokenContents = lineRemaining.Substring(0, matched);
                    lineRemaining = lineRemaining.Substring(matched);
                    if (lineRemaining.Length == 0)
                        nextLine();

                    return true;
                }
            }
            throw new Exception(string.Format("Unable to match against any tokens at line {0} position {1} \"{2}\"",
                                              LineNumber, Position, lineRemaining));
        }

        public string TokenContents { get; private set; }

        public string Token { get; private set; }

        public int LineNumber { get; private set; }

        public int Position { get; private set; }

        public void Dispose()
        {
            reader.Dispose();
        }
    }
}
