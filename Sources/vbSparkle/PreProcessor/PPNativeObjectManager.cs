using System.Collections.Generic;
using vbSparkle.EvaluationObjects;

namespace vbSparkle.PreProcessor
{

    public class PPNativeObjectManager : IVBScopeObject
    {
        public static NativeObjectManager Current { get; } = new NativeObjectManager();

        public Dictionary<string, VbNativeIdentifiedObject> NativeObjects { get; private set; } =
            new Dictionary<string, VbNativeIdentifiedObject>();

        public PPNativeObjectManager()
        {
            Add(new VbNativeConstants(this, "WIN32", new DBoolExpression(true)));
            Add(new VbNativeConstants(this, "WIN64", new DBoolExpression(false)));
            Add(new VbNativeConstants(this, "VB6",   new DBoolExpression(true)));
            Add(new VbNativeConstants(this, "VB7",   new DBoolExpression(false)));

            //// Strings
            //Add(new NativeMethods.VB_Chr(this));
            //Add(new NativeMethods.VB_ChrW(this));
            //Add(new NativeMethods.VB_ChrB(this));
            //Add(new NativeMethods.VB_Asc(this));
            //Add(new NativeMethods.VB_AscW(this));
            //Add(new NativeMethods.VB_AscB(this));

            //Add(new NativeMethods.VB_MonitoringFunction(this, "Chr$"));// TODO
            ////Add(new NativeMethods.VB_MonitoringFunction(this, "ChrB"));// TODO
            //Add(new NativeMethods.VB_MonitoringFunction(this, "ChrB$"));// TODO
            //Add(new NativeMethods.VB_MonitoringFunction(this, "ChrW$"));// TODO
            //Add(new NativeMethods.VB_MonitoringFunction(this, "Filter"));// TODO
            //Add(new NativeMethods.VB_MonitoringFunction(this, "Format"));// TODO
            //Add(new NativeMethods.VB_MonitoringFunction(this, "Format$"));// TODO
            //Add(new NativeMethods.VB_MonitoringFunction(this, "FormatCurrency"));// TODO
            //Add(new NativeMethods.VB_MonitoringFunction(this, "FormatDateTime"));// TODO
            //Add(new NativeMethods.VB_MonitoringFunction(this, "FormatNumber"));// TODO
            //Add(new NativeMethods.VB_MonitoringFunction(this, "FormatPercent"));// TODO
            //Add(new NativeMethods.VB_MonitoringFunction(this, "InStr"));// TODO
            //Add(new NativeMethods.VB_MonitoringFunction(this, "InStrB"));// TODO
            //Add(new NativeMethods.VB_MonitoringFunction(this, "InStrRev"));// TODO
            //Add(new NativeMethods.VB_MonitoringFunction(this, "Join"));// TODO
            //Add(new NativeMethods.VB_MonitoringFunction(this, "LCase"));// TODO
            //Add(new NativeMethods.VB_MonitoringFunction(this, "LCase$"));// TODO
            //Add(new NativeMethods.VB_MonitoringFunction(this, "Left"));// TODO
            //Add(new NativeMethods.VB_MonitoringFunction(this, "Left$"));// TODO
            //Add(new NativeMethods.VB_MonitoringFunction(this, "LeftB"));// TODO
            //Add(new NativeMethods.VB_MonitoringFunction(this, "LeftB$"));// TODO
            //Add(new NativeMethods.VB_MonitoringFunction(this, "Len"));// TODO
            //Add(new NativeMethods.VB_MonitoringFunction(this, "LenB"));// TODO
            //Add(new NativeMethods.VB_MonitoringFunction(this, "LTrim"));// TODO
            //Add(new NativeMethods.VB_MonitoringFunction(this, "LTrim$"));// TODO
            //Add(new NativeMethods.VB_MonitoringFunction(this, "Mid"));// TODO
            //Add(new NativeMethods.VB_MonitoringFunction(this, "Mid$"));// TODO
            //Add(new NativeMethods.VB_MonitoringFunction(this, "MidB"));// TODO
            //Add(new NativeMethods.VB_MonitoringFunction(this, "MidB$"));// TODO
            //Add(new NativeMethods.VB_MonitoringFunction(this, "Replace"));// TODO
            //Add(new NativeMethods.VB_MonitoringFunction(this, "Right"));// TODO
            //Add(new NativeMethods.VB_MonitoringFunction(this, "Right$"));// TODO
            //Add(new NativeMethods.VB_MonitoringFunction(this, "RightB"));// TODO
            //Add(new NativeMethods.VB_MonitoringFunction(this, "RightB$"));// TODO
            //Add(new NativeMethods.VB_MonitoringFunction(this, "RTrim"));// TODO
            //Add(new NativeMethods.VB_MonitoringFunction(this, "RTrim$"));// TODO

            //Add(new NativeMethods.VB_Space_S(this));
            //Add(new NativeMethods.VB_Space(this));

            //Add(new NativeMethods.VB_MonitoringFunction(this, "Split"));
            //Add(new NativeMethods.VB_MonitoringFunction(this, "StrComp"));
            //Add(new NativeMethods.VB_MonitoringFunction(this, "StrConv"));
            //Add(new NativeMethods.VB_MonitoringFunction(this, "String"));
            //Add(new NativeMethods.VB_MonitoringFunction(this, "String$"));
            ////Add(new NativeMethods.VB_MonitoringFunction(this, "StrReverse"));
            //Add(new NativeMethods.VB_StrReverse(this));
            ////Add(new NativeMethods.VB_MonitoringFunction(this, "Trim"));
            //Add(new NativeMethods.VB_Trim(this));
            //Add(new NativeMethods.VB_MonitoringFunction(this, "Trim$"));// TODO
            //Add(new NativeMethods.VB_MonitoringFunction(this, "UCase"));// TODO
            //Add(new NativeMethods.VB_MonitoringFunction(this, "UCase$"));// TODO

            //// Math
            //Add(new NativeMethods.VB_Abs(this));
            //Add(new NativeMethods.VB_Atn(this));
            //Add(new NativeMethods.VB_Cos(this));
            //Add(new NativeMethods.VB_Exp(this));
            //Add(new NativeMethods.VB_Log(this));
            //Add(new NativeMethods.VB_MonitoringFunction(this, "Randomize"));
            //Add(new NativeMethods.VB_Rnd(this));
            //Add(new NativeMethods.VB_Round(this));
            //Add(new NativeMethods.VB_Sgn(this));
            //Add(new NativeMethods.VB_Sin(this));
            //Add(new NativeMethods.VB_Sqr(this));
            //Add(new NativeMethods.VB_Tan(this));
            ////Add(new NativeMethods.VB_Execute(this));

            //// FileSystem
            //Add(new NativeMethods.VB_MonitoringFunction(this, "CurDir"));
            //Add(new NativeMethods.VB_MonitoringFunction(this, "CurDir$"));
            //Add(new NativeMethods.VB_MonitoringFunction(this, "Dir"));
            //Add(new NativeMethods.VB_MonitoringFunction(this, "EOF"));
            //Add(new NativeMethods.VB_MonitoringFunction(this, "FileAttr"));
            //Add(new NativeMethods.VB_MonitoringFunction(this, "FileCopy"));
            //Add(new NativeMethods.VB_MonitoringFunction(this, "FileDateTime"));
            //Add(new NativeMethods.VB_MonitoringFunction(this, "FileLen"));
            //Add(new NativeMethods.VB_MonitoringFunction(this, "FreeFile"));
            //Add(new NativeMethods.VB_MonitoringFunction(this, "GetAttr"));
            //Add(new NativeMethods.VB_MonitoringFunction(this, "Kill"));
            //Add(new NativeMethods.VB_MonitoringFunction(this, "Loc"));
            //Add(new NativeMethods.VB_MonitoringFunction(this, "LOF"));
            //Add(new NativeMethods.VB_MonitoringFunction(this, "MkDir"));
            //Add(new NativeMethods.VB_MonitoringFunction(this, "Reset"));
            //Add(new NativeMethods.VB_MonitoringFunction(this, "RmDir"));
            //Add(new NativeMethods.VB_MonitoringFunction(this, "Seek"));
            //Add(new NativeMethods.VB_MonitoringFunction(this, "SetAttr"));

            //// Interaction
            //Add(new NativeMethods.VB_MonitoringFunction(this, "AppActivate"));
            //Add(new NativeMethods.VB_MonitoringFunction(this, "Beep"));
            //Add(new NativeMethods.VB_MonitoringFunction(this, "CallByName"));
            //Add(new NativeMethods.VB_MonitoringFunction(this, "Choose"));
            //Add(new NativeMethods.VB_MonitoringFunction(this, "Command"));
            //Add(new NativeMethods.VB_MonitoringFunction(this, "Command$"));
            //Add(new NativeMethods.VB_MonitoringFunction(this, "CreateObject"));
            //Add(new NativeMethods.VB_MonitoringFunction(this, "DeleteSetting"));
            //Add(new NativeMethods.VB_MonitoringFunction(this, "DoEvents"));
            //Add(new NativeMethods.VB_MonitoringFunction(this, "Environ"));
            //Add(new NativeMethods.VB_MonitoringFunction(this, "Environ$"));
            //Add(new NativeMethods.VB_MonitoringFunction(this, "Execute"));
            //Add(new NativeMethods.VB_MonitoringFunction(this, "GetAllSettings"));
            //Add(new NativeMethods.VB_MonitoringFunction(this, "GetObject"));
            //Add(new NativeMethods.VB_MonitoringFunction(this, "GetSetting"));
            //Add(new NativeMethods.VB_MonitoringFunction(this, "IIf"));// TODO
            //Add(new NativeMethods.VB_MonitoringFunction(this, "InputBox"));
            //Add(new NativeMethods.VB_MonitoringFunction(this, "MsgBox"));
            //Add(new NativeMethods.VB_MonitoringFunction(this, "Partition"));
            //Add(new NativeMethods.VB_MonitoringFunction(this, "SaveSetting"));
            //Add(new NativeMethods.VB_MonitoringFunction(this, "SendKeys"));
            //Add(new NativeMethods.VB_MonitoringFunction(this, "Shell"));
            //Add(new NativeMethods.VB_MonitoringFunction(this, "Switch"));// TODO

            //// Information
            //Add(new NativeMethods.VB_MonitoringFunction(this, "Err"));
            //Add(new NativeMethods.VB_MonitoringFunction(this, "IMEStatus"));
            //Add(new NativeMethods.VB_MonitoringFunction(this, "IsArray"));
            //Add(new NativeMethods.VB_MonitoringFunction(this, "IsDate"));
            //Add(new NativeMethods.VB_MonitoringFunction(this, "IsEmpty"));
            //Add(new NativeMethods.VB_MonitoringFunction(this, "IsError"));
            //Add(new NativeMethods.VB_MonitoringFunction(this, "IsMissing"));
            //Add(new NativeMethods.VB_MonitoringFunction(this, "IsNull"));
            //Add(new NativeMethods.VB_MonitoringFunction(this, "IsNumeric"));
            //Add(new NativeMethods.VB_MonitoringFunction(this, "IsObject"));
            //Add(new NativeMethods.VB_MonitoringFunction(this, "QBColor"));
            //Add(new NativeMethods.VB_MonitoringFunction(this, "RGB"));// TODO
            //Add(new NativeMethods.VB_MonitoringFunction(this, "TypeName"));
            //Add(new NativeMethods.VB_MonitoringFunction(this, "VarType"));

            //// Financial
            //Add(new NativeMethods.VB_MonitoringFunction(this, "DDB"));// TODO
            //Add(new NativeMethods.VB_MonitoringFunction(this, "FV"));// TODO
            //Add(new NativeMethods.VB_MonitoringFunction(this, "IPmt"));// TODO
            //Add(new NativeMethods.VB_MonitoringFunction(this, "IRR"));// TODO
            //Add(new NativeMethods.VB_MonitoringFunction(this, "MIRR"));// TODO
            //Add(new NativeMethods.VB_MonitoringFunction(this, "NPer"));// TODO
            //Add(new NativeMethods.VB_MonitoringFunction(this, "NPV"));// TODO
            //Add(new NativeMethods.VB_MonitoringFunction(this, "Pmt"));// TODO
            //Add(new NativeMethods.VB_MonitoringFunction(this, "PPmt"));// TODO
            //Add(new NativeMethods.VB_MonitoringFunction(this, "PV"));// TODO
            //Add(new NativeMethods.VB_MonitoringFunction(this, "Rate"));// TODO
            //Add(new NativeMethods.VB_MonitoringFunction(this, "SLN"));// TODO
            //Add(new NativeMethods.VB_MonitoringFunction(this, "SYD"));// TODO

            //// Arrays
            //Add(new NativeMethods.VB_MonitoringFunction(this, "Array"));// TODO
            //Add(new NativeMethods.VB_MonitoringFunction(this, "LBound"));// TODO
            //Add(new NativeMethods.VB_MonitoringFunction(this, "UBound"));// TODO

            //// DateTime

            //Add(new NativeMethods.VB_Time(this));
            //Add(new NativeMethods.VB_MonitoringFunction(this, "DateAdd"));// TODO
            //Add(new NativeMethods.VB_MonitoringFunction(this, "DateDiff"));// TODO
            //Add(new NativeMethods.VB_MonitoringFunction(this, "DatePart"));// TODO
            //Add(new NativeMethods.VB_MonitoringFunction(this, "DateSerial"));// TODO
            //Add(new NativeMethods.VB_MonitoringFunction(this, "DateValue"));// TODO
            //Add(new NativeMethods.VB_MonitoringFunction(this, "Day"));
            //Add(new NativeMethods.VB_MonitoringFunction(this, "Hour"));
            //Add(new NativeMethods.VB_MonitoringFunction(this, "Minute"));
            //Add(new NativeMethods.VB_MonitoringFunction(this, "Month"));
            //Add(new NativeMethods.VB_MonitoringFunction(this, "Second"));
            //Add(new NativeMethods.VB_MonitoringFunction(this, "TimeSerial"));
            //Add(new NativeMethods.VB_MonitoringFunction(this, "TimeValue"));
            //Add(new NativeMethods.VB_MonitoringFunction(this, "WeekDay"));
            //Add(new NativeMethods.VB_MonitoringFunction(this, "Year"));
            //Add(new NativeMethods.VB_MonitoringFunction(this, "WeekdayName"));
            //Add(new NativeMethods.VB_MonitoringFunction(this, "MonthName"));

            // Conversion
            Add(new NativeMethods.VB_MonitoringFunction(this, "CBool"));// TODO
            Add(new NativeMethods.VB_MonitoringFunction(this, "CByte"));// TODO
            Add(new NativeMethods.VB_MonitoringFunction(this, "CCur"));// TODO
            Add(new NativeMethods.VB_MonitoringFunction(this, "CDate"));// TODO
            Add(new NativeMethods.VB_MonitoringFunction(this, "CDbl"));// TODO
            Add(new NativeMethods.VB_MonitoringFunction(this, "CDec"));// TODO
            Add(new NativeMethods.VB_CInt(this));
            Add(new NativeMethods.VB_CLng(this));
            Add(new NativeMethods.VB_MonitoringFunction(this, "CLngLng"));          // TODO
            Add(new NativeMethods.VB_MonitoringFunction(this, "CLngPtr"));          // TODO
            Add(new NativeMethods.VB_MonitoringFunction(this, "CSng"));             // TODO
            Add(new NativeMethods.VB_MonitoringFunction(this, "CStr"));// TODO
            Add(new NativeMethods.VB_MonitoringFunction(this, "CVar"));// TODO
            Add(new NativeMethods.VB_MonitoringFunction(this, "CVDate"));// TODO
            Add(new NativeMethods.VB_MonitoringFunction(this, "CVErr"));// TODO
            //Add(new NativeMethods.VB_MonitoringFunction(this, "Error"));
            //Add(new NativeMethods.VB_MonitoringFunction(this, "Error$"));
            //Add(new NativeMethods.VB_MonitoringFunction(this, "Fix"));         // TODO
            //Add(new NativeMethods.VB_Hex(this));
            //Add(new NativeMethods.VB_Hex_S(this));
            //Add(new NativeMethods.VB_MonitoringFunction(this, "Int"));         // TODO
            //Add(new NativeMethods.VB_MonitoringFunction(this, "Oct"));         // TODO
            //Add(new NativeMethods.VB_MonitoringFunction(this, "Oct$"));         // TODO
            //Add(new NativeMethods.VB_MonitoringFunction(this, "Str"));         // TODO
            //Add(new NativeMethods.VB_MonitoringFunction(this, "Str$"));         // TODO
            //Add(new NativeMethods.VB_Val(this));

            //// Arrays


            //// Specials
            //Add(new NativeMethods.VB_Eval(this));
        }


        private VbNativeFunction Add(VbNativeFunction wrapper)
        {
            var res = wrapper;
            NativeObjects.Add(wrapper.Identifier.ToUpper(), res);
            return res;
        }
        private VbNativeConstants Add(VbNativeConstants wrapper)
        {
            var res = wrapper;
            NativeObjects.Add(wrapper.Identifier.ToUpper(), res);
            return res;
        }

        public VbIdentifiedObject GetIdentifiedObject(string identifier)
        {
            if (NativeObjects.ContainsKey(identifier.ToUpper()))
            {
                return NativeObjects[identifier.ToUpper()];
            }

            return new VbNativeConstants(this, identifier, new DBoolExpression(false));
        }

        public void DeclareVariable(VbUserVariable variable)
        {
            throw new System.NotImplementedException();
        }
        public void SetVarValue(string dest, DExpression value)
        {
            NativeObjects[dest.ToUpper()] = new VbNativeConstants(this, dest, value);
        }

        public void DeclareConstant(VbSubConstStatement vbSubConstStatement)
        {
            throw new System.NotImplementedException();
        }


        //public bool TryGet(string identifier, VbNativeIdentifiedObject nativeObject)
        //{
        //    if (NativeObjects.ContainsKey(identifier.ToUpper()))
        //    {
        //        nativeObject = NativeObjects[identifier.ToUpper()];
        //        return true;
        //    }

        //    return false;
        //}

    }
}
