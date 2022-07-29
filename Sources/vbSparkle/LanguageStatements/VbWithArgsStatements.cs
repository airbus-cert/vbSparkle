using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using System.Collections.Generic;
using System.Linq;
using static vbSparkle.VBScriptParser;

namespace vbSparkle
{
    public class VbWithArgsStatements : VbStatement<IParseTree>
    {
        public string Name { get; set; }
        public VBValueStatement[] Arguments { get; set; }

        public VbWithArgsStatements(IVBScopeObject context, IParseTree bloc, string statementName)
            : base(context, bloc)
        {
            Name = statementName;

            List<VBValueStatement> vList = new List<VBValueStatement>();

            for (int i = 1; i < bloc.ChildCount; i++)
            {
                var child = bloc.GetChild(i);
                
                if (child is ValueStmtContext)
                {
                    vList.Add(VBValueStatement.Get(context, child as ValueStmtContext));
                }
            }

            Arguments = vList.ToArray();
        }

        public override DExpression Prettify(bool partialEvaluation = false)
        {
            return new DCodeBlock($"{Name} {string.Join(", ", Arguments.Select(v => v.Exp(partialEvaluation)))}");
        }
    }

}