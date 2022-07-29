using System.Collections.Generic;
using System.Linq;
using vbSparkle.EvaluationObjects;

namespace vbSparkle
{

    public class VbVariableStatement : VbStatement<VBScriptParser.VariableStmtContext>
    {
        public bool IsStatic { get; set; } = false;
        public bool WithEvents { get; set; } = false;
        public VbVariableListStatement VarList { get; set; }

        public VbVariableStatement(IVBScopeObject context, VBScriptParser.VariableStmtContext bloc)
            : base(context, bloc)
        {
            if (bloc.WITHEVENTS() != null)
                WithEvents = true;

            if (bloc.STATIC() != null)
                IsStatic = true;

            VarList = new VbVariableListStatement(context, bloc.variableListStmt());

            foreach (var variable in VarList.VarList)
            {
                variable.IsStatic = IsStatic;
                variable.WithEvents = WithEvents;

                context.DeclareVariable(variable);
            }
        }

        public override DExpression Prettify(bool partialEvaluation)
        {
            string codeBlock;

            if (IsStatic)
                codeBlock = "Static ";
            else
                codeBlock = "Dim ";

            if (WithEvents)
                codeBlock += "WithEvents ";

            codeBlock += VarList.Exp(partialEvaluation);

            return new DCodeBlock(codeBlock);
        }
    }



    public class VbVariableListStatement : VbStatement<VBScriptParser.VariableListStmtContext>
    {
        public List<VbUserVariable> VarList { get; set; } = new List<VbUserVariable>();

        public VbVariableListStatement(IVBScopeObject context, VBScriptParser.VariableListStmtContext bloc)
            : base(context, bloc)
        {
            foreach (var v in bloc.variableSubStmt())
            {
                VarList.Add(new VbUserVariable(context, v));
            }
        }

        public override DExpression Prettify(bool partialEvaluation)
        {
            string declarations = string.Join(", ", VarList.Select(v => v.Exp(partialEvaluation)));
            return new DCodeBlock(declarations);
        }
    }



    public class VbUserVariable : VbUserIdentifiedObject<VBScriptParser.VariableSubStmtContext>
    {
        public string VType { get; set; }
        public VbSubscriptsStatement Subscripts { get; }
        public bool IsTypeDefined { get; set; }
        public bool IsArray { get; set; } = false;

        public bool IsStatic { get; set; } = false;

        public bool WithEvents { get; set; } = false;

        private DExpression value = null;

        public DExpression CurrentValue {
            get {
                if (value == null)
                    return new DCodeBlock(Identifier);
                else
                    return value;
            }
            internal set { this.value = value; }
        }
        public int WriteAccess { get; internal set; }

        public VbUserVariable(IVBScopeObject context, VBScriptParser.VariableSubStmtContext bloc)
            : base(context, bloc, bloc.ambiguousIdentifier().GetText())
        {
            VType = bloc.asTypeClause()?.type()?.GetText();

            Subscripts = new VbSubscriptsStatement(context, bloc.subscripts());

            if (string.IsNullOrWhiteSpace(VType))
            {
                IsTypeDefined = false;
                VType = "Variant";
            } else
            {
                IsTypeDefined = true;
            }

            if (bloc.LPAREN() != null)
            {
                IsArray = true;
            }

            context.DeclareVariable(this);
        }

        //public VbUserVariable(IVBScopeObject context, string identifier)
        //    : base(context, null, identifier)
        //{
        //    IsTypeDefined = false;
        //    VType = "Variant";

        //    IsArray = false;

        //    context.DeclareVariable(this);
        //}

        public VbIdentifiedObject IdentifiedObject
        {
            get { return Context.GetIdentifiedObject(Identifier); }
        }


        public override DExpression Prettify(bool partialEvaluation)
        {
            string codeBlock = IdentifiedObject.Name;

            if (IsArray)
            {
                codeBlock += $"({Subscripts.Exp(partialEvaluation)})";
            }

            if (IsTypeDefined)
                return new DCodeBlock(codeBlock + " As " + VType);
            else
                return new DCodeBlock(codeBlock);

        }


        public DExpression TryEvaluate()
        {
            if (CurrentValue != null && !(CurrentValue is DCodeBlock))
            {
                return CurrentValue;
            }

            return new DCodeBlock(Identifier);
        }
    }
}