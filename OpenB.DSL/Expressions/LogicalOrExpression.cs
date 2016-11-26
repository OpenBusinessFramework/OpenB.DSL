using System;

namespace OpenB.DSL
{
    internal class LogicalOrExpression : IEQualityExpression
    {
        private bool leftHandBoolean;
        private bool rightHandBoolean;

        public LogicalOrExpression(bool leftHandBoolean, bool rightHandBoolean)
        {
            this.leftHandBoolean = leftHandBoolean;
            this.rightHandBoolean = rightHandBoolean;
        }

        public object Evaluate()
        {
            return leftHandBoolean || rightHandBoolean;
        }
    }
}