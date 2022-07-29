using System;
using Antlr4.Runtime.Tree;

namespace vbSparkle
{
    public class VbStatement
    {
        public IVBScopeObject Context { get; set; }
        public IParseTree Object { get; set; }

        public VbStatement(IVBScopeObject context, IParseTree @obj)
        {
            Context = context;
            Object = @obj;
        }

        public virtual DExpression Prettify(bool partialEvaluation = false)
        {
            return new DCodeBlock(Object.GetText().Trim("\t\r\n ".ToCharArray()));
        }

        public string Exp(bool partialEvaluation = false)
        {
            return Prettify(partialEvaluation).ToExpressionString();
        }
    }
    
    public class VbStatement<T> : VbStatement where T : IParseTree
    {
        public new T Object { get; set; }

        public VbStatement(IVBScopeObject context, T @obj) : base(context, @obj)
        {
            Context = context;
            Object = @obj;
        }

        public override DExpression Prettify(bool partialEvaluation = false)
        {
            return new DCodeBlock(Object.GetText().Trim("\t\r\n ".ToCharArray()));
        }
    }

    public abstract class VbValueAssignableStatement<T> : VbStatement<T>, IVbValueAssignableStatement where T : IParseTree
    {
        public VbValueAssignableStatement(IVBScopeObject context, T @obj) : base(context, @obj)
        {
            Context = context;
            Object = @obj;
        }

        public abstract DExpression GetAssignableExpression(bool partialEvaluation = false);

        public string AssignExp(bool partialEvaluation = false)
        {
            return GetAssignableExpression(partialEvaluation)?.ToExpressionString();
        }
    }

    internal interface IVbValueAssignableStatement
    {
        DExpression Prettify(bool partialEvaluation = false);
        DExpression GetAssignableExpression(bool partialEvaluation = false);
    }
}