using System.Linq;

namespace vbSparkle
{
    public class VbPrintStatement : VbStatement<VBScriptParser.PrintStmtContext>
    {
        public VBValueStatement FileNumber { get; }
        public VbOutputListExprStatement[] OutputList { get; }

        public VbPrintStatement(IVBScopeObject context, VBScriptParser.PrintStmtContext bloc)
            : base(context, bloc)
        {
            FileNumber = VBValueStatement.Get(context, bloc.valueStmt());

            OutputList = bloc.outputList()?.outputList_Expression()?.Select(v => new VbOutputListExprStatement(context, v)).ToArray();
            
        }

        public override DExpression Prettify(bool partialEvaluation = false)
        {
            string args = "";
            if (OutputList != null)
                args = string.Join(", ", OutputList.Select(v => v.Exp(partialEvaluation)));

            return new DCodeBlock($"Print {FileNumber.Exp(partialEvaluation)}, {args}");
        }
    }
}