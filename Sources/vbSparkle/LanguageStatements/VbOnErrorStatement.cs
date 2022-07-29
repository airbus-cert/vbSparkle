namespace vbSparkle
{
    public class VbOnErrorStatement : VbStatement<VBScriptParser.OnErrorStmtContext>
    {
        bool isLocalError = false;
        bool mustGoto = false;
        bool isValid = false;
        public VBValueStatement ValueStatement { get; set; }

        public VbOnErrorStatement(IVBScopeObject context, VBScriptParser.OnErrorStmtContext bloc)
            : base(context, bloc)
        {
            if (bloc.ON_LOCAL_ERROR() != null)
                isLocalError = true;

            mustGoto = bloc.GOTO() != null;

            if (mustGoto)
            {
                isValid = bloc.valueStmt() != null;
                ValueStatement = VBValueStatement.Get(context, bloc.valueStmt());

            }
            else
            {
                isValid = bloc.RESUME() != null && bloc.NEXT() != null;
            }

        }

        public override DExpression Prettify(bool partialEvaluation = false)
        {
            if (!isValid)
                return new DCodeBlock("// Unknown Syntax: " + this.Object.GetText());

            string errorPrefix = "On Error";

            if (isLocalError)
                errorPrefix = "On Local Error";

            if (mustGoto)
                return new DCodeBlock($"{errorPrefix} Goto {ValueStatement.Exp(partialEvaluation)}:");
            else
                return new DCodeBlock($"{errorPrefix} Resume Next");

        }
    }
}