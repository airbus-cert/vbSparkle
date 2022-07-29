using MathNet.Symbolics;
using System;
using System.Collections.Generic;
using System.Text;

namespace vbSparkle.EvaluationObjects
{
    public class DEmptyVariable : DExpression
    {
        public override bool IsValuable { get => false; set => throw new NotImplementedException(); }
        public override bool HasSideEffet { get => true; set => throw new NotImplementedException(); }

        public override string ToExpressionString()
        {
            throw new NotImplementedException();
        }

        public override string ToValueString()
        {
            throw new NotImplementedException();
        }

        internal override SymbolicExpression GetSymExp()
        {
            throw new NotImplementedException();
        }
    }
}
