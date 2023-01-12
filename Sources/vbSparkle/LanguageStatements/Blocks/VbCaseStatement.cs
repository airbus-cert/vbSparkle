using System.Collections.Generic;
using System.Linq;
using System.Text;
using static vbSparkle.VBScriptParser;

namespace vbSparkle
{

    public class VbCaseStatement : VbStatement<SC_CaseContext>
    {
        public bool ElseCond { get; set; } = false;
        public List<VbCondExpStatement> CondExpStmt { get; set; } = new List<VbCondExpStatement>();
        public VbSimpleStackBlock stackBlock { get; set; }

        public VbCaseStatement(IVBScopeObject context, SC_CaseContext obj) 
            : base(context, obj)
        {
            var codeBlock = obj.block();
            stackBlock = new VbSimpleStackBlock(this.Context, codeBlock);
            var cond = obj.sC_Cond();
            if (cond is CaseCondElseContext)
            {
                ElseCond = true;
            } 
            else if (cond is CaseCondExprContext)
            {
                var condExp = (cond as CaseCondExprContext);
                var condExpStmt = condExp.sC_CondExpr();

                foreach (var v in condExpStmt)
                {
                    CondExpStmt.Add(new VbCondExpStatement(context, v));
                }
            }
        }

        public override DExpression Prettify(bool partialEvaluation = false)
        {
            StringBuilder sb = new StringBuilder();

            string caseCond = ElseCond ? "Else" : string.Join(", ", CondExpStmt.Select(v => v.Exp(partialEvaluation)).ToArray());

            sb.AppendLine($"Case {caseCond}:");
            sb.Append(Helpers.IndentLines(Context.Options.IndentSpacing, stackBlock.Exp(partialEvaluation)));

            return new DCodeBlock(sb.ToString());
        }
    }
}