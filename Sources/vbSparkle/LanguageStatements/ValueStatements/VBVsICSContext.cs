using static vbSparkle.VBScriptParser;

namespace vbSparkle
{
    public class VBVsICSContext
        : VBValueStatement<VsICSContext>
    {
        private VbInStatement InStmt { get; set; }

        public VBVsICSContext(IVBScopeObject context, VsICSContext @object)
            : base(context, @object)
        {
            InStmt = new VbInStatement(context, @object.implicitCallStmt_InStmt());
        }

        public override DExpression Prettify(bool partialEvaluation = false)
        {
            if (partialEvaluation)
                try
                {
                    return Evaluate();
                }
                catch { }

            return InStmt.Prettify(partialEvaluation);
        }

        public override DExpression Evaluate()
        {
            return InStmt.Prettify(true);
        }
    }

}