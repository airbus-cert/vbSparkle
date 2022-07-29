using System;
using static vbSparkle.VBPreprocessorsParser;

namespace vbSparkle.PreProcessor.Statements
{
    public abstract class VBMacroValueStatement
    {
        public IVBScopeObject Context { get; set; }

        public static VBMacroValueStatement Get(
            IVBScopeObject context,
            ValueStmtContext @object)
        {
            if (@object is VsDualOperationContext)
                return new VBVsDualOperation(context,
                    (VsDualOperationContext)@object);

            if (@object is VsUnaryOperationContext)
                return new VBVsUnaryOperation(context,
                    (VsUnaryOperationContext)@object);

            if (@object is VsLiteralContext)
                return new VBVsLiteralContext(context,
                    (VsLiteralContext)@object);

            if (@object is VsStructContext)
                return new VBVsStructContext(context,
                    (VsStructContext)@object);


            if (@object is VsConstantContext)
                return new VBVsConstContext(context,
                    (VsConstantContext)@object);


            throw new NotSupportedException("VBMacroValueStatement");
        }

        public abstract DExpression Prettify(bool partialEvaluation = false);
        public abstract DExpression Evaluate();

        public string Exp(bool partialEvaluation = false)
        {
            return Prettify(partialEvaluation)?.ToExpressionString();
        }
    }
}
