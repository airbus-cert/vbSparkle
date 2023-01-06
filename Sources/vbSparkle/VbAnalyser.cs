
using System;
using System.Collections.Generic;

namespace vbSparkle
{
    public class VbAnalyser
    {
        public EvaluatorOptions Options { get; internal set; }

        public VbAnalyser(EvaluatorOptions options)
        {
            Options = options;
        }

        public List<VbModule> Modules { get; set; } = new List<VbModule>();

        internal void Visit(VBScriptParser.StartRuleContext stContext)
        {
            var moduleContext = stContext.module();

            VbModule module = new VbModule(Options, moduleContext);
            Modules.Add(module);
        }
    }
}