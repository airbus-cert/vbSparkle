using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;
using System.Text;

namespace vbSparkle
{
    public partial class VBPreprocessorsParser
    {
        public partial class LiteralContext : ILiteralContext
        {

        }

        public partial class DelimitedLiteralContext : ILiteralContext
        {

        }

        public interface ILiteralContext : IRuleNode, IParseTree, ISyntaxTree, ITree
        {
        }
    }
}
