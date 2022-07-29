using static vbSparkle.VBScriptParser;

namespace vbSparkle
{
    public class VBVsAssignContext
        : VBValueStatement<VsAssignContext>
    {
        public VBValueStatement ValueStatement { get; set; }
        private ImplicitCallStmt_InStmtContext InStmt { get; set; }

        public VBVsAssignContext(IVBScopeObject context, VsAssignContext @object)
            : base(context, @object)
        {
            InStmt = @object.implicitCallStmt_InStmt();
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

            return new DCodeBlock($"{InStmt.GetText()} := {ValueStatement.Exp(partialEvaluation)}");
        }

        public override DExpression Evaluate()
        {
            return Prettify(false);
        }
    }

}