namespace OpenB.DSL
{
    public class ParserResult
    {
        public IExpression CompiledExpression { get; private set; }

        public ParserResult(IExpression compiledExpression)
        {
            this.CompiledExpression = compiledExpression;
        }         
    }
}