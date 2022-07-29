using System.Collections.Generic;
using System.Linq;
using System.Text;
using static vbSparkle.VBScriptParser;

namespace vbSparkle
{
    public class VbSelectCaseStatement
        : VbStatement<SelectCaseStmtContext>
    {
        VBValueStatement Cond { get; set; }
        public List<VbCaseStatement> CaseBlocks { get; set; } = new List<VbCaseStatement>();

        public VbSelectCaseStatement(IVBScopeObject context, SelectCaseStmtContext bloc)
            : base(context, bloc)
        {
            Cond = VBValueStatement.Get(context, bloc.valueStmt());

            var cases = bloc.sC_Case().ToArray();

            foreach (var v in cases)
            {
                CaseBlocks.Add(new VbCaseStatement(context, v));
            }
        }
        public override DExpression Prettify(bool partialEvaluation = false)
        {
            StringBuilder retCode = new StringBuilder();

            retCode.AppendLine($"Select Case {Cond.Exp(partialEvaluation)}");

            foreach (var v in CaseBlocks)
            {
                retCode.AppendLine(Helpers.IndentLines(4, v.Exp(partialEvaluation)));
            }

            retCode.Append("End Case");


            return new DCodeBlock(retCode.ToString());
        }
    }
}