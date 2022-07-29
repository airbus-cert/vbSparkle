using System.Linq;
using System.Text;

namespace vbSparkle
{
    public class VB_ICS_S_VariableOrProcedureCallStatement : VbValueAssignableStatement<VBScriptParser.ICS_S_VariableOrProcedureCallContext>
    {
        public string Identifier { get; set; }

        public VbDefaultMemberAccessStatement DicCall { get; set; }
        public string TypeHInt { get; set; }

        public VB_ICS_S_VariableOrProcedureCallStatement(
            IVBScopeObject context, 
            VBScriptParser.ICS_S_VariableOrProcedureCallContext bloc)
            : base(context, bloc)
        {
            Identifier = bloc.ambiguousIdentifier().GetText();
            TypeHInt = bloc.typeHint()?.GetText();
            
            var dicCall = bloc.defaultMemberAccess();

            if(dicCall != null)
                DicCall = new VbDefaultMemberAccessStatement(context, dicCall);
        }

        public VbIdentifiedObject IdentifiedObject
        {
            get { return Context.GetIdentifiedObject(Identifier); }
        }

        public override DExpression Prettify(bool partialEvaluation = false)
        {
            if (partialEvaluation)
            {
                if (IdentifiedObject is VbUnknownIdentifiedObject)
                {
                    var varValue = ((VbUnknownIdentifiedObject)IdentifiedObject).CurrentValue;
                    if (!varValue.IsValuable) // TO BE CHECKED
                    {
                        return GetAssignableExpression(partialEvaluation);
                    }
                    else
                    {
                        return varValue;
                    }
                }

                if (IdentifiedObject is VbUserVariable)
                {
                    var varValue = ((VbUserVariable)IdentifiedObject).CurrentValue;
                    if (!varValue.IsValuable) // TO BE CHECKED
                    {
                        return GetAssignableExpression(partialEvaluation);
                    }
                    else
                    {
                        return varValue;
                    }
                }

                if (IdentifiedObject is VbSubConstStatement)
                { 
                    var varValue = ((VbSubConstStatement)IdentifiedObject).Value;
                    if (!varValue.IsValuable) // TO BE CHECKED
                    {
                        return GetAssignableExpression(partialEvaluation);
                    }
                    else
                    {
                        return varValue;
                    }
                }
            }

            return GetAssignableExpression(partialEvaluation);
        }

        public override DExpression GetAssignableExpression(bool partialEvaluation = false)
        {
            if (IdentifiedObject is VbNativeConstants)
            {
                if (partialEvaluation)
                {
                    return ((VbNativeConstants)IdentifiedObject).Prettify(partialEvaluation);
                }
            }

            StringBuilder sb = new StringBuilder();
            sb.Append(IdentifiedObject.Name);

            if (!string.IsNullOrWhiteSpace(TypeHInt))
                sb.Append(TypeHInt);

            if (DicCall != null)
                sb.Append(DicCall.Exp(partialEvaluation));

            return new DCodeBlock(sb.ToString());
        }
    }
    
}
