namespace vbSparkle
{
    public class VbRSetStatement : VbStatement<VBScriptParser.RsetStmtContext>
    {
        public VBValueStatement ValueStatement { get; set; }
        public VbInStatement InStatement { get; set; }

        public VbRSetStatement(IVBScopeObject context, VBScriptParser.RsetStmtContext bloc)
            : base(context, bloc)
        {
            InStatement = new VbInStatement(context, bloc.implicitCallStmt_InStmt());
            ValueStatement = VBValueStatement.Get(context, bloc.valueStmt());
        }

        public override DExpression Prettify(bool partialEvaluation = false)
        {
            return new DCodeBlock($"RSet {InStatement.AssignExp(partialEvaluation)} = {ValueStatement.Exp(partialEvaluation)}");
        }

    }
}