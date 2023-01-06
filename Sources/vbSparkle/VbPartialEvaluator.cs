using Antlr4.Runtime;
using System;
using System.Linq;

namespace vbSparkle
{
    public enum JunkCodeProcessingMode
    {
        Nothing = 0,
        Remove = 1,
        Comment = 2
    }

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

    public class EvaluatorOptions
    {
        public bool PerfomPartialEvaluation { get; set; } = true;
        public JunkCodeProcessingMode JunkCodeProcessingMode { get; set; } = JunkCodeProcessingMode.Comment;

        public int IndentSpacing { get; set; } = 4;

        public SymbolRenamingMode SymbolRenamingMode { get; set; } = SymbolRenamingMode.None;
    }

    /// <summary>
    /// Visual Basic Script, Encoded, and VBA macro partial evaluator.
    /// </summary>
    public class VbPartialEvaluator
    {
        /// <summary>
        /// Decode, deobfuscate & prettify a VBA / VBS encoded or clear script.
        /// </summary>
        /// <param name="script">VBE / VBS / VBA script code.</param>
        /// <returns>Deobfuscated & prettified script.</returns>
        public static string PrettifyEncoded(string script, EvaluatorOptions options = null)
        {
            // VBE signatures
            string VBE_SIG_START = "#@~^";
            string VBE_SIG_END = "==^#~@";

            // Replace encoded-VBE script with decoded
            if (HasEncodedScript(script, VBE_SIG_START, VBE_SIG_END))
            {
                do
                {
                    int start = script.IndexOf(VBE_SIG_START);
                    int len = (script.IndexOf(VBE_SIG_END) + VBE_SIG_END.Length) - start;

                    string encodedScript = script.Substring(start, len);

                    var result = vbeDecoder.ScriptDecoder.DecodeScript(encodedScript);
                    result = Prettify(result, options);

                    script = script.Remove(start, len).Insert(start, result);

                }
                while (HasEncodedScript(script, VBE_SIG_START, VBE_SIG_END));
            } 
            else
            {
                return Prettify(script, options);
            }

            return script;
        }

        /// <summary>
        /// Deobfuscate & prettify a clear text VBA / VBS script.
        /// </summary>
        /// <param name="script">Clear text VBA / VBS script</param>
        /// <returns>Deobfuscated & prettified script.</returns>
        private static string Prettify(string script, EvaluatorOptions options = null)
        {
            string processedScript = PreProcessScript(script, options);

            AntlrInputStream antlrStream = new AntlrInputStream(processedScript);

            VBScriptLexer lexer = new VBScriptLexer(antlrStream);

            CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);

            VBScriptParser parser = new VBScriptParser(commonTokenStream);

            VBScriptParser.StartRuleContext context = parser.startRule();

            VbAnalyser analyser = new VbAnalyser(new EvaluatorOptions()
            {
                JunkCodeProcessingMode = JunkCodeProcessingMode.Remove,
                PerfomPartialEvaluation = true,
                IndentSpacing = 2
            }) ;

            analyser.Visit(context);

            var module = analyser.Modules.FirstOrDefault();

            string result = module
                .Prettify(partialEvaluation: true)
                .Trim();

            return result;
        }

        /// <summary>
        /// Pre-process VBA macro-code like "#Const" or "#If / Then / #Else / #End If" instructions.
        /// </summary>
        /// <param name="script">Clear text VBA / VBS script.</param>
        /// <returns>Deobfuscated & prettified script.</returns>
        private static string PreProcessScript(string script, EvaluatorOptions options)
        {
            AntlrInputStream inputStream = new AntlrInputStream(script);

            VBPreprocessorsLexer preprocessorLexer = new VBPreprocessorsLexer(inputStream);

            CommonTokenStream preprocessorCommonTokenStream = new CommonTokenStream(preprocessorLexer);
            VBPreprocessorsParser preprocessorParser = new VBPreprocessorsParser(preprocessorCommonTokenStream);

            VBPreprocessorsParser.StartRuleContext preproStartContext = preprocessorParser.startRule();
            VbPreprocessorAnalyser preAnalyser = new VbPreprocessorAnalyser(options);

            string result = preAnalyser.Visit(preproStartContext);
            return result;
        }

        /// <summary>
        /// Check if a script data contains VBE signatures.
        /// </summary>
        /// <param name="script"></param>
        /// <param name="VBE_SIG_START"></param>
        /// <param name="VBE_SIG_END"></param>
        /// <returns>True if contains encoded script. Otherwise it will return false.</returns>
        private static bool HasEncodedScript(string script, string VBE_SIG_START, string VBE_SIG_END)
        {
            return script.Contains(VBE_SIG_START) && script.Contains(VBE_SIG_END);
        }
    }
}
