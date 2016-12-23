namespace OpenB.DSL
{
    public class ParserResult
    {
        public bool Outcome { get; private set; }

        public ParserResult(bool outcome)
        {
            this.Outcome = outcome;
        }         
    }
}