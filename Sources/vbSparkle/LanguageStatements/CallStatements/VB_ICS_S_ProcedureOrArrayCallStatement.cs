using System;
using System.Collections.Generic;
using System.Linq;

namespace vbSparkle
{
    public class VB_ICS_S_ProcedureOrArrayCallStatement : VbValueAssignableStatement<VBScriptParser.ICS_S_ProcedureOrArrayCallContext>
    {
        public string Identifier { get; set; }
        public List<List<VBArgCall>> CallArgs { get; set; } = new List<List<VBArgCall>>();
        public VbDefaultMemberAccessStatement DefaultMemberAccess { get; }
        public VB_ICS_S_NestedProcedureCallStatement NestedProcCall { get; }

        public VB_ICS_S_ProcedureOrArrayCallStatement(IVBScopeObject context, VBScriptParser.ICS_S_ProcedureOrArrayCallContext bloc)
            : base(context, bloc)
        {
            Identifier = bloc.ambiguousIdentifier()?.GetText() ?? bloc.baseType()?.GetText();
           

            foreach (var arg in bloc.argsCall())
            {
                List<VBArgCall> CallArg = new List<VBArgCall>();

                CallArgs.Add(CallArg);

                foreach (var argCall in arg.argCall())
                {
                    CallArg.Add(new VBArgCall(context, argCall));
                }
            }


            // Not supported
            var nestedProcCall = bloc.iCS_S_NestedProcedureCall();
            var defaultMemberAccess = bloc.defaultMemberAccess();

            if (nestedProcCall != null)
                NestedProcCall = new VB_ICS_S_NestedProcedureCallStatement(context, bloc.iCS_S_NestedProcedureCall());

            if (defaultMemberAccess != null)
                DefaultMemberAccess = new VbDefaultMemberAccessStatement(context, bloc.defaultMemberAccess());

            //TODO
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
                var funcArgs = CallArgs[0].ToArray();
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
            var arguments = string.Concat(this.CallArgs.Select(v => "(" + string.Join(", ", v.Select(q => q.Exp(partialEvaluation))) + ")"));

            string baseObject = "";
            if (NestedProcCall == null)
                baseObject = IdentifiedObject.Name;
            else
                baseObject = NestedProcCall.Exp(partialEvaluation);

            return new DCodeBlock($"{baseObject}{arguments}");
        }
    }
    
}
