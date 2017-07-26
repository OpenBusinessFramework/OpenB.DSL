using System.Collections.Generic;

namespace OpenB.DSL.Expressions
{
    public interface IEQualityExpression : IExpression
    {
        
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