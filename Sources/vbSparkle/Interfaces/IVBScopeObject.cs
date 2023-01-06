using vbSparkle.Options;

namespace vbSparkle
{
    public interface IVBScopeObject
    {
        EvaluatorOptions Options { get; set; }

        VbIdentifiedObject GetIdentifiedObject(string identifier);

        void DeclareVariable(VbUserVariable variable);
        void DeclareConstant(VbSubConstStatement vbSubConstStatement);

        void SetVarValue(string identifier, DExpression value);
    }
}