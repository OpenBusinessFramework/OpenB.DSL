namespace OpenB.DSL
{

    public interface IParser
    {
        ParserResult Parse(ExpressionEvaluationContext parserContext, string expression);
    }
}