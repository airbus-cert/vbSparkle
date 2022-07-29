namespace vbSparkle
{
    public class VBArgCall : VbStatement<VBScriptParser.ArgCallContext>
    {
        public bool ByRef { get; set; } = false;
        public bool ByVal { get; set; } = false;
        public bool HasParamArray { get; set; } = false;
        public VBValueStatement ValueStatement { get; set; }

        public VBArgCall(
            IVBScopeObject context,
            VBScriptParser.ArgCallContext bloc)
            : base(context, bloc)
        {
            ByRef = !string.IsNullOrWhiteSpace(bloc.BYREF()?.GetText());
            ByVal = !string.IsNullOrWhiteSpace(bloc.BYVAL()?.GetText());
            HasParamArray = !string.IsNullOrWhiteSpace(bloc.PARAMARRAY()?.GetText());

            ValueStatement = VBValueStatement.Get(Context, bloc.valueStmt());
        }

        public override DExpression Prettify(bool partialEvaluation = false)
        {
            if (partialEvaluation && !ByRef && !HasParamArray)
            {
                return ValueStatement.Evaluate();
            }
            else
            {
                string pValue = ValueStatement.Exp(partialEvaluation);

                if (ByRef)
                    return new DCodeBlock($"ByRef {pValue}");

                if (ByVal)
                    return new DCodeBlock($"ByVal {pValue}");

                if (HasParamArray)
                    return new DCodeBlock($"ParamArray {pValue}");

                return new DCodeBlock($"{pValue}");
            }

        }
    }
    
}
