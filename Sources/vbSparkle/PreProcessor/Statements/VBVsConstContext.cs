using static vbSparkle.VBPreprocessorsParser;

namespace vbSparkle.PreProcessor.Statements
{
    public class VBVsConstContext
      : VBValueStatement<VsConstantContext>
    {
        public VsConstantContext constObject { get; private set; }

        public VBVsConstContext(IVBScopeObject context, VsConstantContext @object)
            : base(context, @object)
        {
            constObject = @object ;
        }

        public override DExpression Prettify(bool partialEvaluation = false)
        {
            return null;
        }

        public override DExpression Evaluate()
        {
            string identifier = constObject.ambiguousIdentifier().GetText();

            DExpression valueObject = (Context.GetIdentifiedObject(identifier) as VbNativeConstants)?.Value;
            return valueObject;
        }
    }
}
