using System.Collections.Generic;
using System.Linq;

namespace vbSparkle
{
    public abstract class VbNativeFunction : VbNativeIdentifiedObject, IVBGenericFunction
    {
        public VbNativeFunction(
            IVBScopeObject context, string identifier) 
            : base(context, identifier)
        {
        }

        public DExpression TryEvaluate(params VBArgCall[] args)
        {
            List<DExpression> fArgs = new List<DExpression>();
            bool allConstants = true;

            foreach (var v in args)
            {
                var ev = v.Prettify(true);

                fArgs.Add(ev);

                if (ev is DCodeBlock)
                    allConstants = false;
            }

            if (allConstants)
                try
                {
                    return Evaluate(fArgs.ToArray());
                }
                catch { }


            return PrettifyCall(args: fArgs.ToArray());
        }

        public abstract DExpression Evaluate(params DExpression[] args);

        public override DExpression Prettify(bool partialEvaluation = false)
        {
            return new DCodeBlock(Name);
        }

        internal DExpression DefaultExpression(params DExpression[] args)
        {
            return new DCodeBlock($"{Name}({string.Join(", ", args.Select(v => v.ToExpressionString()))})");
        }
    }
}
