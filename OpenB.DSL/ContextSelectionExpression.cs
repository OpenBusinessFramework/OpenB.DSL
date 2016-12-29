using System;

namespace OpenB.DSL
{
    internal class ContextSelectionExpression : IExpression
    {
        private IExpression contextList;
        private IExpression itemType;
        private ContextQuantifier quantifier;
        private IExpression whereExpression;

        public ContextSelectionExpression(object quantifier, IExpression whereExpression, IExpression contextList, IExpression itemType)
        {
            this.quantifier = (ContextQuantifier) Enum.Parse(typeof(ContextQuantifier), quantifier.ToString(), true);
            this.whereExpression = whereExpression;
            this.contextList = contextList;
            this.itemType = itemType;
        }

        public object Evaluate()
        {
            throw new NotImplementedException();
        }

        public string GenerateCode()
        {
            throw new NotImplementedException();
        }
    }

    internal enum ContextQuantifier
    {
        All,
        One
    }
}