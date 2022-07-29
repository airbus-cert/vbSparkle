using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using System.Collections.Generic;
using System.Linq;
using static vbSparkle.VBScriptParser;

namespace vbSparkle
{
    public class VbUnlockStatements : VbStatement<UnlockStmtContext>
    {
        public string Name { get; set; }
        public VBValueStatement[] Arguments { get; set; }

        public VbUnlockStatements(IVBScopeObject context, UnlockStmtContext bloc)
            : base(context, bloc)
        {
            Name = "Unlock";

            List<VBValueStatement> vList = new List<VBValueStatement>();

            foreach (var v in bloc.valueStmt())
                vList.Add(VBValueStatement.Get(context, v));

            Arguments = vList.ToArray();
        }

        public override DExpression Prettify(bool partialEvaluation = false)
        {
            string ret = $"{Name} {Arguments[0].Exp(partialEvaluation)}";

            if (Arguments.Length > 1)
                ret = $"{ret}, {Arguments[1].Exp(partialEvaluation)}";

            if (Arguments.Length > 2)
                ret = $"{ret} To {Arguments[2].Exp(partialEvaluation)}";

            return new DCodeBlock(ret);
        }
    }
    public class VbLockStatements : VbStatement<LockStmtContext>
    {
        public string Name { get; set; }
        public VBValueStatement[] Arguments { get; set; }

        public VbLockStatements(IVBScopeObject context, LockStmtContext bloc)
            : base(context, bloc)
        {
            Name = "Lock";

            List<VBValueStatement> vList = new List<VBValueStatement>();

            foreach (var v in bloc.valueStmt())
                vList.Add(VBValueStatement.Get(context, v));

            Arguments = vList.ToArray();
        }

        public override DExpression Prettify(bool partialEvaluation = false)
        {
            string ret = $"{Name} {Arguments[0].Exp(partialEvaluation)}";

            if (Arguments.Length > 1)
                ret = $"{ret}, {Arguments[1].Exp(partialEvaluation)}";

            if (Arguments.Length > 2)
                ret = $"{ret} To {Arguments[2].Exp(partialEvaluation)}";

            return new DCodeBlock(ret);
        }
    }

}