using System;
using static vbSparkle.VBScriptParser;

namespace vbSparkle
{
    public abstract class VBValueStatement
    {
        public IVBScopeObject Context { get; set; }

        public static VBValueStatement Get(
            IVBScopeObject context,
            ValueStmtContext @object)
        {
            if (@object is VsDualOperationContext)
                return new VBVsDualOperation(context,
                    (VsDualOperationContext) @object);

            if (@object is VsUnaryOperationContext)
                return new VBVsUnaryOperation(context,
                    (VsUnaryOperationContext) @object);

            if (@object is VsAddressOfContext)
                return new VBVsAddressOfContext(context,
                    (VsAddressOfContext) @object);

            if (@object is VsAssignContext)
                return new VBVsAssignContext(context,
                    (VsAssignContext) @object);

            if (@object is VsICSContext)
                return new VBVsICSContext(context,
                    (VsICSContext) @object);

            if (@object is VsLiteralContext)
                return new VBVsLiteralContext(context,
                    (VsLiteralContext) @object);

            if (@object is VsStructContext)
                return new VBVsStructContext(context,
                    (VsStructContext) @object);

            if (@object is VsTypeOfContext)
                return new VBVsTypeOfContext(context,
                    (VsTypeOfContext) @object);

            if (@object is VsNewContext)
                return new VBVsNewContext(context,
                    (VsNewContext) @object);

            throw new NotSupportedException("VBValueStatement");
        }

        public abstract DExpression Prettify(bool partialEvaluation = false);
        public abstract DExpression Evaluate();

        public string Exp(bool partialEvaluation = false)
        {
            return Prettify(partialEvaluation)?.ToExpressionString();
        }
    }

    public abstract class VBValueStatement<T>
        : VBValueStatement
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