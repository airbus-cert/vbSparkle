using System;
using static vbSparkle.VBPreprocessorsParser;

namespace vbSparkle.PreProcessor.Statements
{
    public class VbLtInteger : VBLiteral<LtIntegerContext>
    {
        public VbLtInteger(LtIntegerContext @object)
            : base(@object)
        {
            string quoted = @object.GetText();
            if (quoted.EndsWith("#"))
                Value = new DMathExpression<double>(double.Parse(quoted.Substring(0, quoted.Length - 1)));
            else
                Value = new DMathExpression<Int32>(int.Parse(quoted));
        }

        public override string Prettify()
        {
            if (Value is DMathExpression<Int32>)
            {
                DMathExpression<Int32> val = (DMathExpression<Int32>)Value;
                return $"{val.GetRealValue()}";
            }
            else
            {
                DMathExpression<double> val = (DMathExpression<double>)Value;
                return $"{val.GetRealValue()}";
            }
        }
    }
}
