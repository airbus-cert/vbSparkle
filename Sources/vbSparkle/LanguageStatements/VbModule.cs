using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace vbSparkle
{

    public class VbModule : VbUserScopeObject<VBScriptParser.ModuleContext>
    {
        ///// <summary>
        ///// Evaluators are :
        /////     - variables
        /////     - property get
        ///// </summary>
        ///// <param name="identifier"></param>
        ///// <returns></returns>
        //public VbIdentifiedObject GetIdentifiedEvaluators(string identifier)
        //{
        //    VbIdentifiedObject obj1 = null;
        //    if (AllObjects.TryGetValue(identifier, out obj1))
        //    {
        //        if (obj1 is VbUserPropertyGet || obj1 is VbUserVariable)
        //            return obj1;

        //        throw new Exception($"Invalid symbol type: {identifier} : {obj1.GetType().Name}");
        //    }

        //    VbUserVariable var = new VbUserVariable(this, null, identifier);

        //    Variables.Add(var.Identifier, var);
        //    AllObjects.Add(var.Identifier, var);

        //    return var;
        //}

        ///// <summary>
        ///// </summary>
        ///// <param name="identifier"></param>
        ///// <returns></returns>
        //public override VbIdentifiedObject GetIdentifiedObject(string identifier)
        //{

        //}
        public VbModuleBody Body { get; set; }

        public VbModule(
            VBScriptParser.ModuleContext @object)
            : base(
                null,
                @object,
                "<Module>")
        {

            if (@object.moduleAttributes()?.attributeStmt() != null)
                foreach (var attr in @object.moduleAttributes()?.attributeStmt())
                    AddAttributes(attr);

            var body = @object.moduleBody();

            if (body != null)
                Body = new VbModuleBody(this, body, Identifier);
        }

        public new string Prettify(bool partialEvaluation = false)
        {
            StringBuilder code = new StringBuilder();

            if (HasAttributes)
            {
                code.AppendLine("'##### Attributes #####'");

                foreach (var obj in Attributes)
                    code.AppendLine(obj.Value.Exp(partialEvaluation));

                code.AppendLine();
            }

            if (Body != null)
                code.AppendLine(Body.Prettify(partialEvaluation));

            return code.ToString();
        }

        private void AddAttributes(VBScriptParser.AttributeStmtContext @object)
        {
            if (@object == null)
                return;

            var newAttr = new VbUserAttribute(this, @object);
            Attributes.Add(newAttr.Identifier, newAttr);
            AllObjects.Add(newAttr.Identifier, newAttr);
        }

        public bool HasAttributes
        {
            get { return Attributes.Count > 0; }
        }

    }
}