using System.Collections.Generic;

namespace OpenB.DSL
{
    public interface IEQualityExpression : IExpression
    {
        
    }

    public interface IComplexExpression : IExpression
    {
        IList<IExpression> Children { get; }
    }

    public interface IExpression
    {
        object Evaluate();
    }
}