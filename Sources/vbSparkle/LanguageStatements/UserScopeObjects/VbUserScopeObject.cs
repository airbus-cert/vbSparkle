using System;
using Antlr4.Runtime.Tree;
using System.Collections.Generic;

namespace vbSparkle
{

    public abstract class VbUserScopeObject<T> : VbUserIdentifiedObject<T>, IVBScopeObject
        where T : IParseTree
    {

        public Dictionary<string, VbIdentifiedObject> AllObjects = new Dictionary<string, VbIdentifiedObject>();

        public Dictionary<string, VbUserAttribute> Attributes = new Dictionary<string, VbUserAttribute>();
        public Dictionary<string, VbUserClass> Classes = new Dictionary<string, VbUserClass>();
        public Dictionary<string, VbUserEnum> Enums = new Dictionary<string, VbUserEnum>();
        public Dictionary<string, VbUserType> Types = new Dictionary<string, VbUserType>();
        public Dictionary<string, VbUserDeclare> Declares = new Dictionary<string, VbUserDeclare>();

        public Dictionary<string, VbUserSub> Subs = new Dictionary<string, VbUserSub>();
        public Dictionary<string, VbUserFunction> Functions = new Dictionary<string, VbUserFunction>();
        public Dictionary<string, VbUserPropertyGet> PropertyGets = new Dictionary<string, VbUserPropertyGet>();
        public Dictionary<string, VbUserPropertyLet> PropertyLets = new Dictionary<string, VbUserPropertyLet>();
        public Dictionary<string, VbUserPropertySet> PropertySets = new Dictionary<string, VbUserPropertySet>();

        public Dictionary<string, VbUserVariable> Variables = new Dictionary<string, VbUserVariable>();
        public Dictionary<string, VbSubConstStatement> Constants = new Dictionary<string, VbSubConstStatement>();

        public VbUserScopeObject(
            IVBScopeObject context, 
            T @object, 
            string identifier) 
            : base(context, @object, identifier)
        {
        }

        public EvaluatorOptions _options = null;
        public EvaluatorOptions Options
        {
            get
            {
                if (_options != null)
                {
                    return _options;
                }
                else
                {
                    if (Context?.Options != null)
                    {
                        return Context.Options;
                    }
                }
                return _options = new EvaluatorOptions();
            }
            set
            {
                _options = value;
            }
        }

        public void DeclareConstant(VbSubConstStatement constStatement)
        {
            string id = constStatement.Identifier.ToUpper();
            Constants[id] = constStatement;
            AllObjects[id] = constStatement;
        }

        public void DeclareVariable(VbUserVariable variable)
        {
            string id = variable.Identifier.ToUpper();
            Variables[id] = variable;
            AllObjects[id] = variable;
        }


        public virtual VbIdentifiedObject GetIdentifiedObject(string identifier)
        {
            string identifierKey = identifier.ToUpper();
            VbIdentifiedObject obj1 = null;

            if (AllObjects.TryGetValue(identifierKey, out obj1))
            {
                return obj1;
            }

            var nativeValue = NativeObjectManager.Current.GetIdentifiedObject(identifierKey);

            if (nativeValue != null)
                return nativeValue;

            if (Context != null)
                return Context.GetIdentifiedObject(identifierKey);

            VbUnknownIdentifiedObject var = new VbUnknownIdentifiedObject(Context, identifier); ;

            //Variables.Add(var.Identifier, var);
            AllObjects.Add(var.Identifier.ToUpper(), var);

            return var;

            //if (Variables.ContainsKey(identifierKey))
            //    return Variables[identifierKey];


            //return new VbUnknownIdentifiedObject(Context, identifier);
            ////throw new MissingMemberException(this.GetType().Name, identifier);
        }

        public void SetVarValue(string dest, DExpression value)
        {
            VbIdentifiedObject dstObj = GetIdentifiedObject(dest);
            
            if (dstObj is VbUnknownIdentifiedObject)
            {
                var varObj = dstObj as VbUnknownIdentifiedObject;
                varObj.CurrentValue = value;
                varObj.WriteAccess += 1;
            }

            if (dstObj is VbUserVariable)
            {
                var varObj = dstObj as VbUserVariable;
                varObj.CurrentValue = value;
                varObj.WriteAccess += 1;
            }

            if (dstObj is VbSubConstStatement)
            {
                var varObj = dstObj as VbSubConstStatement;
                varObj.Value = value;
            }
        }
    }
}