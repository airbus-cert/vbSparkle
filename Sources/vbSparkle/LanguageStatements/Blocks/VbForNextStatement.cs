using System.Collections.Generic;
using System.Linq;
using System.Text;
using static vbSparkle.VBScriptParser;

namespace vbSparkle
{
    public class VbForNextStatement
        : VbStatement<ForNextStmtContext>
    {
        public string Type { get; set; }
        public VbSimpleStackBlock CodeBlock { get; set; }
        public List<VBValueStatement> Values { get; set; }
        public string Identifier { get; set; }

        public VB_ICS_S_VariableOrProcedureCallStatement CallStatement { get; set; }


        public VbForNextStatement(IVBScopeObject context, ForNextStmtContext bloc)
            : base(context, bloc)
        {
            Type = bloc.asTypeClause()?.type()?.GetText();
            CodeBlock = new VbSimpleStackBlock(context, bloc.block());
            CallStatement = new VB_ICS_S_VariableOrProcedureCallStatement(context, bloc.iCS_S_VariableOrProcedureCall());

            Values = bloc.valueStmt().Select(v => VBValueStatement.Get(context, v)).ToList() ;

        }

        public override DExpression Prettify(bool partialEvaluation = false)
        {
            StringBuilder retCode = new StringBuilder();
            string var = CallStatement.Exp(partialEvaluation);

            retCode.Append($"For {var} = {Values[0].Exp(partialEvaluation)} To {Values[1].Exp(partialEvaluation)}");

            if (Values.Count == 3)
                retCode.AppendLine($" Step {Values[2].Exp(partialEvaluation)}");
            else
                retCode.AppendLine();

            retCode.AppendLine(Helpers.IndentLines(4, CodeBlock.Exp(partialEvaluation)));

            retCode.Append($"Next {var}");


            return new DCodeBlock(retCode.ToString());
        }
    }
}