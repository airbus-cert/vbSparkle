using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace vbSparkle
{

    /// <summary>
    /// Represent a simple call without parenthesis and return values.
    /// </summary>
    public class VbRaiseEventStatement : VbStatement<VBScriptParser.RaiseEventStmtContext>
    {
        public string Identifier { get; set; }

        public List<VBArgCall> CallArgs { get; set; } = new List<VBArgCall>();

        public VbRaiseEventStatement(
            IVBScopeObject context, 
            VBScriptParser.RaiseEventStmtContext bloc)
            : base(context, bloc)
        {
            Identifier = bloc.ambiguousIdentifier().GetText();

            var args = bloc.argsCall();

            if (args != null)
                foreach (var arg in args.argCall())
                {
                    CallArgs.Add(new VBArgCall(context, arg));
                }
        }


        public VbIdentifiedObject IdentifiedObject
        {
            get { return Context.GetIdentifiedObject(Identifier); }
        }

        public override DExpression Prettify(bool partialEvaluation = false)
        {
            return new DCodeBlock($"RaiseEvent {IdentifiedObject.Name}({string.Join(", ", CallArgs.Select(v => v.Exp(partialEvaluation)))})");
        }
    }
    
}
