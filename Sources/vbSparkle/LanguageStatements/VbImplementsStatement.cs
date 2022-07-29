using System.Linq;

namespace vbSparkle
{
    public class VbImplementsStatement : VbStatement<VBScriptParser.ImplementsStmtContext>
    {
        public string Identifier { get; }

        public VbImplementsStatement(IVBScopeObject context, VBScriptParser.ImplementsStmtContext bloc)
            : base(context, bloc)
        {
            Identifier = bloc.ambiguousIdentifier().GetText();
        }

        public override DExpression Prettify(bool partialEvaluation = false)
        {
            return new DCodeBlock($"Implements {Identifier}");
        }
    }
}