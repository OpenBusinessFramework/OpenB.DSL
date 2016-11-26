using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenB.DSL
{
    public class OperatorItem
    {
        public int Precedance { get; private set; }
        public string Operator { get; private set; }

       

        public OperatorItem(int precedance, string operatorString)
        {
            this.Operator = operatorString;
            this.Precedance = precedance;
        }
    }

    public class CoreConfiguration
    {
        public bool IgnoreWhiteSpace { get; private set; }
        public TokenDefinition[] TokenDefinitions { get; private set; }
        public  List<OperatorItem> OperatorPrecedance { get; private set; }

        public CoreConfiguration()
        {
            TokenDefinitions = new[] {
            new TokenDefinition(@"\[[A-Za-z]+(\.{0,1}[A-Za-z]+){0,}\]", "FIELD"),
            new TokenDefinition(@"([""'])(?:\\\1|.)*?\1", "QUOTED_STRING"),
            new TokenDefinition(@"\(", "SUB_EXPR_START"),
            new TokenDefinition(@"\)", "SUB_EXPR_END"),

            new TokenDefinition(@"[-+]?\d*\.\d+([eE][-+]?\d+)?", "FLOAT"),
            new TokenDefinition(@"[-+]?\d+", "INT"),
            new TokenDefinition(@"[\+|\-|\*|/]{1}", "OPERATOR"),
            new TokenDefinition(@"and", "LOGICAL_OPERATOR"),
            new TokenDefinition(@"or", "LOGICAL_OPERATOR"),
             new TokenDefinition(@"\s", "SPACE"),
            new TokenDefinition(@"\>", "EQUALITY_COMPARER"),
            new TokenDefinition(@"\<", "EQUALITY_COMPARER"),
            new TokenDefinition(@"!=", "EQUALITY_COMPARER"),
            new TokenDefinition(@"\=", "EQUALITY_COMPARER"),
            new TokenDefinition(@"[*<>\?\-+/A-Za-z->!]+", "SYMBOL"),
            new TokenDefinition(@"\.", "DOT"),              
            new TokenDefinition(@"true", "TRUE"),
            new TokenDefinition(@"false", "FALSE"),
           
            new TokenDefinition(@",", "SEPARATOR"),

            };

            IgnoreWhiteSpace = true;

            // TODO: Rooting
            OperatorPrecedance = new List<OperatorItem>
            {
                new OperatorItem(1, "^"),
                new OperatorItem(2, "*"),
                new OperatorItem(3, "/"),
                new OperatorItem(4, "+"),
                new OperatorItem(5, "-"),                
                new OperatorItem(6, "and"),
                new OperatorItem(7, "="),
                new OperatorItem(7, ">"),
                new OperatorItem(7, "<"),
                new OperatorItem(7, "!="),
                new OperatorItem(6, "or"),
                new OperatorItem(8, "("),
                new OperatorItem(8, ")"),
            };
           
        }

    }
}
