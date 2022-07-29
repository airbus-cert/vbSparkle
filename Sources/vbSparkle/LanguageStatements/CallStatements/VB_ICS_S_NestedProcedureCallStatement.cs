using System;
using System.Collections.Generic;
using System.Linq;

namespace vbSparkle
{
    public class VB_ICS_S_NestedProcedureCallStatement : VbValueAssignableStatement<VBScriptParser.ICS_S_NestedProcedureCallContext>
    {
        public string Identifier { get; set; }
        public string TypeHInt { get; }
        public List<VBArgCall> CallArgs { get; set; } = new List<VBArgCall>();

        public VB_ICS_S_NestedProcedureCallStatement(IVBScopeObject context, VBScriptParser.ICS_S_NestedProcedureCallContext bloc)
            : base(context, bloc)
        {
            Identifier = bloc.ambiguousIdentifier()?.GetText();
            TypeHInt = bloc.typeHint().GetText();

            foreach (var argCall in bloc.argsCall()?.argCall())
            {
                CallArgs.Add(new VBArgCall(context, argCall));
            }
        }

        public VbIdentifiedObject IdentifiedObject
        {
            get { return Context.GetIdentifiedObject(Identifier); }
        }

        public override DExpression Prettify(bool partialEvaluation = false)
        {
            if (partialEvaluation)
                try
                {
                    return Evaluate();
                }
                catch { }

            return GetAssignableExpression(partialEvaluation);
        }

        public bool CanEvaluate()
        {
            return IdentifiedObject is VbNativeFunction || IdentifiedObject is VbUserVariable;
        }

        public  DExpression Evaluate()
        {
            var identifiedObject = IdentifiedObject;

            if (identifiedObject is VbNativeFunction)
            {
                var funcArgs = CallArgs.ToArray();
                return (identifiedObject as VbNativeFunction).TryEvaluate(funcArgs);
            }
            else if (identifiedObject is VbUserVariable)
            {
                if (CallArgs.Count() == 0)
                    return (identifiedObject as VbUserVariable).TryEvaluate();
            }

            return GetAssignableExpression(true);
        }

        public override DExpression GetAssignableExpression(bool partialEvaluation = false)
        {
            var arguments = string.Join(", ", CallArgs.Select(q => q.Exp(partialEvaluation)));

            return new DCodeBlock($"{IdentifiedObject.Name}{TypeHInt}({arguments})");
        }
    }
    
}
