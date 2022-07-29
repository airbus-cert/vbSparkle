using Antlr4.Runtime.Tree;
using System.Collections.Generic;
using System.Text;

namespace vbSparkle
{
    public class VBMain : VbIdentifiedObject
    {
        public VbCodeBlockContainer CodeBlockContainer { get; set; }

        public VBMain(
            IVBScopeObject context,
            string identifier)
            : base(
                context,
                identifier)
        {
            CodeBlockContainer = new VbCodeBlockContainer(context);
        }


        internal void AppendBlock(VBScriptParser.ModuleBlockContext @object)
        {
            CodeBlockContainer.AppendBlock(@object.block());
        }

        internal void AppendBlock(VBScriptParser.MacroIfThenElseStmtContext @object)
        {
            CodeBlockContainer.AppendBlock(@object);
        }

        public override DExpression Prettify(bool partialEvaluation = false)
        {
            return CodeBlockContainer.Prettify(partialEvaluation);
        }

    }
}