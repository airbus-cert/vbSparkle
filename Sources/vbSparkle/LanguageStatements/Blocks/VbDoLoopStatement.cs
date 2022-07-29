using System.Text;
using static vbSparkle.VBScriptParser;

namespace vbSparkle
{
    public class VbDoLoopStatement
        : VbStatement<DoLoopStmtContext>
    {
        public bool IsWhile { get; set; }
        public VbSimpleStackBlock CodeBlock { get; set; }
        public VBValueStatement Value { get; set; }

        public VbDoLoopStatement(IVBScopeObject context, DoLoopStmtContext bloc)
            : base(context, bloc)
        {
            if (bloc is DlStatementContext)
            {
                var bloc2 = bloc as DlStatementContext;
                CodeBlock = new VbSimpleStackBlock(context, bloc2.block());
            }
            if (bloc is DlwStatementContext)
            {
                var bloc2 = bloc as DlwStatementContext;
                Value = VBValueStatement.Get(context, bloc2.valueStmt());
                CodeBlock = new VbSimpleStackBlock(context, bloc2.block());
                IsWhile = bloc2.WHILE() != null;
            }
            if (bloc is DwlStatementContext)
            {
                var bloc2 = bloc as DwlStatementContext;
                Value = VBValueStatement.Get(context, bloc2.valueStmt());
                CodeBlock = new VbSimpleStackBlock(context, bloc2.block());
                IsWhile = bloc2.WHILE() != null;
            }
        }

        public override DExpression Prettify(bool partialEvaluation = false)
        {
            StringBuilder retCode = new StringBuilder();

            if (Object is DlStatementContext)
            {
                retCode.AppendLine("Do");
                retCode.AppendLine(Helpers.IndentLines(4, CodeBlock.Exp(partialEvaluation)));
                retCode.Append("Loop");
            }
            else
            {
                string whileOrUntil = IsWhile ? "While" : "Until";

                if (Object is DlwStatementContext)
                {
                    retCode.AppendLine("Do");
                    retCode.AppendLine(Helpers.IndentLines(4, CodeBlock.Exp(partialEvaluation)));
                    retCode.Append($"Loop {whileOrUntil} {Value.Exp(partialEvaluation)}");
                }

                if (Object is DwlStatementContext)
                {
                    retCode.AppendLine($"Do {whileOrUntil} {Value.Exp(partialEvaluation)}");
                    retCode.AppendLine(Helpers.IndentLines(4, CodeBlock.Exp(partialEvaluation)));
                    retCode.Append("Loop");
                }
            }

            return new DCodeBlock(retCode.ToString());
        }
    }
}