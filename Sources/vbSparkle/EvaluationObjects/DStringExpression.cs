using System;
using System.Text;
using MathNet.Symbolics;
using vbSparkle;

namespace vbSparkle.EvaluationObjects
{

    internal class DSimpleStringExpression 
        : DExpression, IStringExpression
    {
        string var;

        public override bool HasSideEffet { get => false; set => throw new NotImplementedException(); }

        public override bool IsValuable { get => true; set => throw new NotImplementedException(); }
        public Encoding Encoding { get; set; }

        public DSimpleStringExpression(string value, Encoding encoding)
        {
            var = value;
            Encoding = encoding;
        }

        public override string ToExpressionString()
        {
            return VbUtils.StrValToExp(var);
        }

        internal override SymbolicExpression GetSymExp()
        {
            double value = 0;
            if (double.TryParse(var, out value))
            {
                return value;
            }

            return SymbolicExpression.Variable(ToExpressionString());
        }

        public override string ToString()
        {
            return ToExpressionString();
        }

        public override string ToValueString()
        {
            return var;
        }

        internal void SetValue(string v)
        {
            var = v;
        }
    }
}
