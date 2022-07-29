using System.Linq;

namespace vbSparkle
{
    public class VbOutputListExprStatement : VbStatement<VBScriptParser.OutputList_ExpressionContext>
    {
        public VBValueStatement Value { get; }
        public VBArgCall[] VBArgCall { get; }
        public bool IsSPC { get; }
        public bool IsTAB { get; }

        public VbOutputListExprStatement(IVBScopeObject context, VBScriptParser.OutputList_ExpressionContext bloc)
            : base(context, bloc)
        {
            //OPEN WS valueStmt WS FOR WS (APPEND | BINARY | INPUT | OUTPUT | RANDOM) (WS ACCESS WS (READ | WRITE | READ_WRITE))? (WS (SHARED | LOCK_READ | LOCK_WRITE | LOCK_READ_WRITE))? WS AS WS valueStmt (WS LEN WS? EQ WS? valueStmt)?
            if (bloc.valueStmt() != null)
                Value = VBValueStatement.Get(context, bloc.valueStmt());

            if (bloc.argsCall() != null)
                VBArgCall = bloc.argsCall().argCall().Select(q => new VBArgCall(context, q)).ToArray();

            IsSPC = bloc.SPC() != null;
            IsTAB = bloc.TAB() != null;

        }

        public override DExpression Prettify(bool partialEvaluation = false)
        {
            string ret = string.Empty; 
            string args = string.Empty;
            if (VBArgCall != null)
                args = string.Join(", ", VBArgCall.Select(q => q.Exp(partialEvaluation)));

            if (IsSPC)
                ret = $"Spc({args})";
            if (IsTAB)
                ret = $"Tab({args})";

            if (Value != null)
            {
                ret = Value.Exp(partialEvaluation);
            }

            return new DCodeBlock(ret);
        }
    }
}