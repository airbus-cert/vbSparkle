namespace vbSparkle
{
    public class VbUserPropertyGet : VbUserScopeObject<VBScriptParser.PropertyGetStmtContext>
    {
        public VbUserPropertyGet(
            IVBScopeObject context,
            VBScriptParser.PropertyGetStmtContext @object)
            : base(
                context,
                @object,
                @object?.ambiguousIdentifier()?.GetText() ?? "<GLOBAL>")
        {
            @object.ambiguousIdentifier();
            @object.argList();
            @object.asTypeClause();
            @object.block();
            @object.visibility();
            @object.typeHint();
        }
    }
}