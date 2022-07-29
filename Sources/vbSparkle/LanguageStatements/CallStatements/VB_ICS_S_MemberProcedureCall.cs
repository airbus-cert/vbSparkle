using System.Collections.Generic;
using System.Linq;

namespace vbSparkle
{
    public class VB_ICS_S_MemberProcedureCall : VbStatement<VBScriptParser.ICS_B_MemberProcedureCallContext>
    {
        public string Identifier { get; set; }
        public VbInStatement InStatement { get; set; }
        public List<VBArgCall> CallArgs { get; set; } = new List<VBArgCall>();
        public VbDefaultMemberAccessStatement DefaultMemberAccess { get; }

        public VB_ICS_S_MemberProcedureCall(IVBScopeObject context, VBScriptParser.ICS_B_MemberProcedureCallContext bloc)
            : base(context, bloc)
        {
            Identifier = bloc.ambiguousIdentifier().GetText();
            
            var args = bloc.noParenthesisArgs()?.argsCall();

            if (args != null)
                foreach (var arg in args.argCall())
                {
                    CallArgs.Add(new VBArgCall(context, arg));
                }

            var inStmt = bloc.implicitCallStmt_InStmt();

            if (inStmt == null)
                InStatement = null;
            else
                InStatement = new VbInStatement(context, bloc.implicitCallStmt_InStmt());

            var defaultMemberAccess = bloc.defaultMemberAccess();

            if (defaultMemberAccess != null)
                DefaultMemberAccess = new VbDefaultMemberAccessStatement(context, defaultMemberAccess);
        }



        public VbIdentifiedObject IdentifiedObject
        {
            get { return Context.GetIdentifiedObject(Identifier); }
        }


        public override DExpression Prettify(bool partialEvaluation = false)
        {
            string inStatement = InStatement == null ? "" : InStatement.Exp(partialEvaluation);
            string callName = IdentifiedObject.Name;

            string ret = $"{inStatement}.{callName}";

            if (CallArgs.Any())
                ret += " " + string.Join(", ", CallArgs.Select(v => v.Exp(partialEvaluation)));

            if (DefaultMemberAccess != null)
                ret += " " + DefaultMemberAccess.Exp(partialEvaluation);

            return new DCodeBlock(ret);
        }
    }
    
}
