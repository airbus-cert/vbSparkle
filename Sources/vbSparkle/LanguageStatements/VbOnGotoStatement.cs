using System.Linq;

namespace vbSparkle
{
    public class VbOnGotoStatement : VbStatement<VBScriptParser.OnGoToStmtContext>
    {
        public VBValueStatement OnValue { get; set; }
        public VBValueStatement[] GotoValues { get; set; }

        public VbOnGotoStatement(IVBScopeObject context, VBScriptParser.OnGoToStmtContext bloc)
            : base(context, bloc)
        {
            OnValue = VBValueStatement.Get(context, bloc.valueStmt().First());
            GotoValues = bloc.valueStmt().Skip(1).Select(v => VBValueStatement.Get(context, v)).ToArray();
        }

        public override DExpression Prettify(bool partialEvaluation = false)
        {
            return new DCodeBlock($"On {OnValue.Exp(partialEvaluation)} Goto {string.Join(", ", GotoValues.Select(v=> v.Exp(partialEvaluation)))}");
        }
    }
}