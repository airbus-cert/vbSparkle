namespace vbSparkle
{
    public class VbUserType : VbUserIdentifiedObject<VBScriptParser.TypeStmtContext>
    {
        public VbUserType(
            IVBScopeObject context,
            VBScriptParser.TypeStmtContext @object)
            : base(
                context,
                @object,
                @object?.ambiguousIdentifier()?.GetText() ?? "<GLOBAL>")
        {
            @object.ambiguousIdentifier();
            @object.typeStmt_Element();
            @object.visibility();
        }
    }
}