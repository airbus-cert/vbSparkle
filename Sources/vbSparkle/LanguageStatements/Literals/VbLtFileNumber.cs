using System;
using static vbSparkle.VBScriptParser;

namespace vbSparkle
{
    public class VbLtFileNumber : VBLiteral<LtFilenumberContext>
    {
        public VbLtFileNumber(LtFilenumberContext @object)
            : base(@object)
        {
            string quoted = @object.GetText();
            Value = new DMathExpression<Int32>(int.Parse(quoted.Replace("#", "")));
        }

        public override string Prettify()
        {
            DMathExpression<Int32> val = (DMathExpression<Int32>)Value;
            return $"#{val.GetRealValue()}";
        }
    }

}