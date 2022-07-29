namespace vbSparkle
{
    public interface IVBScopeObject
    {
        VbIdentifiedObject GetIdentifiedObject(string identifier);

        void DeclareVariable(VbUserVariable variable);
        void DeclareConstant(VbSubConstStatement vbSubConstStatement);

        void SetVarValue(string identifier, DExpression value);
    }
}