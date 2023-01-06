using System.Collections.Generic;
using System.Text;
using static vbSparkle.VBScriptParser;

namespace vbSparkle
{
    public partial class VbIfThenElseStatement 
        : VbStatement<IfThenElseStmtContext>
    {
        public List<VbStatement> IfBlocks { get; set; } = new List<VbStatement>();

        public VbIfThenElseStatement(IVBScopeObject context, IfThenElseStmtContext bloc)
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

            bool hasExecutedBlock = false;
            bool hasAmbigeousBlock = false;
            bool isFirstBlock = true;
            

            string prefixRem = (Context.Options.JunkCodeProcessingMode == JunkCodeProcessingMode.Comment) ? "'" : string.Empty;
            int indentLevel = Context.Options.JunkCodeProcessingMode == JunkCodeProcessingMode.Remove ?
                0:
                Context.Options.IndentSpacing;

            foreach (var v in IfBlocks)
            {
                int diffCode = retCode.Length;
                var ifBl = v as IVbIfBlockStmt;
                if (ifBl != null)
                {
                    var condValue = ifBl.CondValue;
                    bool? isBlockExecuted = condValue.IsExecuted();
                    if (isBlockExecuted.HasValue)
                    {
                        if (!hasExecutedBlock && isBlockExecuted.Value)
                        {
                            hasExecutedBlock = true;
                            // block if executé
                            
                            if (hasAmbigeousBlock)
                            {
                                //if (Context.Options.JunkCodeProcessingMode != JunkCodeProcessingMode.Remove)
                                retCode.AppendLine($"ElseIf {condValue.Exp(partialEvaluation)} Then");

                                retCode.Append(Helpers.IndentLines(indentLevel, v.Exp(partialEvaluation)));
                            }
                            else
                            {
                                if (Context.Options.JunkCodeProcessingMode != JunkCodeProcessingMode.Remove)
                                    retCode.AppendLine($"{prefixRem}If {condValue.Exp(partialEvaluation)} Then");

                                retCode.Append(Helpers.IndentLines(indentLevel, v.Exp(partialEvaluation)));
                            }
                        }
                        else
                        {
                            if (Context.Options.JunkCodeProcessingMode != JunkCodeProcessingMode.Remove)
                            {
                                // block if commenté
                                if (hasAmbigeousBlock || !isFirstBlock)
                                {
                                    retCode.AppendLine($"'ElseIf {condValue.Exp(partialEvaluation)} Then");
                                    retCode.Append(Helpers.CommentLines(Context.Options.IndentSpacing, v.Exp(false), prefixRem));
                                }
                                else
                                {
                                    retCode.AppendLine($"'If {condValue.Exp(partialEvaluation)} Then");
                                    retCode.Append(Helpers.CommentLines(Context.Options.IndentSpacing, v.Exp(false), prefixRem));
                                }
                            }
                        }
                    } 
                    else
                    {
                        if (hasExecutedBlock)
                        {
                            if (Context.Options.JunkCodeProcessingMode != JunkCodeProcessingMode.Remove)
                            {
                                retCode.AppendLine($"'ElseIf {condValue.Exp(partialEvaluation)} Then");
                                retCode.Append(Helpers.CommentLines(Context.Options.IndentSpacing, v.Exp(partialEvaluation), prefixRem));
                            }
                        }
                        else
                        {
                            // block if non commenté
                            if (hasAmbigeousBlock)
                            {
                                retCode.AppendLine($"ElseIf {condValue.Exp(partialEvaluation)} Then");
                                retCode.Append(Helpers.IndentLines(Context.Options.IndentSpacing, v.Exp(partialEvaluation)));
                            }
                            else
                            {
                                retCode.AppendLine($"If {condValue.Exp(partialEvaluation)} Then");
                                retCode.Append(Helpers.IndentLines(Context.Options.IndentSpacing, v.Exp(partialEvaluation)));
                            }
                            hasAmbigeousBlock = true;
                        }
                    }
                } 
                else
                {
                    // Block "else"
                    if (hasExecutedBlock)
                    {
                        if (Context.Options.JunkCodeProcessingMode != JunkCodeProcessingMode.Remove)
                        {
                            // block else commenté
                            retCode.AppendLine($"{prefixRem}Else");
                            retCode.Append(Helpers.CommentLines(Context.Options.IndentSpacing, v.Exp(false), prefixRem));
                        }
                    }
                    else if (hasAmbigeousBlock)
                    {
                        // block else non commenté
                        retCode.AppendLine($"Else");
                        retCode.Append(Helpers.IndentLines(Context.Options.IndentSpacing, v.Exp(partialEvaluation)));
                    }
                    else
                    {
                        // block else executé
                        if (Context.Options.JunkCodeProcessingMode != JunkCodeProcessingMode.Remove)
                            retCode.AppendLine($"{prefixRem}Else");

                        retCode.Append(Helpers.IndentLines(indentLevel, v.Exp(partialEvaluation)));
                    }

                }

                isFirstBlock = false;

                if (diffCode < retCode.Length)
                    retCode.AppendLine();
                //retCode.AppendLine(v.Exp(partialEvaluation));
            }

            
            if (hasAmbigeousBlock)
                retCode.Append("End If");
            else if (Context.Options.JunkCodeProcessingMode != JunkCodeProcessingMode.Remove)
                retCode.Append($"{prefixRem}End If");


            return new DCodeBlock(retCode.ToString());
        }
    }
}