using static vbSparkle.VBPreprocessorsParser;

namespace vbSparkle.PreProcessor.Statements
{
    public class VBVsDualOperation
    : VBValueStatement<VsDualOperationContext>
    {
        public string Operator { get; set; }
        public VBMacroValueStatement Left { get; set; }
        public VBMacroValueStatement Right { get; set; }

        public VBVsDualOperation(IVBScopeObject context, VsDualOperationContext @object)
            : base(context, @object)
        {
            Operator = @object.@operator.Text;
            Left = Get(context, @object.left);
            Right = Get(context, @object.right);
        }

        public override DExpression Prettify(bool partialEvaluation = false)
        {
            if (partialEvaluation)
                try
                {
                    return Evaluate();
                }
                catch { }


            return new DCodeBlock($"{Left.Exp(partialEvaluation)} {Operator} {Right.Exp(partialEvaluation)}");
        }

        public override DExpression Evaluate()
        {
            try
            {
                var left = Left.Evaluate();
                var right = Right.Evaluate();

                var op = Operator;

                DExpression result = Operation.DoOperation(op, left, right);

                return result;
            }
            catch
            {
                return Prettify(false);
            }
        }
    }
}
