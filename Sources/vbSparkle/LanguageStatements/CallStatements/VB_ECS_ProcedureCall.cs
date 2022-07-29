using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace vbSparkle
{

    /// <summary>
    /// Represent a simple call without parenthesis and return values.
    /// </summary>
    public class VB_ECS_ProcedureCall : VbStatement<VBScriptParser.ECS_ProcedureCallContext>
    {
        public string Identifier { get; set; }

        public List<VBArgCall> CallArgs { get; set; } = new List<VBArgCall>();

        public VB_ECS_ProcedureCall(
            IVBScopeObject context, 
            VBScriptParser.ECS_ProcedureCallContext bloc)
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
            return new DCodeBlock($"Call {IdentifiedObject.Name}({string.Join(", ", CallArgs.Select(v => v.Exp(partialEvaluation)))})");
        }
    }
    
}
