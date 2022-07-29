namespace vbSparkle
{
    public class VbInlineBlock : VbGenericBlock<VBScriptParser.InlineBlockStmtContext>
    {

        public VbInlineBlock(
           IVBScopeObject context,
           VBScriptParser.InlineBlockStmtContext @object)
            : base(context, @object)
        {
            Append(@object);
        }


        public void Append(VBScriptParser.InlineBlockStmtContext @object)
        {
            if (Append(@object.constStmt()))
                return;

            if (Append(@object.appActivateStmt()))
                return;

            if (Append(@object.constStmt()))
                return;

            if (Append(@object.attributeStmt()))
                return;

            if (Append(@object.beepStmt()))
                return;

            if (Append(@object.chDirStmt()))
                return;

            if (Append(@object.chDriveStmt()))
                return;

            if (Append(@object.closeStmt()))
                return;

            if (Append(@object.dateStmt()))
                return;

            if (Append(@object.deleteSettingStmt()))
                return;

            if (Append(@object.deftypeStmt()))
                return;

            if (Append(@object.doLoopStmt()))
                return;

            if (Append(@object.endStmt()))
                return;

            if (Append(@object.eraseStmt()))
                return;

            if (Append(@object.errorStmt()))
                return;

            if (Append(@object.exitStmt()))
                return;

            if (Append(@object.explicitCallStmt()))
                return;

            if (Append(@object.filecopyStmt()))
                return;

            if (Append(@object.forEachStmt()))
                return;

            if (Append(@object.forNextStmt()))
                return;

            if (Append(@object.getStmt()))
                return;

            if (Append(@object.goSubStmt()))
                return;

            if (Append(@object.goToStmt()))
                return;

            if (Append(@object.inlineIfThenElseStmt()))
                return;

            if (Append(@object.implementsStmt()))
                return;

            if (Append(@object.inputStmt()))
                return;

            if (Append(@object.killStmt()))
                return;

            if (Append(@object.letStmt()))
                return;

            if (Append(@object.lineInputStmt()))
                return;
            //if (Append(bloc.lineLabel()))
            //    return;
            if (Append(@object.loadStmt()))
                return;

            if (Append(@object.lockStmt()))
                return;

            if (Append(@object.lsetStmt()))
                return;
            //if (Append(@object.macroIfThenElseStmt()))
            //    return;
            if (Append(@object.midStmt()))
                return;

            if (Append(@object.mkdirStmt()))
                return;

            if (Append(@object.nameStmt()))
                return;

            if (Append(@object.onErrorStmt()))
                return;

            if (Append(@object.onGoToStmt()))
                return;

            if (Append(@object.onGoSubStmt()))
                return;

            if (Append(@object.openStmt()))
                return;

            if (Append(@object.printStmt()))
                return;

            if (Append(@object.putStmt()))
                return;

            if (Append(@object.raiseEventStmt()))
                return;

            if (Append(@object.randomizeStmt()))
                return;

            if (Append(@object.redimStmt()))
                return;

            if (Append(@object.resetStmt()))
                return;

            if (Append(@object.resumeStmt()))
                return;

            if (Append(@object.returnStmt()))
                return;

            if (Append(@object.rmdirStmt()))
                return;

            if (Append(@object.rsetStmt()))
                return;

            if (Append(@object.savepictureStmt()))
                return;

            if (Append(@object.saveSettingStmt()))
                return;

            if (Append(@object.seekStmt()))
                return;

            if (Append(@object.selectCaseStmt()))
                return;

            if (Append(@object.sendkeysStmt()))
                return;

            if (Append(@object.setattrStmt()))
                return;

            if (Append(@object.setStmt()))
                return;

            if (Append(@object.stopStmt()))
                return;

            if (Append(@object.timeStmt()))
                return;

            if (Append(@object.unloadStmt()))
                return;

            if (Append(@object.unlockStmt()))
                return;

            if (Append(@object.variableStmt()))
                return;

            if (Append(@object.whileWendStmt()))
                return;

            if (Append(@object.widthStmt()))
                return;

            if (Append(@object.withStmt()))
                return;

            if (Append(@object.writeStmt()))
                return;

            if (Append(@object.implicitCallStmt_InBlock()))
                return;

            if (Append(@object.implicitCallStmt_InStmt()))
                return;

            //Append(bloc.children.FirstOrDefault());
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
            return Statement.Prettify(partialEvaluation);
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}