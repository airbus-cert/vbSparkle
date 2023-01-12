using CommandLine;
using System.Collections.Generic;
using vbSparkle.Options;

namespace vbSparkle.CLI
{
    class BaseOptions
    {
        [Option('o', "output", Required = false, Default = null, HelpText = "File offset.")]
        public string Output { get; set; }

        [Option("sym-rename-mode",
          Default = SymbolRenamingMode.None,
          HelpText = "Define how symbols can be renamed.")]
        public SymbolRenamingMode SymbolRenamingMode { get; set; }

        [Option("junk-code-processing",
          Default = JunkCodeProcessingMode.Comment,
          HelpText = "Define how junk code should be processed.")]
        public JunkCodeProcessingMode JunkCodeProcessingMode { get; set; }

        [Option('i', "indent-spacing",
          Default = 4,
          HelpText = "Defines the number of spaces taken into account for the indentation of the code.")]
        public int IndentSpacing { get; set; }

    }

    class Options: BaseOptions
    {
        [Option('p', "path", Required = true, HelpText = "Path of directory or script file(s) to be deobfuscated.")]
        public IEnumerable<string> InputFiles { get; set; }

    }
}
