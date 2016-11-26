using System;

namespace OpenB.DSL
{
    internal class LogicalAndExpression : IEQualityExpression
    {
        private bool leftHandBoolean;
        private bool rightHandBoolean;

        public LogicalAndExpression(bool leftHandBoolean, bool rightHandBoolean)
        {
            this.leftHandBoolean = leftHandBoolean;
            this.rightHandBoolean = rightHandBoolean;
        }

        public object Evaluate()
        {
            return leftHandBoolean && rightHandBoolean;
        }
    }
}