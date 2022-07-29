using static vbSparkle.VBPreprocessorsParser;

namespace vbSparkle.PreProcessor.Statements
{
    public abstract class VBValueStatement<T>
        : VBMacroValueStatement
        where T : ValueStmtContext
    {
        public T Object { get; set; }

        public VBValueStatement(IVBScopeObject context, T @object)
        {
            Context = context;
            Object = @object;
        }

        public override DExpression Prettify(bool partialEvaluation = false)
        {
            return new DCodeBlock(Object.GetText());
        }
    }
}
