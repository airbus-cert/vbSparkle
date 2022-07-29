using System.Linq;

namespace vbSparkle
{
    public class VbSubScriptStatement : VbStatement<VBScriptParser.SubscriptContext>
    {
        public bool IsRange { get; set; }
        public VBValueStatement UBound { get; }
        public VBValueStatement LBound { get; }

        public VbSubScriptStatement(IVBScopeObject context, VBScriptParser.SubscriptContext bloc)
            : base(context, bloc)
        {
            IsRange = bloc.TO() != null;

            UBound = VBValueStatement.Get(context, bloc.valueStmt().LastOrDefault());

            if (IsRange)
                LBound = VBValueStatement.Get(context, bloc.valueStmt().FirstOrDefault());

        }

        public override DExpression Prettify(bool partialEvaluation = false)
        {
            if (IsRange)
                return new DCodeBlock($"{LBound.Exp(partialEvaluation)} To {UBound.Exp(partialEvaluation)}");

            return new DCodeBlock($"{UBound.Exp(partialEvaluation)}");
        }
    }
}