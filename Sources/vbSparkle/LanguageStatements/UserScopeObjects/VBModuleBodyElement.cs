namespace vbSparkle
{
    internal abstract class VbModuleBodyElement
    {
        private VbModule parent;
        private VBScriptParser.ModuleBodyElementContext moduleBodyElement;

        public VbModuleBodyElement(
            VbModule parent, 
            VBScriptParser.ModuleBodyElementContext moduleBodyElement)
        {
            this.parent = parent;
            this.moduleBodyElement = moduleBodyElement;
        }
    }
}