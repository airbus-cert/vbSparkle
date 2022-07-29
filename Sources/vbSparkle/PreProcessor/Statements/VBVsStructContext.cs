using System.Collections.Generic;
using System.Linq;
using static vbSparkle.VBPreprocessorsParser;

namespace vbSparkle.PreProcessor.Statements
{
    public class VBVsStructContext
      : VBValueStatement<VsStructContext>
    {
        private List<VBMacroValueStatement> Values { get; set; } = new List<VBMacroValueStatement>();

        public VBVsStructContext(IVBScopeObject context, VsStructContext @object)
            : base(context, @object)
        {
            foreach (var value in @object.valueStmt())
            {
                Values.Add(Get(context, value));
            }
        }

        public override DExpression Prettify(bool partialEvaluation = false)
        {
            if (partialEvaluation)
                try
                {
                    return Evaluate();
                }
                catch { }

            return GetCode(partialEvaluation);
        }


        public override DExpression Evaluate()
        {
            if (Values.Count == 1)
            {
                return Values.FirstOrDefault().Evaluate();
            }
            else
            {
                return GetCode(true);
            }

        }
        private DExpression GetCode(bool partialEvaluation)
        {
            string[] values = Values.Select(v => v.Exp(partialEvaluation)).ToArray();
            return new DCodeBlock($"({string.Join(", ", values)})");
        }
    }
}
