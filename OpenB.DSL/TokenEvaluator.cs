using System;
using System.Globalization;
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
                return modelEvaluator.Evaluate(token.Contents);
            }

            if (token.Type == "INT")
            {
                return int.Parse(token.Contents, cultureInfo);
            }

            if (token.Type == "QUOTED_STRING")
            {
                return token.Contents.Replace("'", "");                
            }

            throw new NotSupportedException($"Cannot evaluate token of type {token.Type} at {token.LineNumber}, {token.Position}.");
        }
    }
}