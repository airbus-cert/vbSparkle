using System.Collections.Generic;
using System.Text;

namespace vbSparkle
{

    public class VbCodeBlockContainer
    {
        private IVBScopeObject context;
        
        public List<VbCodeBlock> codeBlocks = new List<VbCodeBlock>();
        
        public VbCodeBlockContainer(IVBScopeObject context)
        {
            this.context = context;
        }

        internal void AppendBlock(VBScriptParser.BlockContext @object)
        {
            codeBlocks.Add(new VbSimpleStackBlock(context, @object));
        }

        internal void AppendBlock(VBScriptParser.MacroIfThenElseStmtContext @object)
        {
            codeBlocks.Add(new VbCodeBlock<VBScriptParser.MacroIfThenElseStmtContext>(context, @object));
        }

        internal DExpression Prettify(bool partialEvaluation = false)
        {
            StringBuilder result = new StringBuilder();

            foreach (var codeBlock in codeBlocks)
            {
                result.AppendLine(codeBlock.Exp(partialEvaluation));
            }

            return new DCodeBlock(result.ToString());
        }
    }
}