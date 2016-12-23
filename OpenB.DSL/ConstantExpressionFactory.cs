using System;
using System.Globalization;
using OpenB.DSL.Reflection;

namespace OpenB.DSL
{
    internal class ConstantExpressionFactory
    {
        readonly CultureInfo cultureInfo;

        readonly ModelEvaluator modelEvaluator;

        public ConstantExpressionFactory(CultureInfo cultureInfo, ModelEvaluator modelEvaluator)
        {
            if (modelEvaluator == null)
                throw new ArgumentNullException(nameof(modelEvaluator));
            if (cultureInfo == null)
                throw new ArgumentNullException(nameof(cultureInfo));

            this.cultureInfo = cultureInfo;
            this.modelEvaluator = modelEvaluator;
        }

        internal IExpression Evaluate(Token token)
        {   

            if (token.Type == "INT")
            {
                return new NumberConstantExpression(int.Parse(token.Contents, cultureInfo));
            }

            if (token.Type == "QUOTED_STRING")
            {
                return new StringConstantExpression(token.Contents.Replace("'", ""));
            }

            if (token.Type == "BOOLEAN")
            {
                return new BooleanConstantExpression(bool.Parse(token.Contents));
            }

            throw new NotSupportedException($"Cannot evaluate token of type {token.Type} at {token.LineNumber}, {token.Position}.");
        }
    }
}