using System;
using System.Text;

namespace vbSparkle
{
    public class VbModuleBody : VbUserScopeObject<VBScriptParser.ModuleBodyContext>
    {

        private VBMain mainSub;

        public VbModuleBody(
            IVBScopeObject context, 
            VBScriptParser.ModuleBodyContext @object, 
            string identifier) 
            : base( 
                  context, 
                  @object, 
                  identifier)
        {
            mainSub = new VBMain(this, "<GLOBAL>");

            /* ignore:
                    - context.controlProperties()   => for VB6 / VBA only
                    - context.moduleHeader()        => for VB6 / VBA only
                    - context.moduleReferences()    => for VB6 / VBA only
                unknown:
                    - context.moduleConfig()
                    - context.moduleOptions() => Option Explicit / Implicit (not needed for forensic analysis)
                    - context.moduleAttributes()
             */

            foreach (var moduleBodyElement in @object.moduleBodyElement())
            {
                //TODO: this.AddOption(moduleBodyElement.moduleOption());
                AddDeclare(moduleBodyElement.declareStmt());

                //TODO: this.AddEvent(moduleBodyElement.eventStmt());
                AddPropertyGet(moduleBodyElement.propertyGetStmt());
                AddPropertyLet(moduleBodyElement.propertyLetStmt());
                AddPropertySet(moduleBodyElement.propertySetStmt());

                AddFunction(moduleBodyElement.functionStmt());
                AddSub(moduleBodyElement.subStmt());

                AddClass(moduleBodyElement.classStmt());
                AddEnumeration(moduleBodyElement.enumerationStmt());
                AddType(moduleBodyElement.typeStmt());

                AddGlobalCode(moduleBodyElement.moduleBlock());
            }

        }
        public bool HasMethods
        {
            get { return Subs.Count > 0 || Functions.Count > 0; }
        }

        public bool HasClasses
        {
            get { return Classes.Count > 0; }
        }

        public bool HasEnums
        {
            get { return Enums.Count > 0; }
        }

        public bool HasTypes
        {
            get { return Types.Count > 0; }
        }

        public bool HasDeclares
        {
            get { return Declares.Count > 0; }
        }

        private void AddGlobalCode(VBScriptParser.MacroIfThenElseStmtContext @object)
        {
            if (@object == null)
                return;

            mainSub.AppendBlock(@object);
        }

        private void AddGlobalCode(VBScriptParser.ModuleBlockContext @object)
        {
            if (@object == null)
                return;

            mainSub.AppendBlock(@object);
        }

        private void AddModuleOption(VBScriptParser.ModuleOptionContext @object)
        {
            if (@object == null)
                return;

            throw new NotImplementedException();
        }

        private void AddEvent(VBScriptParser.EventStmtContext @object)
        {
            if (@object == null)
                return;

            throw new NotImplementedException();
        }

        private void AddType(VBScriptParser.TypeStmtContext @object)
        {
            if (@object == null)
                return;

            var newDeclare = new VbUserType(this, @object);
            Types.Add(newDeclare.Identifier, newDeclare);
            AllObjects.Add(newDeclare.Identifier, newDeclare);
        }

        private void AddSub(VBScriptParser.SubStmtContext @object)
        {
            if (@object == null)
                return;

            var newDeclare = new VbUserSub(this, @object);
            Subs.Add(newDeclare.Identifier, newDeclare);
            AllObjects.Add(newDeclare.Identifier, newDeclare);
        }

        private void AddPropertySet(VBScriptParser.PropertySetStmtContext @object)
        {
            if (@object == null)
                return;

            var newFunc = new VbUserPropertySet(this, @object);
            PropertySets.Add(newFunc.Identifier, newFunc);
            AllObjects.Add(newFunc.Identifier, newFunc);
        }

        private void AddPropertyLet(VBScriptParser.PropertyLetStmtContext @object)
        {
            if (@object == null)
                return;

            var newFunc = new VbUserPropertyLet(this, @object);
            PropertyLets.Add(newFunc.Identifier, newFunc);
            AllObjects.Add(newFunc.Identifier, newFunc);
        }

        private void AddPropertyGet(VBScriptParser.PropertyGetStmtContext @object)
        {
            if (@object == null)
                return;

            var newFunc = new VbUserPropertyGet(this, @object);
            PropertyGets.Add(newFunc.Identifier, newFunc);
            AllObjects.Add(newFunc.Identifier, newFunc);
        }

        private void AddFunction(VBScriptParser.FunctionStmtContext @object)
        {
            if (@object == null)
                return;

            var newFunc = new VbUserFunction(this, @object);
            Functions.Add(newFunc.Identifier, newFunc);
            AllObjects.Add(newFunc.Identifier, newFunc);
        }

        private void AddEnumeration(VBScriptParser.EnumerationStmtContext @object)
        {
            if (@object == null)
                return;

            var newEnum = new VbUserEnum(this, @object);
            Enums.Add(newEnum.Identifier, newEnum);
            AllObjects.Add(newEnum.Identifier, newEnum);
        }

        private void AddDeclare(VBScriptParser.DeclareStmtContext @object)
        {
            if (@object == null)
                return;

            var newDeclare = new VbUserDeclare(this, @object);
            Declares.Add(newDeclare.Identifier, newDeclare);
            AllObjects.Add(newDeclare.Identifier, newDeclare);
        }


        private void AddClass(VBScriptParser.ClassStmtContext @object)
        {
            if (@object == null)
                return;

            var newClass = new VbUserClass(this, @object);
            Classes.Add(newClass.Identifier, newClass);
            AllObjects.Add(newClass.Identifier, newClass);
        }

        public new string Prettify(bool partialEvaluation = false)
        {
            StringBuilder code = new StringBuilder();

            if (HasDeclares)
            {
                code.AppendLine("'##### Declares #####'");
                foreach (var obj in Declares)
                    code.AppendLine(obj.Value.Exp(partialEvaluation));

                code.AppendLine();
            }

            if (HasTypes)
            {
                code.AppendLine("'##### Enums #####'");
                foreach (var obj in Enums)
                {
                    code.AppendLine(obj.Value.Exp(partialEvaluation));
                    code.AppendLine();
                }
                code.AppendLine();
            }

            if (HasTypes)
            {
                code.AppendLine("'##### Types #####'");
                foreach (var obj in Types)
                {
                    code.AppendLine(obj.Value.Exp(partialEvaluation));
                    code.AppendLine();
                }
                code.AppendLine();
            }

            if (HasClasses)
            {
                code.AppendLine("'##### Classes #####'");
                foreach (var obj in Classes)
                {
                    code.AppendLine(obj.Value.Exp(partialEvaluation));
                    code.AppendLine();
                }
                code.AppendLine();
            }

            string mainCode = mainSub.Prettify(partialEvaluation).ToExpressionString();
            if (!string.IsNullOrWhiteSpace(mainCode))
            {
                if (HasMethods || HasClasses || HasEnums || HasTypes)
                    code.AppendLine("'##### Main code #####'");

                code.AppendLine(mainCode);
                code.AppendLine();

            }

            if (HasMethods)
            {
                code.AppendLine("'##### Methods #####'");
                foreach (var obj in Subs)
                {
                    code.AppendLine(obj.Value.Exp(partialEvaluation));
                    code.AppendLine();
                }

                foreach (var obj in Functions)
                {
                    code.AppendLine(obj.Value.Exp(partialEvaluation));
                    code.AppendLine();
                }
            }

            return code.ToString();
        }
    }
}