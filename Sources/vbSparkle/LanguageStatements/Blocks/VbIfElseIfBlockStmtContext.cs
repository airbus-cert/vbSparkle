using System.Text;
using static vbSparkle.VBScriptParser;

namespace vbSparkle
{
    public class VbIfElseIfBlockStmtContext
        : VbStatement<IfElseIfBlockStmtContext>, IVbIfBlockStmt
    {
        public VbIfConditionStmtContext CondValue { get; set; }
        public VbSimpleStackBlock CodeBlock { get; set; }
        public VbIfElseIfBlockStmtContext(IVBScopeObject context, IfElseIfBlockStmtContext bloc)
            : base(context, bloc)
        {
            CondValue = new VbIfConditionStmtContext(context, bloc.ifConditionStmt());
            CodeBlock = new VbSimpleStackBlock(context, bloc.block());
        }

        public override DExpression Prettify(bool partialEvaluation = false)
        {
            //StringBuilder retCode = new StringBuilder();

            //retCode.AppendLine($"ElseIf {CondValue.Exp(partialEvaluation)} Then");
            //retCode.Append(Helpers.IndentLines(Context.Options.IndentSpacing, CodeBlock.Exp(partialEvaluation)));

            return CodeBlock.Prettify(partialEvaluation);// new DCodeBlock(retCode.ToString());
        }
    }
}