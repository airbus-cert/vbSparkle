using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace vbSparkle
{

    /// <summary>
    /// Represent a simple call without parenthesis and return values.
    /// </summary>
    public class VB_ICS_B_ProcedureCall : VbStatement<VBScriptParser.ICS_B_ProcedureCallContext>
    {
        public string Identifier { get; set; }

        public List<VBArgCall> CallArgs { get; set; } = new List<VBArgCall>();

        public VB_ICS_B_ProcedureCall(
            IVBScopeObject context, 
            VBScriptParser.ICS_B_ProcedureCallContext bloc)
            : base(context, bloc)
        {
            Identifier = bloc.certainIdentifier().GetText();

            var args = bloc.noParenthesisArgs().argsCall();

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
            if (CallArgs.Any())
                return new DCodeBlock($"{IdentifiedObject.Name} {string.Join(", ", CallArgs.Select(v => v.Exp(partialEvaluation)))}");
            else
                return new DCodeBlock($"{IdentifiedObject.Name}");
        }
    }
    
}
