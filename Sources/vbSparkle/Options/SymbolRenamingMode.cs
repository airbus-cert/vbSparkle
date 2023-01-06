using System;

namespace vbSparkle.Options
{
    [Flags]
    public enum SymbolRenamingMode
    {
        None,
        Variables,
        Constants,
        PublicMembers,
        PrivateMembers,
        Members = PublicMembers | PrivateMembers,
        All = Variables | Constants | Members,
        AutoDetectObfuscatedSymbols,

    }
}
