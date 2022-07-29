using static vbSparkle.VBScriptParser;

namespace vbSparkle
{
    public class VBVsLiteralContext
        : VBValueStatement<VsLiteralContext>
    {
        private VBLiteral Literal { get; set; }

        public VBVsLiteralContext(IVBScopeObject context, VsLiteralContext @object)
            : base(context, @object)
        {
            LiteralContext litContext = Object.literal();

            if (litContext is LtDelimitedContext)
            {
                var nContext = litContext as LtDelimitedContext;
                DelimitedLiteralContext litContext2 = nContext.delimitedLiteral();
                if (litContext2 is LtStringContext)
                    Literal = new VbLtString(litContext2 as LtStringContext);

                if (litContext2 is LtColorContext)
                    Literal = new VbLtColor(litContext2 as LtColorContext);

                if (litContext2 is LtOctalContext)
                    Literal = new VbLtOctal(litContext2 as LtOctalContext);

                if (litContext2 is LtDateContext)
                    Literal = new VbLtDateTime(litContext2 as LtDateContext);

            }

            if (litContext is LtIntegerContext)
                Literal = new VbLtInteger(litContext as LtIntegerContext);
            
            if (litContext is LtFilenumberContext)
                Literal = new VbLtFileNumber(litContext as LtFilenumberContext);

            if (litContext is LtDoubleContext)
                Literal = new VbLtDouble(litContext as LtDoubleContext);

            if (litContext is LtBooleanContext)
                Literal = new VbLtBoolean(litContext as LtBooleanContext);

            if (litContext is LtNothingContext)
                Literal = new VbLtNothing(litContext as LtNothingContext);

            if (litContext is LtNullContext)
                Literal = new VbLtNull(litContext as LtNullContext);
        }

        private void AssignDefault<T>(LiteralContext litContext) 
            where T: LiteralContext
        {
            if (litContext is T)
                Literal = new VBLiteral<T>(litContext as T);
        }

        public override DExpression Prettify(bool partialEvaluation = false)
        {
            if (partialEvaluation)
                return Evaluate();
            else
                return new DCodeBlock(Literal.Prettify());
        }

        public override DExpression Evaluate()
        {
            return Literal.Value;
        }
    }

}