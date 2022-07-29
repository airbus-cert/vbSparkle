namespace vbSparkle
{
    public class VbTimeStatement : VbStatement<VBScriptParser.TimeStmtContext>
    {
        public VBValueStatement ValueStatement { get; set; }

        public VbTimeStatement(IVBScopeObject context, VBScriptParser.TimeStmtContext bloc)
            : base(context, bloc)
        {
            ValueStatement = VBValueStatement.Get(context, bloc.valueStmt());
        }

        public override DExpression Prettify(bool partialEvaluation = false)
        {
            return new DCodeBlock($"Time = {ValueStatement.Exp(partialEvaluation)}");
        }

    }
}