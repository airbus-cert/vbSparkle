using Antlr4.Runtime.Tree;

namespace vbSparkle
{
    public interface IVbIfBlockStmt
    {
        VbIfConditionStmtContext CondValue { get; set; }
    }

}