using System.Text;
using vbSparkle.EvaluationObjects;
using static vbSparkle.VBScriptParser;

namespace vbSparkle
{
    public class VbLtString : VBLiteral<LtStringContext>
    {
        public VbLtString(LtStringContext @object) 
            : base(@object)
        {
            string quoted = @object.GetText();
            Value = new DSimpleStringExpression(quoted.Substring(1, quoted.Length -2), Encoding.Unicode);
        }
        
        public override string Prettify()
        {
            DSimpleStringExpression val = (DSimpleStringExpression) Value;
            return val.ToExpressionString();
        }
    }

}