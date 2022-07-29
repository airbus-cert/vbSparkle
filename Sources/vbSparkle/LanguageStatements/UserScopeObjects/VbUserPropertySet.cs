namespace vbSparkle
{
    public class VbUserPropertySet : VbUserScopeObject<VBScriptParser.PropertySetStmtContext>
    {
        public VbUserPropertySet(
            IVBScopeObject context,
            VBScriptParser.PropertySetStmtContext @object)
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