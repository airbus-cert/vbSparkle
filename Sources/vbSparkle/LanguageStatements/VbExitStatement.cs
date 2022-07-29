using System.Linq;

namespace vbSparkle
{
    public class VbExitStatement : VbStatement<VBScriptParser.ExitStmtContext>
    {
        public enum ExitType
        {
            EXIT_DO,
            EXIT_FOR,
            EXIT_FUNCTION,
            EXIT_PROPERTY,
            EXIT_SUB
        }

        public ExitType EType { get; set; }

        public VbExitStatement(IVBScopeObject context, VBScriptParser.ExitStmtContext bloc)
            : base(context, bloc)
        {
            if (bloc.EXIT_DO() != null)
                EType = ExitType.EXIT_DO;

            if (bloc.EXIT_FOR() != null)
                EType = ExitType.EXIT_FOR;

            if (bloc.EXIT_FUNCTION() != null)
                EType = ExitType.EXIT_FUNCTION;

            if (bloc.EXIT_PROPERTY() != null)
                EType = ExitType.EXIT_PROPERTY;

            if (bloc.EXIT_SUB() != null)
                EType = ExitType.EXIT_SUB;
        }

        public override DExpression Prettify(bool partialEvaluation = false)
        {
            switch (EType)
            {
                case ExitType.EXIT_DO:
                    return new DCodeBlock("Exit Do");
                case ExitType.EXIT_FOR:
                    return new DCodeBlock("Exit For");
                case ExitType.EXIT_FUNCTION:
                    return new DCodeBlock("Exit Function");
                case ExitType.EXIT_PROPERTY:
                    return new DCodeBlock("Exit Property");
                case ExitType.EXIT_SUB:
                    return new DCodeBlock("Exit Sub");
            }
            return new DCodeBlock($"<UNKNOWN: {Object.GetText().ToUpper()}>");
        }
    }

}