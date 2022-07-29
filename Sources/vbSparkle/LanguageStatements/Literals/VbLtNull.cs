using static vbSparkle.VBScriptParser;

namespace vbSparkle
{
    public class VbLtNull : VBLiteral<LtNullContext>
    {
        public VbLtNull(LtNullContext @object)
            : base(@object)
        {
            Value = new DCodeBlock("Null");
        }

        public override string Prettify()
        {
            return "Null";
        }
    }

}