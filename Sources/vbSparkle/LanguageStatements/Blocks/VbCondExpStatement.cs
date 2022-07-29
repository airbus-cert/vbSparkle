using System.Linq;
using static vbSparkle.VBScriptParser;

namespace vbSparkle
{
    public class VbCondExpStatement : VbStatement<SC_CondExprContext>
    {
        public VbCondExpStatement(IVBScopeObject context, SC_CondExprContext obj) 
            : base(context, obj)
        {

        }

        public override DExpression Prettify(bool partialEvaluation = false)
        {
            if (this.Object is CaseCondExprIsContext)
            {
                var stmt = this.Object as CaseCondExprIsContext;
                string comp = stmt.comparisonOperator().GetText();
                var valueStmt = VBValueStatement.Get(Context, stmt.valueStmt());
                return new DCodeBlock($"{comp} {valueStmt.Exp(partialEvaluation)}");

            } 

            if (this.Object is CaseCondExprToContext)
            {
                var stmt = this.Object as CaseCondExprToContext;
                var values = stmt.valueStmt().Select(v => VBValueStatement.Get(this.Context, v));


                return new DCodeBlock($"{values.First().Exp(partialEvaluation)} To {values.Last().Exp(partialEvaluation)}");
            }

            if (this.Object is CaseCondExprValueContext)
            {
                var stmt = this.Object as CaseCondExprValueContext;
                var valueStmt = VBValueStatement.Get(Context, stmt.valueStmt());
                return new DCodeBlock($"{valueStmt.Exp(partialEvaluation)}");
            }

            return base.Prettify(partialEvaluation);
        }
    }
}