using static vbSparkle.VBScriptParser;

namespace vbSparkle
{
    public class VBVsAddressOfContext
        : VBValueStatement<VsAddressOfContext>
    {
        public VBValueStatement ValueStatement { get; set; }

        public VBVsAddressOfContext(IVBScopeObject context, VsAddressOfContext @object)
            : base(context, @object)
        {
            ValueStatement = Get(context, @object.valueStmt());
        }

        public override DExpression Prettify(bool partialEvaluation = false)
        {
            if (partialEvaluation)
                try
                {
                    return Evaluate();
                }
                catch { }

            return new DCodeBlock($"AddressOf {ValueStatement.Exp(partialEvaluation)}");
        }

        public override DExpression Evaluate()
        {
            return Prettify(false);
        }
    }

}