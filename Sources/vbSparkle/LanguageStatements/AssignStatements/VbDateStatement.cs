namespace vbSparkle
{
    public class VbDateStatement : VbStatement<VBScriptParser.DateStmtContext>
    {
        public VBValueStatement ValueStatement { get; set; }

        public VbDateStatement(IVBScopeObject context, VBScriptParser.DateStmtContext bloc)
            : base(context, bloc)
        {
            ValueStatement = VBValueStatement.Get(context, bloc.valueStmt());
        }

        public override DExpression Prettify(bool partialEvaluation = false)
        {
            return new DCodeBlock($"Date = {ValueStatement.Exp(partialEvaluation)}");
        }

    }
}