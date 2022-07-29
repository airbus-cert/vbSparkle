namespace vbSparkle
{
    public class VbUserArg : VbUserIdentifiedObject<VBScriptParser.ArgContext>
    {
        public bool ByRef { get; set; } = false;
        public bool ByVal { get; set; } = false;
        public bool HasParamArray { get; set; } = false;
        public VBValueStatement ValueStatement { get; set; }
        //public VBValueStatement DefaultValueStatement { get; set; }

        public VbUserArg(
            IVBScopeObject context, 
            VBScriptParser.ArgContext @object, 
            string identifier) 
            : base(context, @object, identifier)
        {
            ByRef = !string.IsNullOrWhiteSpace(@object.BYREF()?.GetText());
            ByVal = !string.IsNullOrWhiteSpace(@object.BYVAL()?.GetText());
            HasParamArray = !string.IsNullOrWhiteSpace(@object.PARAMARRAY()?.GetText());

            var val = @object?.argDefaultValue()?.valueStmt();

            if (val != null)
            {
                ValueStatement = VBValueStatement.Get(context, val);
                //CurrentValue = 0; //ValueStatement.Evaluate();
            }

            //ValueStatement = DefaultValueStatement
        }

        public DMathExpression CurrentValue { get; set; }
    }

}