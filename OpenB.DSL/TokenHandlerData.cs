using System.Collections;
using System.Collections.Generic;

namespace OpenB.DSL
{

    public class TokenHandlerData
    {       
        public Queue OutputQueue { get; private set; }
        public Stack<IExpression> ExpressionStack { get; private set; }
        public ConstantExpressionFactory ConstantExpressionFactory { get; private set; }
        public OperatorExpressionFactory ExpressionFactory { get; private set; }
        

        public TokenHandlerData(ConstantExpressionFactory constantExpressionFactory, OperatorExpressionFactory expressionFactory)
        {        
            if (expressionFactory == null)
                throw new System.ArgumentNullException(nameof(expressionFactory));
            if (constantExpressionFactory == null)
                throw new System.ArgumentNullException(nameof(constantExpressionFactory));
         
            ExpressionFactory = expressionFactory;
            ConstantExpressionFactory = constantExpressionFactory;
            ExpressionStack = new Stack<IExpression>();
            OutputQueue = new Queue();
        }
    }
}