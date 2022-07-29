using Antlr4.Runtime.Tree;

namespace vbSparkle
{
    public class VbUserIdentifiedObject<T> : VbIdentifiedObject
        where T: IParseTree
    {
        public T Object { get; set; }

        public VbUserIdentifiedObject(
            IVBScopeObject context, 
            T @object, 
            string identifier):
            base(context, identifier)
        {
            Context = context;
            Object = @object;
        }
        
        public override DExpression Prettify(bool partialEvaluation = false)
        {
            return new DCodeBlock(Object?.GetText().Trim());
        }

        public string Exp(bool partialEvaluation)
        {
            return Prettify(partialEvaluation).ToExpressionString();
        }
    }
}
