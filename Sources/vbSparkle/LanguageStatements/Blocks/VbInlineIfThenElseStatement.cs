using System.Collections.Generic;
using System.Text;
using static vbSparkle.VBScriptParser;

namespace vbSparkle
{
    public class VbInlineIfThenElseStatement
        : VbStatement<InlineIfThenElseStmtContext>
    {
        public List<VbStatement> IfBlocks { get; set; } = new List<VbStatement>();

        public VbInlineIfThenElseStatement(IVBScopeObject context, InlineIfThenElseStmtContext bloc)
            : base(context, bloc)
        {
            foreach (var v in bloc.children)
            {
                if (v is InlineIfBlockStmtContext)
                {

                    var o1 = new VbInlineIfBlockStmtContext(context, v as InlineIfBlockStmtContext);
                    IfBlocks.Add(o1);
                }
                else if (v is InlineElseBlockStmtContext)
                {
                    var o1 = new VbInlineIfElseBlockStmtContext(context, v as InlineElseBlockStmtContext);
                    IfBlocks.Add(o1);
                }
                else if (v is IfBlockStmtContext)
                {
                    var o1 = new VbIfBlockStmtContext(context, v as IfBlockStmtContext);
                    IfBlocks.Add(o1);
                }
                else if (v is IfElseIfBlockStmtContext)
                {
                    var o2 = new VbIfElseIfBlockStmtContext(context, v as IfElseIfBlockStmtContext);
                    IfBlocks.Add(o2);
                }
                else if (v is IfElseBlockStmtContext)
                {
                    var o3 = new VbIfElseBlockStmtContext(context, v as IfElseBlockStmtContext);
                    IfBlocks.Add(o3);
                }
            }

        }

        public override DExpression Prettify(bool partialEvaluation = false)
        {
            StringBuilder retCode = new StringBuilder();

            foreach (var v in IfBlocks)
            {
                retCode.AppendLine(v.Exp(partialEvaluation));
            }

            retCode.Append("End If");


            return new DCodeBlock(retCode.ToString());
        }
    }
}