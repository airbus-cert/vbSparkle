using vbSparkle.EvaluationObjects;
using vbSparkle.NativeMethods;
using static vbSparkle.VBScriptParser;

namespace vbSparkle
{
    public class VbIfConditionStmtContext
        : VbStatement<IfConditionStmtContext>
    {
        public VBValueStatement Value { get; set; }

        public VbIfConditionStmtContext(IVBScopeObject context, IfConditionStmtContext bloc)
            : base(context, bloc)
        {
            Value = VBValueStatement.Get(context, bloc.valueStmt());
        }

        public override DExpression Prettify(bool partialEvaluation = false)
        {
            bool input;
            if (Converter.TryGetBool(Value.Prettify(partialEvaluation), out input))
                return new DBoolExpression(input);
            

            return Value.Prettify(partialEvaluation);
        }

        public bool? IsExecuted()
        {
            bool input;
            if (Converter.TryGetBool(Value.Prettify(true), out input))
                return input;
            return null;
        }
    }
}