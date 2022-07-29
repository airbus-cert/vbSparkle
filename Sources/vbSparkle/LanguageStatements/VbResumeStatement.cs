namespace vbSparkle
{
    public class VbResumeStatement : VbStatement<VBScriptParser.ResumeStmtContext>
    {
        public bool HasNext { get; set; }

        public VbResumeStatement(IVBScopeObject context, VBScriptParser.ResumeStmtContext bloc)
            : base(context, bloc)
        {
            HasNext = bloc.NEXT() != null;
        }

        public override DExpression Prettify(bool partialEvaluation)
        {
            if (HasNext)
                return new DCodeBlock($"Resume Next");
            else
                return new DCodeBlock($"Resume");
        }
    }
}