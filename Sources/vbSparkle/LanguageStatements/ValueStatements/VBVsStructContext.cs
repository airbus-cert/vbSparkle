using System.Collections.Generic;
using System.Linq;
using static vbSparkle.VBScriptParser;

namespace vbSparkle
{
    public class VBVsStructContext
        : VBValueStatement<VsStructContext>
    {
        private List<VBValueStatement> Values { get; set; } = new List<VBValueStatement>();

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

            string[] values = Values.Select(v => v.Exp(partialEvaluation)).ToArray();
            return new DCodeBlock($"({string.Join(", ", values)})");
        }

        public override DExpression Evaluate()
        {
            if (Values.Count == 1)
            {
                return Values.FirstOrDefault().Evaluate();
            }
            else
            {
                string[] values = Values.Select(v => v.Exp(true)).ToArray();
                return new DCodeBlock($"({string.Join(", ", values)})");
            }

        }
    }

}