using System;
using System.Text;

namespace vbSparkle
{
    public class VbUserClass : VbUserScopeObject<VBScriptParser.ClassStmtContext>
    {

        public VbModuleBody Body { get; set; }
        public string Visibility { get; set; }

        public VbUserClass(
            IVBScopeObject context, 
            VBScriptParser.ClassStmtContext @object) 
            : base(
                context,
                @object,
                @object?.ambiguousIdentifier()?.GetText() ?? "<GLOBAL>")
        {
            
            var body = @object.moduleBody();
            if (body != null)
            {
                Body = new VbModuleBody(context, body, Identifier);
            }

            Visibility = @object.publicPrivateVisibility()?.GetText()?.ToLower();

            if (Visibility != null)
                Visibility = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Visibility);
            else
                Visibility = string.Empty;
        }

        public VbIdentifiedObject IdentifiedObject
        {
            get { return Context.GetIdentifiedObject(Identifier); }
        }

        public override DExpression Prettify(bool partialEvaluation = false)
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(Visibility))
            {
                sb.Append(Visibility);
                sb.Append(" Class ");
            }
            else
            {
                sb.Append("Class ");
            }
            sb.AppendLine(IdentifiedObject.Name);
            sb.AppendLine(Helpers.IndentLines(Context.Options.IndentSpacing, Body.Prettify(partialEvaluation)));
            sb.Append("End Class");

            System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Visibility);
            return new DCodeBlock(sb.ToString());
        }
    }
}