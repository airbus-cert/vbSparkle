using Antlr4.Runtime.Tree;

namespace vbSparkle
{
    public class VbSimpleStatement : VbStatement<IParseTree>
    {
        public string Keyword { get; set; }

        public VbSimpleStatement(IVBScopeObject context, IParseTree bloc, string keyword)
            : base(context, bloc)
        {
            Keyword = keyword;
        }

        public override DExpression Prettify(bool partialEvaluation = false)
        {
            return new DCodeBlock(Keyword);
        }
    }
}