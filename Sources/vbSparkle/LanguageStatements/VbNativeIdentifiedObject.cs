namespace vbSparkle
{
    public abstract class VbNativeIdentifiedObject : VbIdentifiedObject
    {
        public VbNativeIdentifiedObject(
            IVBScopeObject context, 
            string identifier) 
            : base(context, identifier)
        {
        }

        public override abstract DExpression Prettify(bool partialEvaluation = false);
    }
}
