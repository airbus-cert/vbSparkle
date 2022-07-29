using static vbSparkle.VBScriptParser;

namespace vbSparkle
{
    public class VBVsNewContext
        : VBValueStatement<VsNewContext>
    {
        //TODO:Replace by identifier
        public TypeContext Type { get; set; }

        public VBVsNewContext(IVBScopeObject context, VsNewContext @object)
            : base(context, @object)
        {
            Type = @object.type();
        }

        public override DExpression Prettify(bool partialEvaluation = false)
        {
            if (partialEvaluation)
                try
                {
                    return Evaluate();
                }
                catch { }

            return new DCodeBlock($"!New {Type.GetText()}");
        }

        public override DExpression Evaluate()
        {
            return Prettify(false);
        }
    }

}