using System.Collections.Generic;

namespace OpenB.DSL.Expressions
{
    public interface IEQualityExpression : IExpression
    {
        IExpression Left { get; }
        IExpression Right { get; }
    }

    public interface IComplexExpression : IExpression
    {
        IList<IExpression> ChildExpressions { get; }
    }

    public interface IExpression
    {
        object Evaluate();
        string GenerateCode();
    }
}