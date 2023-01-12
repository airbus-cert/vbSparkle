using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vbSparkle
{
    public class VbUserFunction : VbUserScopeObject<VBScriptParser.FunctionStmtContext>
    {
        public VbVisibilityStatement Visibility { get; set; }
        public VbSimpleStackBlock CodeBlock { get; set; }
        public VBScriptParser.TypeContext Type { get; set; } 
        public Dictionary<string, VbUserArg> ArgList { get; set; } = new Dictionary<string, VbUserArg>();

        public VbUserFunction(
            IVBScopeObject context,
            VBScriptParser.FunctionStmtContext @object)
            : base(
                context,
                @object,
                @object?.ambiguousIdentifier()?.GetText() ?? "<GLOBAL>")
        {
            foreach (var arg in @object.argList().arg())
            {
                string argId = arg.ambiguousIdentifier().GetText();
                VbUserArg usrArg = new VbUserArg(this, arg, argId);
                ArgList.Add(argId.ToUpper(), usrArg);
            }
            

            Type  = @object.asTypeClause()?.type();
            CodeBlock = new VbSimpleStackBlock(this, @object.block());

            Visibility = new VbVisibilityStatement(this, @object.visibility());
        }

        public override VbIdentifiedObject GetIdentifiedObject(string identifier)
        {
            if (ArgList.ContainsKey(identifier.ToUpper()))
                return ArgList[identifier.ToUpper()];

            return base.GetIdentifiedObject(identifier);
        }

        public override DExpression Prettify(bool partialEvaluation = false)
        {
            StringBuilder sb = new StringBuilder();

            string visibility = Visibility.Prettify();
            if (!string.IsNullOrWhiteSpace(visibility))
                sb.Append($"{visibility} ");

            string arguments = string.Join(", ", ArgList.Select(v => v.Value.Exp(partialEvaluation)));

            if (Type != null)
                sb.AppendLine($"Function {Name}({arguments}) As {Type.GetText()}");
            else
                sb.AppendLine($"Function {Name}({arguments})");

            sb.AppendLine(Helpers.IndentLines(Context.Options.IndentSpacing, CodeBlock.Exp(partialEvaluation)));
            sb.Append("End Function");

            return new DCodeBlock(sb.ToString());
        }
    }
}