using System.Linq;

namespace vbSparkle
{
    public class VbRedimStatement : VbStatement<VBScriptParser.RedimStmtContext>
    {
        public VbReDimSubStatement[] SubStatements { get; }
        public bool Preserve { get; }

        public VbRedimStatement(IVBScopeObject context, VBScriptParser.RedimStmtContext bloc)
            : base(context, bloc)
        {
            SubStatements = bloc.redimSubStmt().Select(v=> new VbReDimSubStatement(context, v)).ToArray();
            Preserve = bloc.PRESERVE() != null;
        }

        public override DExpression Prettify(bool partialEvaluation)
        {
            string codeBlock;

            if (Preserve)
                codeBlock = "Redim Preserve ";
            else
                codeBlock = "Redim ";

            string subStmts = string.Join(", ", SubStatements.Select(v => v.Exp(partialEvaluation)));
            codeBlock += subStmts;

            return new DCodeBlock(codeBlock);
        }
    }
}