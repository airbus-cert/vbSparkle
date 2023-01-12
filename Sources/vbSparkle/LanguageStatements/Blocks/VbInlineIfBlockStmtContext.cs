using System.Text;
using static vbSparkle.VBScriptParser;

namespace vbSparkle
{

    public class VbInlineIfBlockStmtContext
        : VbStatement<InlineIfBlockStmtContext>, IVbIfBlockStmt
    {
        public VbIfConditionStmtContext CondValue { get; set; }
        public VbInlineSimpleStackBlock CodeBlock { get; set; }

        public VbInlineIfBlockStmtContext(IVBScopeObject context, InlineIfBlockStmtContext bloc)
            : base(context, bloc)
        {
            CondValue = new VbIfConditionStmtContext(context, bloc.ifConditionStmt());
            CodeBlock = new VbInlineSimpleStackBlock(context, bloc.inlineBlock());
        }

        public override DExpression Prettify(bool partialEvaluation = false)
        {
            StringBuilder retCode = new StringBuilder();

            retCode.AppendLine($"If {CondValue.Exp(partialEvaluation)} Then");
            retCode.Append(Helpers.IndentLines(Context.Options.IndentSpacing, CodeBlock.Exp(partialEvaluation)));

            return new DCodeBlock(retCode.ToString());
        }
    }
}