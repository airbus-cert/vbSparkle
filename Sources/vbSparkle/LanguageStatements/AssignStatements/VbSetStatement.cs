namespace vbSparkle
{
    public class VbSetStatement : VbStatement<VBScriptParser.SetStmtContext>
    {
        public VBValueStatement ValueStatement { get; set; }
        public VbInStatement InStatement { get; set; }

        public VbSetStatement(IVBScopeObject context, VBScriptParser.SetStmtContext bloc) 
            : base(context, bloc)
        {
            InStatement = new VbInStatement(context, bloc.implicitCallStmt_InStmt());
            ValueStatement = VBValueStatement.Get(context, bloc.valueStmt());
        }

        public override DExpression Prettify(bool partialEvaluation = false)
        {
            return new DCodeBlock($"Set {InStatement.AssignExp(partialEvaluation)} = {ValueStatement.Exp(partialEvaluation)}");
        }

    }
}