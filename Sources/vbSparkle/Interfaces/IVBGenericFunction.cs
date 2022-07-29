using System.Collections.Generic;

namespace vbSparkle
{
    public interface IVBGenericFunction
    {
        DExpression TryEvaluate(params VBArgCall[] args);
        DExpression Evaluate(params DExpression[] args);
    }
}