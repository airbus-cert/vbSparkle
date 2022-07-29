using System.Collections.Generic;
using System.Text;
using static vbSparkle.VBScriptParser;

namespace vbSparkle
{
    public class VbWhileStatement
        : VbStatement<WhileWendStmtContext>
    {
        public bool IsWhile { get; set; }
        public List<VbSimpleStackBlock> CodeBlocks { get; set; } = new List<VbSimpleStackBlock>();
        public VBValueStatement Value { get; set; }

        public VbWhileStatement(IVBScopeObject context, WhileWendStmtContext bloc)
            : base(context, bloc)
        {
            Value = VBValueStatement.Get(context, bloc.valueStmt());

            foreach (var block in bloc.block())
                CodeBlocks.Add(new VbSimpleStackBlock(context, block));

        }

        public override DExpression Prettify(bool partialEvaluation = false)
        {
            StringBuilder retCode = new StringBuilder();

            retCode.AppendLine($"While {Value.Exp(partialEvaluation)}");

            foreach (var CodeBlock in CodeBlocks)
                retCode.AppendLine(Helpers.IndentLines(4, CodeBlock.Exp(partialEvaluation)));

            retCode.Append("Loop");

            return new DCodeBlock(retCode.ToString());
        }
    }
}