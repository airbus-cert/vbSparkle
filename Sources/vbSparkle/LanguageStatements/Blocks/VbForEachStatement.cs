using System.Linq;
using System.Text;
using static vbSparkle.VBScriptParser;

namespace vbSparkle
{
    public class VbForEachStatement
        : VbStatement<ForEachStmtContext>
    {
        string firstIdentifier { get; set; }
        string secondIdentifier { get; set; }

        public VbSimpleStackBlock CodeBlock { get; set; }
        public VBValueStatement Value { get; set; }

        public VbForEachStatement(IVBScopeObject context, ForEachStmtContext bloc)
            : base(context, bloc)
        {
            firstIdentifier = bloc.ambiguousIdentifier().FirstOrDefault().GetText();
            secondIdentifier = bloc.ambiguousIdentifier().LastOrDefault().GetText();

            CodeBlock = new VbSimpleStackBlock(context, bloc.block());
            Value = VBValueStatement.Get(context, bloc.valueStmt());
        }

        public VbIdentifiedObject FirstIdentifiedObject
        {
            get { return Context.GetIdentifiedObject(firstIdentifier); }
        }

        public VbIdentifiedObject SecondIdentifiedObject
        {
            get { return Context.GetIdentifiedObject(secondIdentifier); }
        }

        public override DExpression Prettify(bool partialEvaluation = false)
        {
            StringBuilder retCode = new StringBuilder();
            retCode.AppendLine($"For Each {FirstIdentifiedObject.Name} In {Value.Exp(partialEvaluation)}");

            retCode.AppendLine(Helpers.IndentLines(Context.Options.IndentSpacing, CodeBlock.Exp(partialEvaluation)));

            retCode.Append($"Next {SecondIdentifiedObject.Name}");


            return new DCodeBlock(retCode.ToString());
        }
    }
}