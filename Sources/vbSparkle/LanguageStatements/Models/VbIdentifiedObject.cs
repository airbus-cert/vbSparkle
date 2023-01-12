using System.Linq;

namespace vbSparkle
{
    public abstract class VbIdentifiedObject
    {
        public IVBScopeObject Context { get; internal set; }
        public string Identifier { get; internal set; }
        public string Name { get; internal set; }

        public VbIdentifiedObject(
            IVBScopeObject context,
            string identifier)
        {
            Name = KeywordHelper.RenameVar(identifier);
            Context = context;
            Identifier = identifier.ToUpper();
        }

        public abstract DExpression Prettify(bool partialEvaluation = false);

        public DExpression PrettifyCall(bool useParenthesis = true, params DExpression[] args)
        {
            string[] sArgs = args.Select(v => v.ToExpressionString()).ToArray();
            return PrettifyCall(args: sArgs);
        }

        public DExpression PrettifyCall(bool useParenthesis = true, params string[] args)
        {
            return new DCodeBlock($"{Prettify()}({string.Join(", ", args)})");
        }
    }

    public class VbUnknownIdentifiedObject : VbIdentifiedObject
    {
        public VbUnknownIdentifiedObject(
            IVBScopeObject context, 
            string identifier
            ) 
            : base(context, identifier)
        {
        }

        private DExpression value = null; 
        
        public DExpression CurrentValue
        {
            get
            {
                if (value == null)
                    return new DCodeBlock(Identifier);
                else
                    return value;
            }
            internal set { this.value = value; }
        }
        public int WriteAccess { get; internal set; }

        public override DExpression Prettify(bool partialEvaluation)
        {
            return TryEvaluate();
        }


        public DExpression TryEvaluate()
        {
            if (CurrentValue != null && !(CurrentValue is DCodeBlock))
            {
                return CurrentValue;
            }

            return new DCodeBlock(Identifier);
        }
    }
}