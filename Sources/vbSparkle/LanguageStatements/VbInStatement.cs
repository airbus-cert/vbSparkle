using System;

namespace vbSparkle
{
    public class VbInStatement : VbValueAssignableStatement<VBScriptParser.ImplicitCallStmt_InStmtContext>
    {
        private IVbValueAssignableStatement Statement { get; set; }

        public VbInStatement(IVBScopeObject context, VBScriptParser.ImplicitCallStmt_InStmtContext @object)
            : base(context, @object)
        {
            Context = context;
            Object = @object;

            Visit(@object.iCS_S_DefaultMemberAccess());
            Visit(@object.iCS_S_MembersCall());
            Visit(@object.iCS_S_ProcedureOrArrayCall());
            Visit(@object.iCS_S_VariableOrProcedureCall());
        }

        private bool Visit(VBScriptParser.ICS_S_VariableOrProcedureCallContext bloc)
        {
            if (bloc == null)
                return false;

            Statement = new VB_ICS_S_VariableOrProcedureCallStatement(Context, bloc); 
            return true;
        }

        internal void SetValue(DExpression dExpression)
        {
            if (Statement is VB_ICS_S_VariableOrProcedureCallStatement)
            {
                var ide = (Statement as VB_ICS_S_VariableOrProcedureCallStatement).Identifier;
                Context.SetVarValue(ide, dExpression);
            }
        }

        private bool Visit(VBScriptParser.ICS_S_ProcedureOrArrayCallContext bloc)
        {
            if (bloc == null)
                return false;

            Statement = new VB_ICS_S_ProcedureOrArrayCallStatement(Context, bloc);
            return true;
        }

        private bool Visit(VBScriptParser.ICS_S_MembersCallContext bloc)
        {
            if (bloc == null)
                return false;

            Statement = new VB_ICS_S_MembersCallStatement(Context, bloc);
            return true;
        }

        private bool Visit(VBScriptParser.ICS_S_DefaultMemberAccessContext bloc)
        {
            if (bloc == null)
                return false;

            Statement = new VbDefaultMemberAccessStatement(Context, bloc.defaultMemberAccess());
            return true;
        }

        public override DExpression Prettify(bool partialEvaluation = false)
        {
            return Statement.Prettify(partialEvaluation);
        }

        public override DExpression GetAssignableExpression(bool partialEvaluation = false)
        {
            return Statement.GetAssignableExpression(partialEvaluation);
        }
    }
}