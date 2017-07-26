using System;
using OpenB.DSL.Reflection;

namespace OpenB.DSL.Expressions
{
    internal class ContextSelectionExpression : IExpression
    {       
        private IExpression itemType;
        private ContextQuantifier quantifier;
        private IExpression whereExpression;
        public ExpressionEvaluationContext Context { get; private set; } 

        public ContextSelectionExpression(ExpressionEvaluationContext context, object quantifier, IExpression whereExpression, IExpression contextList, IExpression itemType)
        {
           
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            Context = context;


            this.quantifier = (ContextQuantifier)Enum.Parse(typeof(ContextQuantifier), quantifier.ToString(), true);


            this.itemType = itemType;

            GetContextList(contextList, whereExpression);

        }

        private void GetContextList(IExpression contextListExpression, IExpression whereExpression)
        {
            if (whereExpression == null)
                throw new ArgumentNullException(nameof(whereExpression));
            if (contextListExpression == null)
                throw new ArgumentNullException(nameof(contextListExpression));

            FieldExpression fieldExpression = contextListExpression as FieldExpression;
            var contextPath = fieldExpression.FieldName.Split('.');

            if (contextPath.Length < 2)
            {
                throw new NotSupportedException();
            }

            // TODO: Select right assembly.


            Context.ModelEvaluator.Evaluate(itemType.ToString(), contextPath);
           
          
            
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