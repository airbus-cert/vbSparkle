using System.Text;
using static vbSparkle.VBScriptParser;

namespace vbSparkle
{
    public class VbIfElseBlockStmtContext
        : VbStatement<IfElseBlockStmtContext>
    {
        public VbSimpleStackBlock CodeBlock { get; set; }

        public VbIfElseBlockStmtContext(IVBScopeObject context, IfElseBlockStmtContext bloc)
            : base(context, bloc)
        {
            CodeBlock = new VbSimpleStackBlock(context, bloc.block());
        }

        public override DExpression Prettify(bool partialEvaluation = false)
        {
            StringBuilder retCode = new StringBuilder();

            //retCode.AppendLine("Else");
            retCode.Append(Helpers.IndentLines(4, CodeBlock.Exp(partialEvaluation)));

            return new DCodeBlock(retCode.ToString());
        }
    }
}