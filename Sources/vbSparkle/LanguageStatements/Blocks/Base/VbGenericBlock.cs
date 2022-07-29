using Antlr4.Runtime.Tree;
using System;

namespace vbSparkle
{
    public abstract class VbGenericBlock<T> : VbCodeBlock<T> where T: IParseTree
    {
        public BlockContextType NewBlockContext { get; set; }

        public VbStatement Statement { get; set; }

        public VbGenericBlock(
           IVBScopeObject context,
           T @object)
            : base(context, @object)
        {
            NewBlockContext = BlockContextType.SimpleInline;
        }

        public bool AppendGeneric(IParseTree bloc)
        {
            if (bloc == null)
                return false;

            Statement = new VbStatement(Context, bloc);
            return true;
        }

        public bool Append(VBScriptParser.SetStmtContext bloc)
        {
            if (bloc == null)
                return false;

            Statement = new VbSetStatement(Context, bloc);

            return true;
        }

        public bool Append(VBScriptParser.LetStmtContext bloc)
        {
            if (bloc == null)
                return false;

            NewBlockContext = BlockContextType.SimpleInline;
            Statement = new VbLetStatement(Context, bloc);

            return true;
        }


        public bool Append(VBScriptParser.PutStmtContext bloc)
        {
            if (bloc == null)
                return false;

            Statement = new VbPutStatement(Context, bloc);

            return true;
        }

        public bool Append(VBScriptParser.ImplicitCallStmt_InBlockContext bloc)
        {
            if (bloc == null)
                return false;

            NewBlockContext = BlockContextType.InlineCall;

            if (Append(bloc.iCS_B_MemberProcedureCall()))
                return true;

            if (Append(bloc.iCS_B_ProcedureCall()))
                return true;

            return false;
        }

        public bool Append(VBScriptParser.ImplicitCallStmt_InStmtContext bloc)
        {
            if (bloc == null)
                return false;

            NewBlockContext = BlockContextType.InlineCall;

            Statement = new VbInStatement(Context, bloc);

            return true;
        }


        public bool Append(VBScriptParser.ICS_B_MemberProcedureCallContext bloc)
        {
            if (bloc == null)
                return false;

            NewBlockContext = BlockContextType.InlineCall;

            Statement = new VB_ICS_S_MemberProcedureCall(Context, bloc);

            return true;
        }

        public bool Append(VBScriptParser.ICS_B_ProcedureCallContext bloc)
        {
            if (bloc == null)
                return false;

            NewBlockContext = BlockContextType.InlineCall;

            Statement = new VB_ICS_B_ProcedureCall(Context, bloc);

            return true;
        }


        public bool Append(VBScriptParser.RaiseEventStmtContext bloc)
        {
            if (bloc == null)
                return false;

            NewBlockContext = BlockContextType.InlineCall;

            Statement = new VbRaiseEventStatement(Context, bloc);

            return true;
        }

        public bool Append(VBScriptParser.RandomizeStmtContext bloc)
        {
            if (bloc == null)
                return false;

            Statement = new VbRandomizeStatement(Context, bloc);

            return true;
        }

        public bool Append(VBScriptParser.RedimStmtContext bloc)
        {
            if (bloc == null)
                return false;

            NewBlockContext = BlockContextType.SpecialStatement;

            Statement = new VbRedimStatement(Context, bloc);

            return true;
        }

        public bool Append(VBScriptParser.ResetStmtContext bloc)
        {
            if (bloc == null)
                return false;

            Statement = new VbSimpleStatement(Context, bloc, "Reset");

            return true;
        }

        public bool Append(VBScriptParser.ResumeStmtContext bloc)
        {
            if (bloc == null)
                return false;

            NewBlockContext = BlockContextType.SpecialStatement;

            Statement = new VbResumeStatement(Context, bloc);

            return true;
        }

        public bool Append(VBScriptParser.ReturnStmtContext bloc)
        {
            if (bloc == null)
                return false;

            Statement = new VbSimpleStatement(Context, bloc, "Return");

            return true;
        }

        public bool Append(VBScriptParser.RmdirStmtContext bloc)
        {
            if (bloc == null)
                return false;

            Statement = new VbWithArgsStatements(Context, bloc, "RmDir");

            return true;
        }

        public bool Append(VBScriptParser.RsetStmtContext bloc)
        {
            if (bloc == null)
                return false;

            Statement = new VbRSetStatement(Context, bloc);

            return true;
        }

        public bool Append(VBScriptParser.SavepictureStmtContext bloc)
        {
            if (bloc == null)
                return false;

            Statement = new VbWithArgsStatements(Context, bloc, "SavePicture");

            return true;
        }

        public bool Append(VBScriptParser.SaveSettingStmtContext bloc)
        {
            if (bloc == null)
                return false;

            Statement = new VbWithArgsStatements(Context, bloc, "SaveSetting");

            return true;
        }

        public bool Append(VBScriptParser.SeekStmtContext bloc)
        {
            if (bloc == null)
                return false;

            Statement = new VbWithArgsStatements(Context, bloc, "Seek");

            return true;
        }

        public bool Append(VBScriptParser.SelectCaseStmtContext bloc)
        {
            if (bloc == null)
                return false;

            NewBlockContext = BlockContextType.Block;
            Statement = new VbSelectCaseStatement(Context, bloc);

            return true;
        }

        public bool Append(VBScriptParser.SendkeysStmtContext bloc)
        {
            if (bloc == null)
                return false;

            Statement = new VbWithArgsStatements(Context, bloc, "SendKeys");

            return true;
        }

        public bool Append(VBScriptParser.SetattrStmtContext bloc)
        {
            if (bloc == null)
                return false;

            Statement = new VbWithArgsStatements(Context, bloc, "SetAttr");

            return true;
        }

        public bool Append(VBScriptParser.StopStmtContext bloc)
        {
            if (bloc == null)
                return false;

            Statement = new VbSimpleStatement(Context, bloc, "Stop");

            return true;
        }

        public bool Append(VBScriptParser.TimeStmtContext bloc)
        {
            if (bloc == null)
                return false;

            Statement = new VbTimeStatement(Context, bloc);

            return true;
        }

        public bool Append(VBScriptParser.UnloadStmtContext bloc)
        {
            if (bloc == null)
                return false;

            Statement = new VbWithArgsStatements(Context, bloc, "Unload");

            return true;
        }

        public bool Append(VBScriptParser.UnlockStmtContext bloc)
        {
            if (bloc == null)
                return false;

            Statement = new VbUnlockStatements(Context, bloc);

            return true;
        }

        public bool Append(VBScriptParser.VariableStmtContext bloc)
        {
            if (bloc == null)
                return false;

            NewBlockContext = BlockContextType.SpecialStatement;
            Statement = new VbVariableStatement(Context, bloc);

            return true;
        }

        public bool Append(VBScriptParser.WhileWendStmtContext bloc)
        {
            if (bloc == null)
                return false;

            NewBlockContext = BlockContextType.Block;
            Statement = new VbWhileStatement(Context, bloc);

            return true;
        }

        public bool Append(VBScriptParser.WidthStmtContext bloc)
        {
            if (bloc == null)
                return false;

            Statement = new VbWithArgsStatements(Context, bloc, "Width");

            return true;
        }

        public bool Append(VBScriptParser.WithStmtContext bloc)
        {
            if (bloc == null)
                return false;

            NewBlockContext = BlockContextType.Block;
            Statement = new VbWithStatement(Context, bloc);

            return true;
        }

        public bool Append(VBScriptParser.WriteStmtContext bloc)
        {
            if (bloc == null)
                return false;

            Statement = new VbWriteStatement(Context, bloc);

            return true;
        }


        public bool Append(VBScriptParser.PrintStmtContext bloc)
        {
            if (bloc == null)
                return false;

            Statement = new VbPrintStatement(Context, bloc);

            return true;
        }

        public bool Append(VBScriptParser.OpenStmtContext bloc)
        {
            if (bloc == null)
                return false;

            NewBlockContext = BlockContextType.SpecialStatement;
            Statement = new VbOpenStatement(Context, bloc);

            return true;
        }

        public bool Append(VBScriptParser.OnGoSubStmtContext bloc)
        {
            if (bloc == null)
                return false;

            Statement = new VbOnGoSubStatement(Context, bloc);

            return true;
        }

        public bool Append(VBScriptParser.OnGoToStmtContext bloc)
        {
            if (bloc == null)
                return false;

            Statement = new VbOnGotoStatement(Context, bloc);

            return true;
        }

        public bool Append(VBScriptParser.OnErrorStmtContext bloc)
        {
            if (bloc == null)
                return false;

            NewBlockContext = BlockContextType.SpecialStatement;
            Statement = new VbOnErrorStatement(Context, bloc);

            return true;
        }

        public bool Append(VBScriptParser.NameStmtContext bloc)
        {
            if (bloc == null)
                return false;

            Statement = new VbNameAsStatement(Context, bloc);

            return true;
        }

        public bool Append(VBScriptParser.MkdirStmtContext bloc)
        {
            if (bloc == null)
                return false;

            Statement = new VbWithArgsStatements(Context, bloc, "MkDir");

            return true;
        }

        public bool Append(VBScriptParser.MidStmtContext bloc)
        {
            return AppendGeneric(bloc);
        }


        public bool Append(VBScriptParser.LsetStmtContext bloc)
        {
            if (bloc == null)
                return false;

            Statement = new VbLSetStatement(Context, bloc);

            return true;
        }

        public bool Append(VBScriptParser.LockStmtContext bloc)
        {
            if (bloc == null)
                return false;

            Statement = new VbLockStatements(Context, bloc);

            return true;
        }

        public bool Append(VBScriptParser.LoadStmtContext bloc)
        {
            if (bloc == null)
                return false;

            Statement = new VbWithArgsStatements(Context, bloc, "Load");

            return true;
        }

        //public bool Append(VBScriptParser.LineLabelContext bloc)
        //{
        //    return AppendGeneric(bloc);
        //}

        public bool Append(VBScriptParser.LineInputStmtContext bloc)
        {
            if (bloc == null)
                return false;

            Statement = new VbWithArgsStatements(Context, bloc, "Line");

            return true;
        }


        public bool Append(VBScriptParser.KillStmtContext bloc)
        {
            if (bloc == null)
                return false;

            Statement = new VbWithArgsStatements(Context, bloc, "Kill");

            return true;
        }

        public bool Append(VBScriptParser.InputStmtContext bloc)
        {
            if (bloc == null)
                return false;

            Statement = new VbWithArgsStatements(Context, bloc, "Input");

            return true;
        }

        public bool Append(VBScriptParser.ImplementsStmtContext bloc)
        {
            if (bloc == null)
                return false;

            Statement = new VbImplementsStatement(Context, bloc);

            return true;
        }

        public bool Append(VBScriptParser.IfThenElseStmtContext bloc)
        {
            if (bloc == null)
                return false;

            NewBlockContext = BlockContextType.Block;
            Statement = new VbIfThenElseStatement(Context, bloc);

            return true;
        }

        public bool Append(VBScriptParser.InlineIfThenElseStmtContext bloc)
        {
            if (bloc == null)
                return false;

            NewBlockContext = BlockContextType.Block;
            Statement = new VbInlineIfThenElseStatement(Context, bloc);

            return true;
        }

        public bool Append(VBScriptParser.GoToStmtContext bloc)
        {
            if (bloc == null)
                return false;

            NewBlockContext = BlockContextType.SpecialStatement;
            Statement = new VbWithArgsStatements(Context, bloc, "GoTo");

            return true;
        }

        public bool Append(VBScriptParser.GoSubStmtContext bloc)
        {
            if (bloc == null)
                return false;

            NewBlockContext = BlockContextType.SpecialStatement;
            Statement = new VbWithArgsStatements(Context, bloc, "GoSub");

            return true;
        }

        public bool Append(VBScriptParser.GetStmtContext bloc)
        {
            if (bloc == null)
                return false;

            Statement = new VbWithArgsStatements(Context, bloc, "Get");

            return true;
        }

        public bool Append(VBScriptParser.ForNextStmtContext bloc)
        {
            if (bloc == null)
                return false;

            NewBlockContext = BlockContextType.Block;
            Statement = new VbForNextStatement(Context, bloc);

            return true;
        }

        public bool Append(VBScriptParser.ForEachStmtContext bloc)
        {
            if (bloc == null)
                return false;

            NewBlockContext = BlockContextType.Block;
            Statement = new VbForEachStatement(Context, bloc);

            return true;
        }

        public bool Append(VBScriptParser.FilecopyStmtContext bloc)
        {
            if (bloc == null)
                return false;

            Statement = new VbWithArgsStatements(Context, bloc, "FileCopy");

            return true;
        }

        public bool Append(VBScriptParser.ExitStmtContext bloc)
        {
            if (bloc == null)
                return false;

            NewBlockContext = BlockContextType.SpecialStatement;
            Statement = new VbExitStatement(Context, bloc);

            return true;
        }

        public bool Append(VBScriptParser.ErrorStmtContext bloc)
        {
            if (bloc == null)
                return false;

            NewBlockContext = BlockContextType.SpecialStatement;
            Statement = new VbWithArgsStatements(Context, bloc, "Error");

            return true;
        }

        public bool Append(VBScriptParser.EraseStmtContext bloc)
        {
            if (bloc == null)
                return false;

            Statement = new VbWithArgsStatements(Context, bloc, "Erase");

            return true;
        }

        public bool Append(VBScriptParser.EndStmtContext bloc)
        {
            if (bloc == null)
                return false;

            NewBlockContext = BlockContextType.SpecialStatement;
            Statement = new VbSimpleStatement(Context, bloc, "End");

            return true;
        }

        public bool Append(VBScriptParser.DoLoopStmtContext bloc)
        {
            if (bloc == null)
                return false;

            NewBlockContext = BlockContextType.Block;
            Statement = new VbDoLoopStatement(Context, bloc);

            return true;
        }

        public bool Append(VBScriptParser.DeftypeStmtContext bloc)
        {
            if (bloc == null)
                return false;

            NewBlockContext = BlockContextType.SpecialStatement;
            return AppendGeneric(bloc);
        }

        public bool Append(VBScriptParser.DeleteSettingStmtContext bloc)
        {
            if (bloc == null)
                return false;

            Statement = new VbWithArgsStatements(Context, bloc, "DeleteSetting");

            return true;
        }

        public bool Append(VBScriptParser.DateStmtContext bloc)
        {
            if (bloc == null)
                return false;

            Statement = new VbDateStatement(Context, bloc);

            return true;
        }

        public bool Append(VBScriptParser.CloseStmtContext bloc)
        {
            if (bloc == null)
                return false;

            Statement = new VbWithArgsStatements(Context, bloc, "Close");

            return true;
        }

        public bool Append(VBScriptParser.ChDriveStmtContext bloc)
        {
            if (bloc == null)
                return false;

            Statement = new VbWithArgsStatements(Context, bloc, "ChDrive");

            return true;
        }

        public bool Append(VBScriptParser.ChDirStmtContext bloc)
        {
            if (bloc == null)
                return false;

            Statement = new VbWithArgsStatements(Context, bloc, "ChDir");

            return true;
        }

        public bool Append(VBScriptParser.BeepStmtContext bloc)
        {
            if (bloc == null)
                return false;

            Statement = new VbSimpleStatement(Context, bloc, "Beep");

            return true;
        }

        public bool Append(VBScriptParser.AppActivateStmtContext bloc)
        {
            if (bloc == null)
                return false;

            Statement = new VbAppActivateStatement(Context, bloc);

            return true;
        }

        public bool Append(VBScriptParser.ExplicitCallStmtContext bloc)
        {
            if (bloc == null)
                return false;

            NewBlockContext = BlockContextType.InlineCall;

            if (Append(bloc.eCS_MemberProcedureCall()))
                return true;

            if (Append(bloc.eCS_ProcedureCall()))
                return true;

            return false;
        }

        private bool Append(VBScriptParser.ECS_ProcedureCallContext bloc)
        {
            if (bloc == null)
                return false;

            NewBlockContext = BlockContextType.InlineCall;

            Statement = new VB_ECS_ProcedureCall(Context, bloc);

            return true;
        }

        private bool Append(VBScriptParser.ECS_MemberProcedureCallContext bloc)
        {
            if (bloc == null)
                return false;

            NewBlockContext = BlockContextType.InlineCall;

            Statement = new VB_ECS_MemberProcedureCall(Context, bloc);

            return true;
        }

        public bool Append(VBScriptParser.AttributeStmtContext bloc)
        {
            if (bloc == null)
                return false;

            return AppendGeneric(bloc);
        }

        public bool Append(VBScriptParser.LineLabelContext bloc)
        {
            if (bloc == null)
                return false;

            NewBlockContext = BlockContextType.Label;
            return AppendGeneric(bloc);
        }

        public bool Append(VBScriptParser.ConstStmtContext bloc)
        {
            if (bloc == null)
                return false;

            NewBlockContext = BlockContextType.SpecialStatement;

            Statement = new VbConstStatement(Context, bloc);
            return true;
        }

    }
}