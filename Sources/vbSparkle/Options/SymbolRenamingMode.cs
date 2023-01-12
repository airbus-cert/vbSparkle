using System;

namespace vbSparkle.Options
{
    [Flags]
    public enum SymbolRenamingMode
    {
        None,
        Variables,
        Constants,
        //TODO: PublicMembers, 
        //TODO: PrivateMembers,
        //TODO: Members = PublicMembers | PrivateMembers,
        All = Variables | Constants, // TODO:  | Members,
        //TODO: AutoDetectObfuscatedSymbols,

    }
}
