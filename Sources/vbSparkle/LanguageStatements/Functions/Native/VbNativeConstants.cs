namespace vbSparkle
{
    public class VbNativeConstants : VbNativeIdentifiedObject
    {
        public DExpression Value { get; private set; }

        public VbNativeConstants(
            IVBScopeObject context, string identifier, DExpression value)
            : base(context, identifier)
        {
            Value = value;
        }


        public override DExpression Prettify(bool partialEvaluation = false)
        {
            if (partialEvaluation)
            {
                return Value;
            }
            else
            {
                return new DCodeBlock(this.Name);
            }
        }
    }
}
