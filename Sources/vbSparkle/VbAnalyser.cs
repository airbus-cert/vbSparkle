
using System;
using System.Collections.Generic;

namespace vbSparkle
{
    public class VbAnalyser
    {
        public List<VbModule> Modules { get; set; } = new List<VbModule>();

        internal void Visit(VBScriptParser.StartRuleContext stContext)
        {
            var moduleContext = stContext.module();

            VbModule module = new VbModule(moduleContext);
            Modules.Add(module);
        }
    }
}