namespace vbSparkle
{
    public class VbReDimSubStatement : VbStatement<VBScriptParser.RedimSubStmtContext>
    {
        public string VType { get; }
        public string Identifier { get; }
        public VbSubscriptsStatement SubScripts { get; }

        public VbReDimSubStatement(IVBScopeObject context, VBScriptParser.RedimSubStmtContext bloc)
            : base(context, bloc)
        {
            VType = bloc.asTypeClause()?.type()?.GetText();
            Identifier = bloc.ambiguousIdentifier().GetText();
            SubScripts = new VbSubscriptsStatement(context, bloc.subscripts());
        }

        public VbIdentifiedObject IdentifiedObject
        {
            get { return Context.GetIdentifiedObject(Identifier); }
        }

        public override DExpression Prettify(bool partialEvaluation)
        {
            string ret = $"{IdentifiedObject.Name}({SubScripts.Exp(partialEvaluation)})";

            if (VType != null)
                ret += $" As {VType}";

            return new DCodeBlock(ret);
        }
    }
}