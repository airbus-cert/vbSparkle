namespace vbSparkle
{
    public class VbLetStatement : VbStatement<VBScriptParser.LetStmtContext>
    {
        public bool HasPrefix = false;

        public VBValueStatement ValueStatement { get; set; }
        public VbInStatement InStatement { get; set; }

        public VbLetStatement(IVBScopeObject context, VBScriptParser.LetStmtContext bloc)
            : base(context, bloc)
        {
            HasPrefix = !string.IsNullOrWhiteSpace(bloc.LET()?.GetText());
            InStatement = new VbInStatement(context, bloc.implicitCallStmt_InStmt());
            ValueStatement = VBValueStatement.Get(context, bloc.valueStmt());
        }

        public override DExpression Prettify(bool partialEvaluation = false)
        {

            string prefix = HasPrefix ? "Let " : string.Empty;
            var code = new DCodeBlock($"{prefix}{InStatement.AssignExp(partialEvaluation)} = {ValueStatement.Exp(partialEvaluation)}");

            if (partialEvaluation)
            {
                InStatement.SetValue(ValueStatement.Evaluate());
                
            }

            return code;
        }
    }
}