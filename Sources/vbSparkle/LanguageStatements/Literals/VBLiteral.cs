namespace vbSparkle
{
    public abstract class VBLiteral
    {
        public DExpression Value { get; set; }

        public abstract string Prettify();
    }

    public class VBLiteral<T> : VBLiteral
        where T : ILiteralContext
    {
        public T Object { get; set; }

        public VBLiteral(T @object)
        {
            Object = @object;
            Value = new DCodeBlock(@object?.GetText());
        }

        public override string Prettify()
        {
            return Object.GetText();
        }


    }

}