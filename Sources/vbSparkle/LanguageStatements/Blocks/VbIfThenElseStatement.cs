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

            //bool isFirstNonEvaluatedBlock = false;
            bool hasExecutedBlock = false;
            bool hasAmbigeousBlock = false;
            
            foreach (var v in IfBlocks)
            {
                //if (v is IVbIfBlockStmt)
                //{
                //    IVbIfBlockStmt v2 = (IVbIfBlockStmt)v;
                //    var Exp = v2.CondValue.Value.Evaluate();
                //    if (Exp.IsValuable)
                //    {
                //        var valueString = Exp.ToValueString();
                //        (0).ToString();
                //    }
                //    (0).ToString();
                //}
                var ifBl = v as IVbIfBlockStmt;
                if (ifBl != null)
                {
                    var condValue = ifBl.CondValue;
                    bool? isExecutedBlock = condValue.IsExecuted();
                    if (isExecutedBlock.HasValue)
                    {
                        if (!hasExecutedBlock && isExecutedBlock.Value)
                        {
                            hasExecutedBlock = true;
                            // block if executé
                            
                            if (hasAmbigeousBlock)
                            {
                                retCode.AppendLine($"'ElseIf {condValue.Exp(partialEvaluation)} Then");
                                retCode.Append(Helpers.IndentLines(4, v.Exp(partialEvaluation)));
                            }
                            else
                            {
                                retCode.AppendLine($"'If {condValue.Exp(partialEvaluation)} Then");
                                retCode.Append(Helpers.IndentLines(4, v.Exp(partialEvaluation)));
                            }
                        }
                        else
                        {
                            // block if commenté
                            if (hasAmbigeousBlock)
                            {
                                retCode.AppendLine($"'ElseIf {condValue.Exp(partialEvaluation)} Then");
                                retCode.Append(Helpers.CommentLines(4, v.Exp(false)));
                            } 
                            else
                            {
                                retCode.AppendLine($"'If {condValue.Exp(partialEvaluation)} Then");
                                retCode.Append(Helpers.CommentLines(4, v.Exp(false)));
                            }
                        }
                    } 
                    else
                    {
                        if (hasExecutedBlock)
                        {
                            retCode.AppendLine($"'ElseIf {condValue.Exp(partialEvaluation)} Then");
                            retCode.Append(Helpers.CommentLines(4, v.Exp(partialEvaluation)));
                        }
                        else
                        {
                            // block if non commenté
                            if (hasAmbigeousBlock)
                            {
                                retCode.AppendLine($"ElseIf {condValue.Exp(partialEvaluation)} Then");
                                retCode.Append(Helpers.IndentLines(4, v.Exp(partialEvaluation)));
                            }
                            else
                            {
                                retCode.AppendLine($"If {condValue.Exp(partialEvaluation)} Then");
                                retCode.Append(Helpers.IndentLines(4, v.Exp(partialEvaluation)));
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
                        // block else commenté
                        retCode.AppendLine($"'Else");
                        retCode.Append(Helpers.CommentLines(4, v.Exp(false)));
                    }
                    else if (hasAmbigeousBlock)
                    {
                        // block else non commenté
                        retCode.AppendLine($"Else");
                        retCode.Append(Helpers.IndentLines(4, v.Exp(partialEvaluation)));
                    }
                    else
                    {
                        // block else executé
                        retCode.AppendLine($"'Else");
                        retCode.Append(Helpers.IndentLines(4, v.Exp(partialEvaluation)));
                    }

                }

                retCode.AppendLine();
                //retCode.AppendLine(v.Exp(partialEvaluation));
            }

            
            if (hasAmbigeousBlock)
                retCode.Append("End If");
            else
                retCode.Append("'End If");


            return new DCodeBlock(retCode.ToString());
        }
    }
}