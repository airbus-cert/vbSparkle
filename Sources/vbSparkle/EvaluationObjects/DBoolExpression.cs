using System;
using MathNet.Symbolics;
using vbSparkle;

namespace vbSparkle.EvaluationObjects
{
    internal class DBoolExpression : DMathExpression<bool>
    {
        public DBoolExpression(SymbolicExpression exp)
            : base(exp)
        {
            this.MathObject = exp;
        }

        public DBoolExpression(bool value):
            base(value ? -1 : 0)
        {
        }

        public override object GetValueObject()
        {
            return base.GetValueObject();
        }

        public override bool IsValuable
        {
            get
            {
                return MathObject.Expression.IsNumber || MathObject.Expression.IsApproximation;
            }
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public override string ToExpressionString()
        {
            try
            {
                if (IsValuable)
                {
                    if (Convert.ToDouble(GetValueObject()) == 0)
                        return "False";
                    else
                        return "True";
                }
            }
            catch { }

            return base.ToExpressionString();

        }

    }
}
