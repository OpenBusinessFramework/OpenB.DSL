using System;

namespace OpenB.DSL.Expressions
{
    internal class LogicalAndExpression : EqualityExpressionBase
    {

        public LogicalAndExpression(IExpression leftHandExpression, IExpression rightHandExpression) : base(leftHandExpression, rightHandExpression)
        {

        }

        public override object Evaluate()
        {
            return (bool)Left.Evaluate() && (bool)Right.Evaluate();
        }

        public override string ToString()
        {
            return $"{Left} and {Right}";
        }

        public override string GenerateCode()
        {
            return $"({Left.GenerateCode()}) && ({Right.GenerateCode()})";
        }
    }
}