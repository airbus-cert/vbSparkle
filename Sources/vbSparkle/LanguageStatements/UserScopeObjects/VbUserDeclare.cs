namespace vbSparkle
{
    public class VbUserDeclare : VbUserIdentifiedObject<VBScriptParser.DeclareStmtContext>
    {
        public VbUserDeclare(
            IVBScopeObject context,
            VBScriptParser.DeclareStmtContext @object)
            : base(
                context,
                @object,
                @object?.ambiguousIdentifier()?.GetText() ?? "<GLOBAL>")
        {
            @object.ambiguousIdentifier();
            @object.argList();
            @object.asTypeClause();
            @object.typeHint();
            @object.visibility();
        }
    }

    public class VbUserAttribute : VbUserIdentifiedObject<VBScriptParser.AttributeStmtContext>
    {
        public VbUserAttribute(
            IVBScopeObject context,
            VBScriptParser.AttributeStmtContext @object)
            : base(
                context,
                @object,
                @object.implicitCallStmt_InStmt().GetText())
        {
            //@object.ambiguousIdentifier();
            //@object.argList();
            //@object.asTypeClause();
            //@object.typeHint();
            //@object.visibility();
        }
    }
}