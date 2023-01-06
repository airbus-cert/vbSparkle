
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static vbSparkle.VBPreprocessorsParser;

namespace vbSparkle
{
    public class VbPreprocessorAnalyser
    {
        public EvaluatorOptions Options { get; internal set; }

        public VbPreprocessorAnalyser(EvaluatorOptions options) { 
            Options = options;
        }

        private PreProcessor.PPNativeObjectManager context { get; set; } = new PreProcessor.PPNativeObjectManager();

        internal string Visit(StartRuleContext stContext)
        {

            var codeBlocks = stContext.codeBlock();

            string result = ParseCodeBlocks(codeBlocks);

            return result;
        }

        private string ParseCodeBlocks(CodeBlockContext[] codeBlocks)
        {
            StringBuilder cleanCode = new StringBuilder();

            foreach (var block in codeBlocks)
            {
                var labels = block.lineLabel();

                foreach (var label in labels)
                    cleanCode.AppendLine(label.GetText());


                var blockBody = block.codeBlockBody();

                if (blockBody is VmacroIfContext)
                {
                    string code = PreProcBranchIfContext(blockBody as VmacroIfContext);
                    cleanCode.AppendLine(code);
                    cleanCode.AppendLine();
                }

                if (blockBody is VmacroConstContext)
                {
                    cleanCode.AppendLine("' " + PreProcDefineConst(blockBody as VmacroConstContext));
                }

                if (blockBody is VcommentBlockContext)
                {
                    cleanCode.AppendLine(blockBody.GetText());
                }

                if (blockBody is VcodeBlockContext)
                {
                    cleanCode.AppendLine(blockBody.GetText());
                }

                if (blockBody is VlineLabelContext)
                {
                    cleanCode.AppendLine(blockBody.GetText());
                }

                var comment = block.commentBlock();

                if (comment != null)
                    cleanCode.AppendLine(comment.GetText());
            }

            return cleanCode.ToString();
        }

        private string PreProcBranchIfContext(VmacroIfContext vmacroIfContext)
        {
            StringBuilder ret = new StringBuilder();

            var stmt = vmacroIfContext.macroIfThenElseStmt();
            var ifBlock = stmt.macroIfBlockStmt();
            var elseIfBlocks = stmt.macroElseIfBlockStmt();
            var elseBlock = stmt.macroElseBlockStmt();

            var ifCondStmt = ifBlock.macroIfBlockCondStmt();
            var ifValueStmt = ifCondStmt.ifConditionStmt().valueStmt();

            bool ifOk = PreProcEvalCond(ifValueStmt);
            bool alreadyBranched = ifOk;

            ret.AppendLine($"'#If ({ifValueStmt.GetText()}) Then ' Evaluated to '{ifOk}'");
            PreProcWriteBranch(ret, ifOk, ifBlock.codeBlock());
            PreProcWriteComments(ret, ifBlock.commentBlock());

            if (elseIfBlocks != null)
                foreach (var elif in elseIfBlocks)
                {
                    var elifValueStmt = elif.ifConditionStmt().valueStmt();
                    ifOk = !alreadyBranched && PreProcEvalCond(elifValueStmt);
                    ret.AppendLine($"'#ElseIf ({elifValueStmt.GetText()}) Then ' Evaluated to '{ifOk}'");
                    PreProcWriteBranch(ret, ifOk, elif.codeBlock());
                    PreProcWriteComments(ret, elif.commentBlock());
                }

            if (elseBlock != null)
            {
                ifOk = !alreadyBranched;
                ret.AppendLine($"'#Else ' Evaluated to '{ifOk}'");
                PreProcWriteBranch(ret, ifOk, elseBlock.codeBlock());
                PreProcWriteComments(ret, elseBlock.commentBlock());
            }


            ret.AppendLine($"'#End If");

            return ret.ToString();

        }

        private void PreProcWriteComments(StringBuilder ret, CommentBlockContext[] comments)
        {
            foreach (var c in comments)
                ret.AppendLine(c.GetText());
        }

        private void PreProcWriteBranch(StringBuilder ret, bool isExecutedBranch, CodeBlockContext[] codeBlocks)
        {
            if (isExecutedBranch)
            {
                ret.AppendLine(ParseCodeBlocks(codeBlocks));
                return;
            }

            var prefix = isExecutedBranch ? "" : "' ";

            foreach (var v in codeBlocks)
            {
                var lineLabels = v.lineLabel();
                if (lineLabels != null)
                    foreach (var label in lineLabels)
                        ret.AppendLine(prefix + label.GetText().ToUpperInvariant());

                ret.AppendLine(prefix + v.codeBlockBody().GetText());

                foreach (var n in v.NEWLINE())
                    ret.AppendLine();
            }
        }

        private bool PreProcEvalCond(ValueStmtContext valueStmtContext)
        {
            var ValueStatement = PreProcessor.Statements.VBMacroValueStatement.Get(context, valueStmtContext);
            var valueExp = ValueStatement?.Evaluate()?.ToValueString();

            if (valueExp == null)
                return false;

            switch (valueExp.ToUpper())
            {
                case "TRUE":
                    return true;
                case "FALSE":
                    return false;
                case "0":
                    return false;
                case "":
                    return false;
            }

            return true;
        }

        private string PreProcDefineConst(VmacroConstContext macroConst)
        {
            var macroConstStmt = macroConst.macroConst();

            var identifier = macroConstStmt.IDENTIFIER().GetText();
            var valueStmt = macroConstStmt.valueStmt();

            var ValueStatement = PreProcessor.Statements.VBMacroValueStatement.Get(context, valueStmt);
            var valueExp = ValueStatement.Evaluate();

            context.SetVarValue(identifier, valueExp);

            return $"#Const {identifier}={valueStmt.GetText()} => Evaluated to: {valueExp.ToValueString()}";
        }
    }
}