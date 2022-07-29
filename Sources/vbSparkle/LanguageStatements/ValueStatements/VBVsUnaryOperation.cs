using static vbSparkle.VBScriptParser;

namespace vbSparkle
{
    public class VBVsUnaryOperation
        : VBValueStatement<VsUnaryOperationContext>
    {
        public string Operator { get; set; }
        public VBValueStatement ValueStatement { get; set; }

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
                DMathExpression left = null;
                var right = ValueStatement.Evaluate();

                var op = Operator;

                DExpression result = Operation.DoOperation(op, left, right, unaryOperation: true);

                return result;
            } 
            catch
            {
                return Prettify(false);
            }
        }
    }

}