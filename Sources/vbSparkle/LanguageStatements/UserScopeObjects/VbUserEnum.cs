namespace vbSparkle
{
    public class VbUserEnum : VbUserIdentifiedObject<VBScriptParser.EnumerationStmtContext>
    {
        public VbUserEnum(
            IVBScopeObject context,
            VBScriptParser.EnumerationStmtContext @object)
            : base(
                context,
                @object,
                @object?.ambiguousIdentifier()?.GetText() ?? "<GLOBAL>")
        {
            @object.ambiguousIdentifier();
            @object.enumerationStmt_Constant();
            @object.publicPrivateVisibility();
        }
    }
}