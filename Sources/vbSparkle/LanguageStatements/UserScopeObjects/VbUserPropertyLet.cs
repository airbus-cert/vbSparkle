namespace vbSparkle
{
    public class VbUserPropertyLet : VbUserScopeObject<VBScriptParser.PropertyLetStmtContext>
    {
        public VbUserPropertyLet(
            IVBScopeObject context,
            VBScriptParser.PropertyLetStmtContext @object)
            : base(
                context,
                @object,
                @object?.ambiguousIdentifier()?.GetText() ?? "<GLOBAL>")
        {
            @object.ambiguousIdentifier();
            @object.argList();
            @object.block();
            @object.visibility();
        }
    }
}