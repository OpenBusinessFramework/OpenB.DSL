using System;
using System.Globalization;
using System.Text.RegularExpressions;
using OpenB.DSL.Reflection;

namespace OpenB.DSL
{
    internal class TokenEvaluator
    {
        readonly CultureInfo cultureInfo;

        readonly ModelEvaluator modelEvaluator;

        public TokenEvaluator(CultureInfo cultureInfo, ModelEvaluator modelEvaluator)
        {            
            if (modelEvaluator == null)
                throw new ArgumentNullException(nameof(modelEvaluator));
            if (cultureInfo == null)
                throw new ArgumentNullException(nameof(cultureInfo));

            this.cultureInfo = cultureInfo;
            this.modelEvaluator = modelEvaluator;
        }

        internal object Evaluate(Token token)
        {
            if (token.Type == "FIELD")
            {
                Regex fieldExpression = new Regex(@"\[(?<contents>.*)\]");
                Match match = fieldExpression.Match(token.Contents);
                if (match.Success && match.Groups.Count > 0)
                {
                    return modelEvaluator.Evaluate(match.Groups["contents"].Value);
                }
                throw new NotSupportedException("Cannot parse field");
               
            }

            if (token.Type == "INT")
            {
                return int.Parse(token.Contents, cultureInfo);
            }

            if (token.Type == "QUOTED_STRING")
            {
                return token.Contents.Replace("'", "");                
            }

            if (token.Type == "BOOLEAN")
            {
                return bool.Parse(token.Contents);
            }

            throw new NotSupportedException($"Cannot evaluate token of type {token.Type} at {token.LineNumber}, {token.Position}.");
        }
    }
}