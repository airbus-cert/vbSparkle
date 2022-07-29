using System.Linq;

namespace vbSparkle
{
    public class VbMidStatement : VbStatement<VBScriptParser.MidStmtContext>
    {
        public string Identifier { get; }
        public VBArgCall Start { get; }
        public VBValueStatement ValueStatement { get; set; }
        public VBArgCall End { get; }

        public VbMidStatement(IVBScopeObject context, VBScriptParser.MidStmtContext bloc)
            : base(context, bloc)
        {
            Identifier = bloc.ambiguousIdentifier().GetText();

            var args = bloc.argCall();

            Start = new VBArgCall(context, args[0]);

            if (args.Count() > 1)
                End = new VBArgCall(context, args[1]);

            ValueStatement = VBValueStatement.Get(context, bloc.valueStmt());
        }

        public override DExpression Prettify(bool partialEvaluation = false)
        {
            if (End != null)
                return new DCodeBlock($"Mid({Identifier}, {Start.Exp(partialEvaluation)}, {End.Exp(partialEvaluation)}) = {ValueStatement.Exp(partialEvaluation)}");
            else
                return new DCodeBlock($"Mid({Identifier}, {Start.Exp(partialEvaluation)}) = {ValueStatement.Exp(partialEvaluation)}");
        }
    }
}