namespace vbSparkle
{
    public class VbVisibilityStatement
    {
        private IVBScopeObject Context { get; set; }
        private VBScriptParser.VisibilityContext Object { get; set; }

        public VbVisibilityStatement(
            IVBScopeObject context, 
            VBScriptParser.VisibilityContext @object)
        {
            Context = context;
            Object = @object;
        }

        public string Prettify()
        {
            if (Object == null)
                return "";

            switch (Object.GetText().ToUpper())
            {
                case "PRIVATE":
                    return "Private";
                case "FRIEND":
                    return "Friend";
                case "GLOBAL":
                    return "Global";
                case "PUBLIC":
                    return "Public";
            }

            return "";
        }
    }
}