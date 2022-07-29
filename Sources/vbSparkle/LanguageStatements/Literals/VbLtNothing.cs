using static vbSparkle.VBScriptParser;

namespace vbSparkle
{
    public class VbLtNothing : VBLiteral<LtNothingContext>
    {
        public VbLtNothing(LtNothingContext @object)
            : base(@object)
        {
            Value = new DCodeBlock("Nothing");
        }

        public override string Prettify()
        {
            return "Nothing";
        }
    }

}