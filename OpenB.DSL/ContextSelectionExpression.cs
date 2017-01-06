using System;
using OpenB.DSL.Reflection;

namespace OpenB.DSL
{
    internal class ContextSelectionExpression : IExpression
    {       
        private IExpression itemType;
        private ContextQuantifier quantifier;
        private IExpression whereExpression;
        public ParserContext Context { get; private set; } 

        public ContextSelectionExpression(ParserContext context, object quantifier, IExpression whereExpression, IExpression contextList, IExpression itemType)
        {
           
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            Context = context;

            GetContextList(contextList, whereExpression);

            this.quantifier = (ContextQuantifier) Enum.Parse(typeof(ContextQuantifier), quantifier.ToString(), true);
            this.whereExpression = whereExpression;

            
            //this.contextList = contextList;
            this.itemType = itemType;
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
            Type modelType = Type.GetType(contextPath[0]);
            

           // ModelEvaluator modelEvaluator = new ModelEvaluator()
            
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