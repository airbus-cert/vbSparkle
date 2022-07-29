using System.Linq;

namespace vbSparkle
{
    public class VbNameAsStatement : VbStatement<VBScriptParser.NameStmtContext>
    {
        public VBValueStatement SrcValue { get; set; }
        public VBValueStatement DstValue { get; set; }

        public VbNameAsStatement(IVBScopeObject context, VBScriptParser.NameStmtContext bloc)
            : base(context, bloc)
        {
            SrcValue = VBValueStatement.Get(context, bloc.valueStmt().First());
            DstValue = VBValueStatement.Get(context, bloc.valueStmt().Last());
        }


        public override DExpression Prettify(bool partialEvaluation = false)
        {
            return new DCodeBlock($"Name {SrcValue.Exp(partialEvaluation)} As {DstValue.Exp(partialEvaluation)}");
        }
    }
}