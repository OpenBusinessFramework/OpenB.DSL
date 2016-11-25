using System;
using System.Globalization;

namespace OpenB.DSL
{
    internal class TokenEvaluator
    {
        readonly CultureInfo cultureInfo;

        public TokenEvaluator(CultureInfo cultureInfo)
        {
            this.cultureInfo = cultureInfo;
            if (cultureInfo == null)
                throw new ArgumentNullException(nameof(cultureInfo));
        }

        internal object Evaluate(Token token)
        {
            if (token.Type == "INT")
            {
                return int.Parse(token.Contents, cultureInfo);
            }

            throw new NotSupportedException($"Cannot evaluate token of type {token.Type} at {token.LineNumber}, {token.Position}.");
        }
    }
}