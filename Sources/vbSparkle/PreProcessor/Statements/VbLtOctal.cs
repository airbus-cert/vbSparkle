using System;
using static vbSparkle.VBPreprocessorsParser;

namespace vbSparkle.PreProcessor.Statements
{
    public class VbLtOctal : VBLiteral<LtOctalContext>
    {
        public VbLtOctal(LtOctalContext @object)
            : base(@object)
        {
            string quoted = @object.GetText();
            Value = new DMathExpression<Int32>(Convert.ToInt32(quoted.Substring(2, quoted.Length - 2), 8));
        }

        public override string Prettify()
        {
            DMathExpression<Int32> val = (DMathExpression<Int32>)Value;
            return $"&O{Convert.ToString(val.GetRealValue(), 8)}";
        }
    }
}
