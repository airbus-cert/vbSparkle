using System;
using System.Diagnostics.Contracts;
using System.Reflection;
using Antlr4.Runtime.Tree;
using MathNet.Symbolics;

namespace vbSparkle
{
    public abstract class DMathExpression : DExpression
    {
        protected DMathExpression()
        {
        }

        public SymbolicExpression MathObject { get; set; } = 0;
        public abstract object GetValueObject();
    }
}