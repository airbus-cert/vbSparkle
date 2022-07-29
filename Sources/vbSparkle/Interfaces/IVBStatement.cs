using System;

namespace vbSparkle
{
    public interface IVBStatement
    {
        DCodeBlock GetCodeBlock();

        DExpression Evaluate();
    }
}