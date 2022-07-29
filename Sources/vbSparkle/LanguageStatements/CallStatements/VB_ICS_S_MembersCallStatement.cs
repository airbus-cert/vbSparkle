using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vbSparkle
{
    public class VB_ICS_S_MembersCallStatement : VbValueAssignableStatement<VBScriptParser.ICS_S_MembersCallContext>
    {
        public VbStatement CallObject { get; set; }
        public VbDefaultMemberAccessStatement DefaultMemberAccess { get; set; }
        private List<VB_ICS_S_MemberCallStatement> MemberCalls { get; set; } = new List<VB_ICS_S_MemberCallStatement>();

        public VB_ICS_S_MembersCallStatement(IVBScopeObject context, VBScriptParser.ICS_S_MembersCallContext bloc)
            : base(context, bloc)
        {
            var vop = bloc.iCS_S_VariableOrProcedureCall();
            var poa = bloc.iCS_S_ProcedureOrArrayCall();

            if (vop != null)
                CallObject = new VB_ICS_S_VariableOrProcedureCallStatement(context, vop);

            if (poa != null)
                CallObject = new VB_ICS_S_ProcedureOrArrayCallStatement(context, poa);
            
            foreach (var item in bloc.iCS_S_MemberCall())
            {
                MemberCalls.Add(new VB_ICS_S_MemberCallStatement(context, item));
            }

            var defaultMemberAccess = bloc.defaultMemberAccess();
            if (defaultMemberAccess != null)
                DefaultMemberAccess = new VbDefaultMemberAccessStatement(context, defaultMemberAccess);
        }

        public override DExpression Prettify(bool partialEvaluation = false)
        {
            return GetAssignableExpression(partialEvaluation);
        }

        public override DExpression GetAssignableExpression(bool partialEvaluation = false)
        {
            StringBuilder sb = new StringBuilder();
            if (CallObject != null)
                sb.Append(CallObject.Exp(partialEvaluation));

            foreach (var memberCall in MemberCalls)
            {
                sb.Append(memberCall.Exp(partialEvaluation));
            }

            if (DefaultMemberAccess != null)
                sb.Append(DefaultMemberAccess.Exp(partialEvaluation));

            return new DCodeBlock(sb.ToString());
        }
    }
    
}
