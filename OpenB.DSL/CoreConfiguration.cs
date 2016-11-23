using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenB.DSL
{
    public class CoreConfiguration
    {
        public bool IgnoreWhiteSpace { get; private set; }
        public TokenDefinition[] TokenDefinitions { get; private set; }
        public string OperatorPrecedance { get; private set; }

        public CoreConfiguration()
        {
            TokenDefinitions = new [] {
            new TokenDefinition(@"\[[A-Za-z]+(\.{0,1}[A-Za-z]+){0,}\]", "FIELD"),
            new TokenDefinition(@"([""'])(?:\\\1|.)*?\1", "QUOTED_STRING"),
            new TokenDefinition(@"\(", "SUB_EXPR_START"),
            new TokenDefinition(@"\)", "SUB_EXPR_END"),
            new TokenDefinition(@"[\+|\-|\*|/]{1}", "OPERATOR"),
            new TokenDefinition(@"[*<>\?\-+/A-Za-z->!]+", "SYMBOL"),
            new TokenDefinition(@"[-+]?\d*\.\d+([eE][-+]?\d+)?", "FLOAT"),
            new TokenDefinition(@"[-+]?\d+", "INT"),
            new TokenDefinition(@"\.", "DOT"),
            new TokenDefinition(@"\s", "SPACE"),
            new TokenDefinition(@"\>", "EQUALITY_COMPARER"),
            new TokenDefinition(@"\<", "EQUALITY_COMPARER"),
            new TokenDefinition(@"\=", "EQUALITY_COMPARER"),
            new TokenDefinition(@"\!", "EQUALITY_COMPARER"),
            new TokenDefinition(@"true", "TRUE"),
            new TokenDefinition(@"false", "FALSE"),
            new TokenDefinition(@"and", "LOG_AND"),
            new TokenDefinition(@"or", "LOG_OR"),
            new TokenDefinition(@",", "SEPARATOR"),
           
            };

            IgnoreWhiteSpace = true;

            // TODO: Rooting
            OperatorPrecedance = $"^*/+-";
    }

    }
}
