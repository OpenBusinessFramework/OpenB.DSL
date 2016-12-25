using System;

namespace OpenB.DSL.Functions
{

    [ParserFunction("SQRT")]
    public class SquareRoot : IParserFunction
    {
        public double Number { get; private set; }

        public SquareRoot(double number)
        {
            Number = number;
        }

        public object Evaluate()
        {
            return Math.Sqrt(Number);
        }

        public string GenerateCode()
        {
            // TODO: Add dynamic usings.
            return $"System.Math.Sqrt({Number})";
        }
    }
}