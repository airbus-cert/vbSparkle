using System.Linq;
using System.Text;
using static vbSparkle.VBScriptParser;

namespace vbSparkle
{
    public class VbWithStatement
        : VbStatement<WithStmtContext>
    {
        VbInStatement ObjStatement { get; set; }

        public VbSimpleStackBlock CodeBlock { get; set; }
        public VBValueStatement Value { get; set; }

        public VbWithStatement(IVBScopeObject context, WithStmtContext bloc)
            : base(context, bloc)
        {
            ObjStatement = new VbInStatement(context, bloc.implicitCallStmt_InStmt());

            CodeBlock = new VbSimpleStackBlock(context, bloc.block());

        }

        public override DExpression Prettify(bool partialEvaluation = false)
        {
            StringBuilder retCode = new StringBuilder();

            retCode.AppendLine($"With {ObjStatement.Exp(partialEvaluation)}");

            retCode.AppendLine(Helpers.IndentLines(Context.Options.IndentSpacing, CodeBlock.Exp(partialEvaluation)));

            retCode.Append($"End With");


            return new DCodeBlock(retCode.ToString());
        }
    }
}