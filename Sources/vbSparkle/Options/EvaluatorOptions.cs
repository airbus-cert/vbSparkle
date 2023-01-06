namespace vbSparkle.Options
{
    public class EvaluatorOptions
    {
        public int ConstIdx { get; set; }
        public int VarIdx { get; set; }

        public bool PerfomPartialEvaluation { get; set; } = true;
        public JunkCodeProcessingMode JunkCodeProcessingMode { get; set; } = JunkCodeProcessingMode.Comment;

        public int IndentSpacing { get; set; } = 4;

        public SymbolRenamingMode SymbolRenamingMode { get; set; } = SymbolRenamingMode.None;
    }
}
