using Antlr4.Runtime.Tree;

namespace vbSparkle
{
    public abstract class VbCodeBlock
    {
        public IVBScopeObject Context { get; set; }

        public VbCodeBlock(IVBScopeObject context)
        {
            Context = context;
        }

        public abstract DExpression Prettify(bool partialEvaluation = false);

        public string Exp(bool partialEvaluation = false)
        {
            return Prettify(partialEvaluation).ToExpressionString();
        }
    }

    public class VbCodeBlock<T> : VbCodeBlock
        where T: IParseTree
    {
        public T Object { get; set; }

        public VbCodeBlock(IVBScopeObject context, T @object) 
            : base(context)
        {
            Object = @object;
        }


        public override DExpression Prettify(bool partialEvaluation = false)
        {
            return new DCodeBlock(Object?.GetText().Trim("\r\n\t ".ToCharArray()));
        }
    }
}