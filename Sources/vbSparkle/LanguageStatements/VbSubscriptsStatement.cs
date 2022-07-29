using System.Linq;

namespace vbSparkle
{
    public class VbSubscriptsStatement : VbStatement<VBScriptParser.SubscriptsContext>
    {
        public VbSubScriptStatement[] SubScripts { get; }

        public VbSubscriptsStatement(IVBScopeObject context, VBScriptParser.SubscriptsContext bloc)
            : base(context, bloc)
        {
            if (bloc == null)
                return;

            var subScripts = bloc.subscript();
            if (subScripts == null)
                return;

            SubScripts = subScripts.Select(v => new VbSubScriptStatement(context, v)).ToArray();
        }

        public override DExpression Prettify(bool partialEvaluation)
        {
            if (SubScripts == null)
                return new DCodeBlock(string.Empty);

            string declarations = string.Join(", ", SubScripts.Select(v => v.Exp(partialEvaluation)));
            return new DCodeBlock(declarations);
        }
    }
}