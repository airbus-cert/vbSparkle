using System.Linq;

namespace vbSparkle
{
    public class VbOpenStatement : VbStatement<VBScriptParser.OpenStmtContext>
    {
        public enum OpenMode
        {
            Append, Binary, Input, Output, Random
        }
        public enum FileAccess
        {
            Read, Write, ReadWrite
        }
        public enum FileLockType
        {
            Shared, LockRead, LockWrite, LockReadWrite
        }

        public VBValueStatement[] Values { get; }
        public VBValueStatement PathName { get; }
        public VBValueStatement FileNumber { get; }
        public VBValueStatement Length { get; } = null;
        public OpenMode Mode { get; }
        public FileAccess? Access { get; } = null;
        public FileLockType? LockType { get; } = null;

        public VbOpenStatement(IVBScopeObject context, VBScriptParser.OpenStmtContext bloc)
            : base(context, bloc)
        {
            //OPEN WS valueStmt WS FOR WS (APPEND | BINARY | INPUT | OUTPUT | RANDOM) (WS ACCESS WS (READ | WRITE | READ_WRITE))? (WS (SHARED | LOCK_READ | LOCK_WRITE | LOCK_READ_WRITE))? WS AS WS valueStmt (WS LEN WS? EQ WS? valueStmt)?
            Values = bloc.valueStmt().Select(v => VBValueStatement.Get(context, v)).ToArray();

            PathName = Values[0];
            FileNumber = Values[1];

            if (Values.Length > 2)
                Length = Values[2];

            if (bloc.APPEND() != null)
                Mode = OpenMode.Append;
            if (bloc.BINARY() != null)
                Mode = OpenMode.Binary;
            if (bloc.INPUT() != null)
                Mode = OpenMode.Input;
            if (bloc.OUTPUT() != null)
                Mode = OpenMode.Output;
            if (bloc.RANDOM() != null)
                Mode = OpenMode.Random;


            if (bloc.READ() != null)
                Access = FileAccess.Read;
            if (bloc.WRITE() != null)
                Access = FileAccess.Write;
            if (bloc.READ_WRITE() != null)
                Access = FileAccess.ReadWrite;

            if (bloc.SHARED() != null)
                LockType = FileLockType.Shared;
            if (bloc.LOCK_READ() != null)
                LockType = FileLockType.LockRead;
            if (bloc.LOCK_WRITE() != null)
                LockType = FileLockType.LockWrite;
            if (bloc.LOCK_READ_WRITE() != null)
                LockType = FileLockType.LockReadWrite;
        }

        public override DExpression Prettify(bool partialEvaluation = false)
        {
            string ret = $"Open {PathName.Exp(partialEvaluation)} For {Mode.ToString()}";

            if (Access.HasValue)
                ret += $" Access {Access.Value.ToString()}";

            if (LockType.HasValue)
                ret += $" {LockType.Value.ToString()}";

            ret += $" As {FileNumber.Exp(partialEvaluation)}";

            if (Length != null)
                ret += $" {Length.Exp(partialEvaluation)}";

            return new DCodeBlock(ret);

        }
    }

}