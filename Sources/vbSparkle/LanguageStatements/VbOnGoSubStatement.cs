using System.Linq;

namespace vbSparkle
{
    public class VbOnGoSubStatement : VbStatement<VBScriptParser.OnGoSubStmtContext>
    {
        public VBValueStatement OnValue { get; set; }
        public VBValueStatement[] GotoValues { get; set; }

        public VbOnGoSubStatement(IVBScopeObject context, VBScriptParser.OnGoSubStmtContext bloc)
            : base(context, bloc)
        {
            OnValue = VBValueStatement.Get(context, bloc.valueStmt().First());
            GotoValues = bloc.valueStmt().Skip(1).Select(v => VBValueStatement.Get(context, v)).ToArray();
        }

        public override DExpression Prettify(bool partialEvaluation)
        {
            return new DCodeBlock($"On {OnValue.Exp(partialEvaluation)} GoSub {string.Join(", ", GotoValues.Select(v => v.Exp(partialEvaluation)))}");
        }
    }
}