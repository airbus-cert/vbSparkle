using CommandLine;
using System.Collections.Generic;
using vbSparkle.Options;

namespace vbSparkle.CLI
{
    class Options
    {
        [Option('p', "path", Group = "input", HelpText = "Path of directory or script file(s) to be deobfuscated.")]
        public IEnumerable<string> InputFiles { get; set; }
        
        [Option("stdin",
          Default = false,
          Group = "input",
          HelpText = "Read from stdin")]
        public bool stdin { get; set; }

        [Option('o', "output", Required = false, Default = null, HelpText = "File offset.")]
        public string Output { get; set; }

        //[Option('v',
        //  Default = false,
        //  HelpText = "Prints all messages to standard output.")]
        //public bool Verbose { get; set; }

        [Option("sym-rename-mode",
          Default = SymbolRenamingMode.None,
          HelpText = "Define how symbols can be renamed.")]
        public SymbolRenamingMode SymbolRenamingMode { get; set; }

        [Option("junk-code-processing",
          Default = JunkCodeProcessingMode.Comment,
          HelpText = "Define junk code should be processed.")]
        public JunkCodeProcessingMode JunkCodeProcessingMode { get; set; }

        [Option('i', "indent-spacing",
          Default = 4,
          HelpText = "Defines the number of spaces taken into account for the indentation of the code.")]
        public int IndentSpacing { get; set; }

    }
}
