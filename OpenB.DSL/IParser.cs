namespace OpenB.DSL
{

    public interface IParser
    {
        ParserResult Parse(ParserContext parserContext, string expression);
    }
}