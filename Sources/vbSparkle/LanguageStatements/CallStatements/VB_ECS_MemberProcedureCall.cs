using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace vbSparkle
{

    /// <summary>
    /// Represent a simple call without parenthesis and return values.
    /// </summary>
    public class VB_ECS_MemberProcedureCall : VbStatement<VBScriptParser.ECS_MemberProcedureCallContext>
    {
        public VbInStatement InStatement { get; }
        public string PropertyName { get; set; }
        public string TypeHInt { get; set; }

        public List<VBArgCall> CallArgs { get; set; } = new List<VBArgCall>();

        public VB_ECS_MemberProcedureCall(
            IVBScopeObject context, 
            VBScriptParser.ECS_MemberProcedureCallContext bloc)
            : base(context, bloc)
        {
            InStatement = new VbInStatement(context, bloc.implicitCallStmt_InStmt());
            PropertyName = bloc.ambiguousIdentifier().GetText();
            TypeHInt = bloc.typeHint()?.GetText();


            var args = bloc.argsCall();

            if (args != null)
                foreach (var arg in args.argCall())
                {
                    CallArgs.Add(new VBArgCall(context, arg));
                }
        }

        public override DExpression Prettify(bool partialEvaluation = false)
        {
            string args = string.Join(", ", CallArgs.Select(v => v.Exp(partialEvaluation)));
            return new DCodeBlock($"Call {InStatement.Exp(partialEvaluation)}.{PropertyName}{TypeHInt}({args})");
        }
    }
}
