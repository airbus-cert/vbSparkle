using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vbSparkle
{
    public enum BlockSpanType
    {
        None,
        SimpleLine,
        Block,
        LineLabel
    }

    public class VbSimpleStackBlock : VbCodeBlock<VBScriptParser.BlockContext>
    {
        public List<VbCodeBlock> Statements = new List<VbCodeBlock>();

        public VbSimpleStackBlock(
            IVBScopeObject context,
            VBScriptParser.BlockContext @object)
            : base(context, @object)
        {
            if (@object == null)
                return;

            foreach (var blockSwitch in @object.blockSwitch())
            {
                foreach (var bloc in blockSwitch.children)
                {
                    if (bloc is VBScriptParser.BlockStmtContext)
                    {
                        Statements.Add(new VbSimpleBlock(context, bloc as VBScriptParser.BlockStmtContext));
                    }
                    else if (bloc is VBScriptParser.InlineBlockContext)
                    {
                        Statements.Add(new VbInlineSimpleStackBlock(context, bloc as VBScriptParser.InlineBlockContext));
                    }
                    else if (bloc is VBScriptParser.LineLabelContext)
                    {
                        Statements.Add(new VbCodeBlock<VBScriptParser.LineLabelContext>(context, bloc as VBScriptParser.LineLabelContext));
                    }
                }
            }
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override DExpression Prettify(bool partialEvaluation = false)
        {
            StringBuilder result = new StringBuilder();

            BlockContextType? prev = BlockContextType.None;

            for (int i = 0; i < Statements.Count; i++)
            {
                var st = Statements[i];

                var contextType = (st as VbSimpleBlock)?.NewBlockContext;

                if (contextType == null)
                    contextType = (st as VbInlineSimpleStackBlock)?.LastBlockContext;

                if (contextType == null)
                    contextType = BlockContextType.Label;

                bool addLine = i > 0 && (contextType != prev || contextType == BlockContextType.Block);

                if (addLine)
                    result.AppendLine();

                result.Append(st.Exp(partialEvaluation));

                if (i < Statements.Count - 1)
                    result.AppendLine();

                prev = contextType;
            }

            return new DCodeBlock(result.ToString());
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}