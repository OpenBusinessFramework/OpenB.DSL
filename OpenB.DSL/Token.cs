using System;

namespace OpenB.DSL
{
    public class Token
    {
        public int LineNumber { get; private set; }
        public int Position { get; private set; }
        public string Type { get; private set; }
        public string Contents { get; private set; }

        public override string ToString()
        {
            return $"({LineNumber},{Position}) {Type}:{Contents}";
        }

        public Token(int lineNumber, int position, string token, string tokenContents)
        {
            if (token == null)
                throw new ArgumentNullException(nameof(token));

            this.LineNumber = lineNumber;
            this.Position = position;
            this.Type = token;
            this.Contents = tokenContents;
        }
    }
}
