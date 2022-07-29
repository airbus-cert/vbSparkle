using System;
using MathNet.Symbolics;
using vbSparkle.EvaluationObjects;

namespace vbSparkle
{
    public abstract class DExpression
    {
        //public static DExpression DoOperation(string op, DExpression left, DExpression right)
        //{
        //    DExpression result;

        //    if (op == "&")
        //    {
        //        result = 
        //    }
        //    else
        //    {
        //        if (left is DMathExpression)
        //        {
        //            try
        //            {
        //                result = Operation.DoOperation(op, (DMathExpression)left, right); // ((DValueLiteral) value).DoOperationN(op, (DExpression) value2);
        //            }
        //            catch (Exception e)
        //            {
        //                result = new DCodeBlock($"{left.ToString()} {op} {right.ToString()}");
        //            }
        //        }

        //        else
        //        {
        //            result = new DCodeBlock($"{left.ToString()} {op} {right.ToString()}");
        //        }
        //    }

        //    return result;
        //}

        public abstract bool IsValuable { get; set; }

        public abstract string ToValueString();

        public abstract string ToExpressionString();
        public abstract bool HasSideEffet { get; set; }

        internal abstract SymbolicExpression GetSymExp();
    }
}