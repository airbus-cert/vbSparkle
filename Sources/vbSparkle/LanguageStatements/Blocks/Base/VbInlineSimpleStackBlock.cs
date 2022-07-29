using System.Collections.Generic;
using System.Text;

namespace vbSparkle
{
    public class VbInlineSimpleStackBlock : VbCodeBlock<VBScriptParser.InlineBlockContext>
    {
        public List<VbInlineBlock> Statements = new List<VbInlineBlock>();
        public BlockContextType LastBlockContext { get; set; } = BlockContextType.None;

        public VbInlineSimpleStackBlock(
            IVBScopeObject context,
            VBScriptParser.InlineBlockContext @object)
            : base(context, @object)
        {
            if (@object == null)
                return;

            foreach (VBScriptParser.InlineBlockStmtContext bloc in @object.inlineBlockStmt())
            {
                var st = new VbInlineBlock(context, bloc);
                Statements.Add(st);

                LastBlockContext = st.NewBlockContext;
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

            BlockContextType prev = BlockContextType.None;

            for (int i = 0; i < Statements.Count; i++)
            {
                var st = Statements[i];

                var contextType = st.NewBlockContext;

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