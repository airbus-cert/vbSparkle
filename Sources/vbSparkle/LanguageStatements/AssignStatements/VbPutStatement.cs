
using System.Linq;

namespace vbSparkle
{
    public class VbPutStatement : VbStatement<VBScriptParser.PutStmtContext>
    {
        public VBValueStatement[] ValueStatements { get; set; }

        public VbPutStatement(IVBScopeObject context, VBScriptParser.PutStmtContext bloc)
            : base(context, bloc)
        {
            ValueStatements = bloc.valueStmt().Select(v => VBValueStatement.Get(context, v)).ToArray();
        }

        public override DExpression Prettify(bool partialEvaluation = false)
        {
            var values = string.Join(", ", ValueStatements.Select(v => v.Exp(partialEvaluation)).ToArray());
            return new DCodeBlock($"Put# {values}");
        }
    }
}