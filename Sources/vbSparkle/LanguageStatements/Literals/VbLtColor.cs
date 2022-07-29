using System;
using static vbSparkle.VBScriptParser;

namespace vbSparkle
{
    public class VbLtColor : VBLiteral<LtColorContext>
    {
        public VbLtColor(LtColorContext @object)
            : base(@object)
        {
            string quoted = @object.GetText();
            Value = new DMathExpression<Int32>( Convert.ToInt32(quoted.Substring(2).Replace("&",""), 16));
        }

        public override string Prettify()
        {
            DMathExpression<Int32> val = (DMathExpression<Int32>)Value;
            return $"&H{val.GetRealValue():X}";
        }
    }

}