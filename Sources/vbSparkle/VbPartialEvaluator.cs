using Antlr4.Runtime;
using System.Linq;

namespace vbSparkle
{
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
        public static string PrettifyEncoded(string script)
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
                    result = Prettify(result);

                    script = script.Remove(start, len).Insert(start, result);

                }
                while (HasEncodedScript(script, VBE_SIG_START, VBE_SIG_END));
            } 
            else
            {
                return Prettify(script);
            }

            return script;
        }

        /// <summary>
        /// Deobfuscate & prettify a clear text VBA / VBS script.
        /// </summary>
        /// <param name="script">Clear text VBA / VBS script</param>
        /// <returns>Deobfuscated & prettified script.</returns>
        private static string Prettify(string script)
        {
            string processedScript = PreProcessScript(script);

            AntlrInputStream antlrStream = new AntlrInputStream(processedScript);

            VBScriptLexer lexer = new VBScriptLexer(antlrStream);

            CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);

            VBScriptParser parser = new VBScriptParser(commonTokenStream);

            VBScriptParser.StartRuleContext context = parser.startRule();

            VbAnalyser analyser = new VbAnalyser();
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
        private static string PreProcessScript(string script)
        {
            AntlrInputStream inputStream = new AntlrInputStream(script);

            VBPreprocessorsLexer preprocessorLexer = new VBPreprocessorsLexer(inputStream);

            CommonTokenStream preprocessorCommonTokenStream = new CommonTokenStream(preprocessorLexer);
            VBPreprocessorsParser preprocessorParser = new VBPreprocessorsParser(preprocessorCommonTokenStream);

            VBPreprocessorsParser.StartRuleContext preproStartContext = preprocessorParser.startRule();
            VbPreprocessorAnalyser preAnalyser = new VbPreprocessorAnalyser();

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
