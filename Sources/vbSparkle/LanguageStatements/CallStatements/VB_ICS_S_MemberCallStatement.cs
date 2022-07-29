namespace vbSparkle
{
    public class VB_ICS_S_MemberCallStatement : VbStatement<VBScriptParser.ICS_S_MemberCallContext>
    {
        public VbStatement CallObject { get; set; }

        public VB_ICS_S_MemberCallStatement(IVBScopeObject context, VBScriptParser.ICS_S_MemberCallContext bloc)
            : base(context, bloc)
        {
            var vop = bloc.iCS_S_VariableOrProcedureCall();
            var poa = bloc.iCS_S_ProcedureOrArrayCall();
            
            if (vop != null)
                CallObject = new VB_ICS_S_VariableOrProcedureCallStatement(context, vop);

            if (poa != null)
                CallObject = new VB_ICS_S_ProcedureOrArrayCallStatement(context, poa);
        }

        public override DExpression Prettify(bool partialEvaluation = false)
        {
            return new DCodeBlock($".{CallObject.Exp(partialEvaluation)}");
        }
    }
    
}
