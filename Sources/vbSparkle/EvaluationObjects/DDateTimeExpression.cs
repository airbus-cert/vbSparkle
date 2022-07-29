using MathNet.Symbolics;
using System;
using System.Collections.Generic;
using System.Text;
using vbSparkle;

namespace vbSparkle.EvaluationObjects
{
    internal class DDateTimeExpression : DMathExpression<DateTime>
    {

        public DDateTimeExpression(SymbolicExpression exp)
            : base(exp)
        {
            MathObject = exp;
        }

        public DDateTimeExpression(DateTime value) 
            : base(value.ToOADate())
        {
        }


        public override string ToString()
        {
            return base.ToString();
        }

        internal DateTime? AsDateTime()
        {
            try
            {
                double value = MathObject.RealNumberValue;
                DateTime ret = DateTime.FromOADate(value);
                return ret;
            }
            catch
            {
                return (DateTime?)null;
            }
        }

        public override string ToExpressionString()
        {
            DateTime? res = AsDateTime();

            if (res.HasValue)
                return $"#{res:yyyy-MM-dd}#";
            else
                return MathObject.ToString();
        }
    }
}
