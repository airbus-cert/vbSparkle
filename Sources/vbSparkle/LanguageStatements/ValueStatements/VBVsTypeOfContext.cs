using static vbSparkle.VBScriptParser;

namespace vbSparkle
{
    public class VBVsTypeOfContext
        : VBValueStatement<VsTypeOfContext>
    {
        public VBValueStatement ValueStatement { get; set; }

        //TODO:Replace by identifier
        public TypeContext Type { get; set; }

        public VBVsTypeOfContext(IVBScopeObject context, VsTypeOfContext @object)
            : base(context, @object)
        {
            var to = @object.typeOfStmt();
            
            ValueStatement = Get(context, to.valueStmt());
            Type = to.type();
        }

        public override DExpression Prettify(bool partialEvaluation = false)
        {
            if (partialEvaluation)
                try
                {
                    return Evaluate();
                }
                catch { }
            string pref = $"TypeOf {ValueStatement.Exp(partialEvaluation)}";

            if (Type != null)
                return new DCodeBlock($"{pref} Is {Type.GetText()}");

            return new DCodeBlock(pref);

        }

        public override DExpression Evaluate()
        {
            return Prettify(false);
        }
    }

}