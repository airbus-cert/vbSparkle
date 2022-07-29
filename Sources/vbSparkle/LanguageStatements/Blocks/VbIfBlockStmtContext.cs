using System.Text;
using vbSparkle.NativeMethods;
using static vbSparkle.VBScriptParser;

namespace vbSparkle
{
    public class VbIfBlockStmtContext
        : VbStatement<IfBlockStmtContext>, IVbIfBlockStmt
    {
        public VbIfConditionStmtContext CondValue { get; set; }
        public VbSimpleStackBlock CodeBlock { get; set; }

        public VbIfBlockStmtContext(IVBScopeObject context, IfBlockStmtContext bloc)
            : base(context, bloc)
        {
            CondValue = new VbIfConditionStmtContext(context, bloc.ifConditionStmt());
            CodeBlock = new VbSimpleStackBlock(context, bloc.block());
        }

        public override DExpression Prettify(bool partialEvaluation = false)
        {
            StringBuilder retCode = new StringBuilder();

            //bool? isExecuted = CondValue.IsExecuted();

            //if (isExecuted != null && partialEvaluation)
            //{
            //    if (isExecuted == true)
            //    {
            //        retCode.AppendLine($"'If {CondValue.Exp(partialEvaluation)} Then");
            //        retCode.Append(Helpers.IndentLines(4, CodeBlock.Exp(partialEvaluation)));
            //    }
            //    else
            //    {
            //        retCode.AppendLine($"'If {CondValue.Exp(partialEvaluation)} Then");
            //        retCode.Append(Helpers.CommentLines(4, CodeBlock.Exp(false)));
            //    }
            //}
            //else
            //{
            //    retCode.AppendLine($"If {CondValue.Exp(partialEvaluation)} Then");
                retCode.Append(Helpers.IndentLines(4, CodeBlock.Exp(partialEvaluation)));
            //}


            return new DCodeBlock(retCode.ToString());
        }
    }
}