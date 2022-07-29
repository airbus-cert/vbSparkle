namespace vbSparkle
{
    public class VbDefaultMemberAccessStatement : VbValueAssignableStatement<VBScriptParser.DefaultMemberAccessContext>
    {
        public string Identifier { get; }
        public string TypeHInt { get; }

        public VbDefaultMemberAccessStatement(IVBScopeObject context, VBScriptParser.DefaultMemberAccessContext obj) 
            : base(context, obj)
        {
            Identifier = obj.ambiguousIdentifier().GetText();
            TypeHInt = obj.typeHint().GetText();
        }


        public override DExpression GetAssignableExpression(bool partialEvaluation = false)
        {
            return new DCodeBlock($"!{Identifier}{TypeHInt}");
        }
    }
    
}
