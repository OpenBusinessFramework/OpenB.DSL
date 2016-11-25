namespace OpenB.DSL.Functions
{

    public interface IParserFunction : IExpression
    {
        object Evaluate();
    }
}