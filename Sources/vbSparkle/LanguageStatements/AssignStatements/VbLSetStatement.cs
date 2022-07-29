namespace vbSparkle
{
    public class VbLSetStatement : VbStatement<VBScriptParser.LsetStmtContext>
    {
        public VBValueStatement ValueStatement { get; set; }
        public VbInStatement InStatement { get; set; }

        public VbLSetStatement(IVBScopeObject context, VBScriptParser.LsetStmtContext bloc)
            : base(context, bloc)
        {
            InStatement = new VbInStatement(context, bloc.implicitCallStmt_InStmt());
            ValueStatement = VBValueStatement.Get(context, bloc.valueStmt());
        }

        public override DExpression Prettify(bool partialEvaluation = false)
        {
            return new DCodeBlock($"LSet {InStatement.AssignExp(partialEvaluation)} = {ValueStatement.Exp(partialEvaluation)}");
        }

    }
}