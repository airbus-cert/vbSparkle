using System.Linq;

namespace vbSparkle
{
    public class VbAppActivateStatement : VbStatement<VBScriptParser.AppActivateStmtContext>
    {
        public VBValueStatement[] AppValues { get; set; }

        public VbAppActivateStatement(IVBScopeObject context, VBScriptParser.AppActivateStmtContext bloc)
            : base(context, bloc)
        {
            AppValues = bloc.valueStmt().Select(v => VBValueStatement.Get(context, v)).ToArray();
        }

        public override DExpression Prettify(bool partialEvaluation = false)
        {
            return new DCodeBlock($"AppActivate {string.Join(", ", AppValues.Select(v => v.Exp(partialEvaluation)))}");
        }
    }

}