namespace vbSparkle
{
    public class VbRandomizeStatement : VbStatement<VBScriptParser.RandomizeStmtContext>
    {
        public VBValueStatement Value { get; set; }

        public VbRandomizeStatement(IVBScopeObject context, VBScriptParser.RandomizeStmtContext bloc)
            : base(context, bloc)
        {
            var valueStmt = bloc.valueStmt();
            if (valueStmt != null)
                Value = VBValueStatement.Get(context, valueStmt);
        }

        public override DExpression Prettify(bool partialEvaluation = false)
        {
            if (Value != null)
                return new DCodeBlock($"Randomize {Value.Exp(partialEvaluation)}");
            else
                return new DCodeBlock($"Randomize");
        }
    }
}