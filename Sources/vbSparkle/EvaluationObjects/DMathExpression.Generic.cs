
using MathNet.Symbolics;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Expr = MathNet.Symbolics.SymbolicExpression;

namespace vbSparkle
{
    internal class DMathExpression<T> : DMathExpression
        where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>
    {
        public override bool HasSideEffet { get; set; }

        public DMathExpression(SymbolicExpression value)
        {
            MathObject = value;
        }

        public DMathExpression(T value)
        {
            if (IsNumericType(value))
            {
                MathObject = Convert.ToDouble(value);
                return;
            }
        }

        public T GetRealValue()
        {
            return (T)Convert.ChangeType(MathObject.RealNumberValue, typeof(T));
        }

        public static bool IsNumericType(object o)
        {
            switch (Type.GetTypeCode(o.GetType()))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }

        public override object GetValueObject()
        {
            return MathObject.RealNumberValue;
        }

        public override bool IsValuable
        {
            get
            {
                return MathObject.Expression.IsNumber || MathObject.Expression.IsApproximation;
            }
            set => throw new NotImplementedException();
        }

        public override string ToExpressionString()
        {
            if (MathObject.Expression.IsNumber)
            {
                return MathObject.RealNumberValue.ToString();
            }

            return MathObject.ToString();
        }

        internal override Expr GetSymExp()
        {
            return this.MathObject;
        }

        public override string ToValueString()
        {
            if (IsValuable)
                return ToExpressionString();
            else
                throw new Exception("Not Valuable");
        }

    }
}