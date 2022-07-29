using System;
using System.Collections.Generic;
using System.Linq;
using vbSparkle.EvaluationObjects;

namespace vbSparkle
{

    public class VbSubConstStatement : VbUserIdentifiedObject<VBScriptParser.ConstSubStmtContext>
    {
        public string VType { get; }
        public string TypeHInt { get; }
        public VBValueStatement ValueStatement { get; }
        public DExpression Value { get; internal set; }

        public VbSubConstStatement(IVBScopeObject context, VBScriptParser.ConstSubStmtContext bloc)
            : base(context, bloc, bloc.ambiguousIdentifier().GetText())
        {
            VType = bloc.asTypeClause()?.type()?.GetText();
            TypeHInt = bloc.typeHint()?.GetText();
            ValueStatement = VBValueStatement.Get(context, bloc.valueStmt());
            Value = ValueStatement.Evaluate();

            context.DeclareConstant(this);
        }

        public VbIdentifiedObject IdentifiedObject
        {
            get { return Context.GetIdentifiedObject(Identifier); }
        }


        public override DExpression Prettify(bool partialEvaluation = false)
        {
            string asType = string.Empty;
            if (!string.IsNullOrEmpty(VType))
            {
                asType = $" As {VType}";
            }

            return new DCodeBlock($"{IdentifiedObject.Name}{TypeHInt}{asType} = {ValueStatement.Exp(partialEvaluation)}");
        }

        public DExpression TryEvaluate()
        {
            if (Value != null && !(Value is DCodeBlock))
            {
                return Value;
            }

            return new DCodeBlock(Identifier);
        }
    }

    public class VbConstStatement : VbStatement<VBScriptParser.ConstStmtContext>
    {
        public ConstVisibility Visibility { get; set; } = ConstVisibility.Undefined;
        public List<VbSubConstStatement> SubStatements { get; set; } = new List<VbSubConstStatement>();

        public VbConstStatement(IVBScopeObject context, VBScriptParser.ConstStmtContext bloc)
            : base(context, bloc)
        {
            var subStmts = bloc.constSubStmt();
            var visibility = bloc.publicPrivateGlobalVisibility()?.GetText()?.ToLower();

            if (!string.IsNullOrEmpty(visibility))
                Visibility = Enum.GetValues(typeof(ConstVisibility)).OfType<ConstVisibility>().FirstOrDefault(q => q.ToString().ToLower() == visibility);
            

            foreach (var subStmt in subStmts)
            {
                SubStatements.Add(new VbSubConstStatement(context, subStmt));
            }
        }

        public override DExpression Prettify(bool partialEvaluation)
        {
            string codeBlock = Visibility == ConstVisibility.Undefined 
                ? "Const " 
                : $"{Visibility} Const ";

            codeBlock = $"{codeBlock}{string.Join(", ", SubStatements.Select(v => v.Exp(partialEvaluation)))}";

            return new DCodeBlock(codeBlock);
        }
    }


}