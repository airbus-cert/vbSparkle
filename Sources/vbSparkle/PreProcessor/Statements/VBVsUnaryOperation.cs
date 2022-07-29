using static vbSparkle.VBPreprocessorsParser;

namespace vbSparkle.PreProcessor.Statements
{
    public class VBVsUnaryOperation
       : VBValueStatement<VsUnaryOperationContext>
    {
        public string Operator { get; set; }
        public VBMacroValueStatement ValueStatement { get; set; }

        public VBVsUnaryOperation(IVBScopeObject context, VsUnaryOperationContext @object)
            : base(context, @object)
        {
            Operator = @object.@operator.Text;
            ValueStatement = Get(context, @object.valUnary);
        }


        public override DExpression Prettify(bool partialEvaluation = false)
        {
            if (partialEvaluation)
                try
                {
                    return Evaluate();
                }
                catch { }

            return new DCodeBlock($"{Operator}{ValueStatement.Exp(partialEvaluation)}");
        }

        public override DExpression Evaluate()
        {
            try
            {
                var left = new DMathExpression<int>(0);
                var right = ValueStatement.Evaluate();

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
