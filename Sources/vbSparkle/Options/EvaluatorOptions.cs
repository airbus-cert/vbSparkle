namespace vbSparkle.Options
{
    public class EvaluatorOptions
    {
        public SymbolRenamingMode SymbolRenamingMode { get; set; } = SymbolRenamingMode.None;
        public JunkCodeProcessingMode JunkCodeProcessingMode { get; set; } = JunkCodeProcessingMode.Comment;

        public int IndentSpacing { get; set; } = 4;


        //TODO: public bool PerfomPartialEvaluation { get; set; } = true;
        #region Internal
        internal int ConstIdx { get; set; } = 0;
        internal int VarIdx { get; set; } = 0;
        #endregion

    }
}
