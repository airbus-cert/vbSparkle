using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vbSparkle.EvaluationObjects;

namespace vbSparkle.NativeMethods
{
    public class VB_MonitoringFunction
    : VbNativeFunction
    {
        public VB_MonitoringFunction(IVBScopeObject context, string Name)
            : base(context, Name)
        {
        }

        public override DExpression Evaluate(params DExpression[] args)
        {
            return DefaultExpression(args);
        }

    }


    public class VB_Time
        : VbNativeFunction
    {
        public VB_Time(IVBScopeObject context)
            : base(context, "Time")
        {
        }

        public override DExpression Evaluate(params DExpression[] args)
        {

            return new DCodeBlock("Time");
        }

    }

    //public class VB_CreateObject
    //    : VbNativeFunction
    //{
    //    public VB_CreateObject(IVBScopeObject context)
    //        : base(context, "CreateObject")
    //    {
    //    }

    //    public override DExpression Evaluate(params DExpression[] args)
    //    {
    //        throw new NotImplementedException();
    //    }

    //}

    //public class VB_Array
    //    : VbNativeFunction
    //{
    //    public VB_Array(IVBScopeObject context)
    //        : base(context, "Array")
    //    {
    //    }

    //    public override DExpression Evaluate(params DExpression[] args)
    //    {
    //        throw new NotImplementedException();
    //    }

    //}

    public class VB_Eval
        : VbNativeFunction
    {
        public VB_Eval(IVBScopeObject context)
            : base(context, "Eval")
        {
        }

        public override DExpression Evaluate(params DExpression[] args)
        {
            DExpression arg1 = args.FirstOrDefault();

            if (arg1.IsValuable)
            {
                return arg1;
            }

            return DefaultExpression(args);
        }

    }
    public class VB_StrReverse
    : VbNativeFunction
    {
        public VB_StrReverse(IVBScopeObject context)
            : base(context, "StrReverse")
        {
        }
        
        public override DExpression Evaluate(params DExpression[] args)
        {
            DExpression arg1 = args.FirstOrDefault();

            string strArg;

            if (!Converter.TryGetStringValue(arg1, out strArg))
                return DefaultExpression(args);
            
            string str = new string(strArg.ToCharArray().Reverse().ToArray());

            return new DSimpleStringExpression(str, Encoding.Unicode);
        }

    }

    public class VB_Replace
    : VbNativeFunction
    {
        public VB_Replace(IVBScopeObject context)
            : base(context, "Replace")
        {
        }

        public override DExpression Evaluate(params DExpression[] args)
        {
            if (args.Length < 3)
                throw new Exception();

            DExpression exp = args[0];
            DExpression findExp = args[1];
            DExpression replExp = args[2];

            string expStr;
            string findStr;
            string replStr;

            if (!Converter.TryGetStringValue(exp, out expStr))
                return DefaultExpression(args);
            if (!Converter.TryGetStringValue(findExp, out findStr))
                return DefaultExpression(args);
            if (!Converter.TryGetStringValue(replExp, out replStr))
                return DefaultExpression(args);

            if (args.Length > 3)
            {
                DExpression startExp = args[3];
            }

            if (args.Length > 4)
            {
                DExpression countExp = args[4];
            }

            if (args.Length > 5)
            {
                DExpression compareExp = args[5];
            }

            string str = findStr.Equals(replStr) ? expStr : expStr.Replace(findStr, replStr);

            return new DSimpleStringExpression(str, Encoding.Unicode);
        }

    }

    public class VB_Trim_S
     : VB_Trim
    {
        public VB_Trim_S(IVBScopeObject context)
       : base(context, "Trim$")
        {
        }
    }

    public class VB_Trim
     : VbNativeFunction
    {
        public VB_Trim(IVBScopeObject context, string name)
            : base(context, name)
        {
        }
        public VB_Trim(IVBScopeObject context)
            : base(context, "Trim")
        {
        }

        public override DExpression Evaluate(params DExpression[] args)
        {
            DExpression arg1 = args.FirstOrDefault();

            string strArg;

            if (!Converter.TryGetStringValue(arg1, out strArg))
                return DefaultExpression(args);

            string str = strArg.Trim(' ');
            return new DSimpleStringExpression(str, Encoding.Unicode);
        }

    }

    public class VB_Space_S : VB_Space
    {
        public VB_Space_S(IVBScopeObject context)
            : base(context, "Space$")
        {
        }

    }

    public class VB_Space
    : VbNativeFunction
    {
        public VB_Space(IVBScopeObject context, string name)
            : base(context, name)
        {
        }

        public VB_Space(IVBScopeObject context)
            : base(context, "Space")
        {
        }

        public override DExpression Evaluate(params DExpression[] args)
        {
            DExpression arg1 = args.FirstOrDefault();

            int count;
            if (!Converter.TryGetInt32Value(arg1, out count))
                return DefaultExpression(args);
            

            string value = new string(' ', count);

            return new DSimpleStringExpression(value, Encoding.Unicode);
        }

    }

    public class VB_ChrW 
        : VbNativeFunction
    {
        public VB_ChrW(IVBScopeObject context, string name)
            : base(context, name)
        {
        }

        public VB_ChrW(IVBScopeObject context) 
            : base(context, "ChrW")
        {
        }

        public override DExpression Evaluate(params DExpression[] args)
        {
            DExpression arg1 = args.FirstOrDefault();

            int ascii;

            if (!Converter.TryGetInt32Value(arg1, out ascii))
                return DefaultExpression(args);

            char test = (char)ascii;
            string value = new string(new char[] { test });

            //string value = Char.ConvertFromUtf32((int)ascii); //(byte) (UInt32)Math.Round(ascii) & 0x0000FFFF);

            return new DSimpleStringExpression(value, Encoding.Unicode);
        }

    }

    public class VB_ChrB_S
        : VB_ChrB
    {
        public VB_ChrB_S(IVBScopeObject context)
            : base(context, "ChrB$")
        {
        }
    }

        public class VB_ChrB
    : VbNativeFunction
    {
        public VB_ChrB(IVBScopeObject context, string name)
            : base(context, name)
        {
        }
        public VB_ChrB(IVBScopeObject context)
            : base(context, "ChrB")
        {
        }

        public override DExpression Evaluate(params DExpression[] args)
        {
            DExpression arg1 = args.FirstOrDefault();

            int ascii;

            if (!Converter.TryGetInt32Value(arg1, out ascii))
                return DefaultExpression(args);

            //byte[] test = { (byte) ascii };
            ////throw new Exception("Not managed !");
            //string value = Encoding.ASCII.GetString(test);
            string value = new string(new char[] { VbUtils.Chr(ascii) });

            return new DSimpleStringExpression(value, Encoding.Unicode);
        }
    }

    public class VB_Chr_S
        : VB_Chr
    {
        public VB_Chr_S(IVBScopeObject context)
            : base(context, "Chr$")
        {
        }
    }

    public class VB_Chr 
        : VbNativeFunction
    {
        public VB_Chr(IVBScopeObject context, string identifier)
            : base(context, identifier)
        {
        }

        public VB_Chr(IVBScopeObject context)
            : base(context, "Chr")
        {
        }

        public override DExpression Evaluate(params DExpression[] args)
        {
            DExpression arg1 = args.FirstOrDefault();

            int ascii;

            if (!Converter.TryGetInt32Value(arg1, out ascii))
                return DefaultExpression(args);

            string value = new string(new char[]{ VbUtils.Chr(ascii) }); 
            
            return new DSimpleStringExpression(value, Encoding.Unicode);
        }

    }


    public class VB_Abs
    : VbNativeFunction
    {
        public VB_Abs(IVBScopeObject context)
            : base(context, "Abs")
        {
        }

        public override DExpression Evaluate(params DExpression[] args)
        {
            DExpression arg1 = args.FirstOrDefault();

            double inputVal;

            if (!Converter.TryGetDoubleValue(arg1, out inputVal))
                return DefaultExpression(args);

            double value = Math.Abs(inputVal);

            return new DMathExpression<double>(value) { HasSideEffet = false };
        }
    }
    public class VB_Sgn
        : VbNativeFunction
    {
        public VB_Sgn(IVBScopeObject context)
            : base(context, "Sgn")
        {
        }

        public override DExpression Evaluate(params DExpression[] args)
        {
            DExpression arg1 = args.FirstOrDefault();

            double inputVal;

            if (!Converter.TryGetDoubleValue(arg1, out inputVal))
                return DefaultExpression(args);

            double value = Math.Sign(inputVal);

            return new DMathExpression<double>(value) { HasSideEffet = false };
        }
    }
    public class VB_Sqr
        : VbNativeFunction
    {
        public VB_Sqr(IVBScopeObject context)
            : base(context, "Sqr")
        {
        }

        public override DExpression Evaluate(params DExpression[] args)
        {
            DExpression arg1 = args.FirstOrDefault();

            double inputVal;

            if (!Converter.TryGetDoubleValue(arg1, out inputVal))
                return DefaultExpression(args);
            
            double value = Math.Sqrt(inputVal);

            return new DMathExpression<double>(value) { HasSideEffet = false };
        }
    }


    public class VB_Cos
    : VbNativeFunction
    {
        public VB_Cos(IVBScopeObject context)
            : base(context, "Cos")
        {
        }

        public override DExpression Evaluate(params DExpression[] args)
        {
            DExpression arg1 = args.FirstOrDefault();

            double input;
            if (!Converter.TryGetDoubleValue(arg1, out input))
                return DefaultExpression(args);
            
            double value = Math.Cos(input);

            return new DMathExpression<double>(value) { HasSideEffet = false };
        }
    }

    public class VB_CInt
        : VbNativeFunction
    {
        public VB_CInt(IVBScopeObject context)
            : base(context, "CInt")
        {
        }

        public override DExpression Evaluate(params DExpression[] args)
        {
            DExpression arg1 = args.FirstOrDefault();

            Int16 input;
            if (!Converter.TryGetInt16Value(arg1, out input))
                return DefaultExpression(args);


            return new DMathExpression<Int16>(input) { HasSideEffet = false };
        }
    }

    public class VB_CBool
        : VbNativeFunction
    {
        public VB_CBool(IVBScopeObject context)
            : base(context, "CBool")
        {
        }

        public override DExpression Evaluate(params DExpression[] args)
        {
            DExpression arg1 = args.FirstOrDefault();

            bool input;
            if (!Converter.TryGetBool(arg1, out input))
                return DefaultExpression(args);

            return new DBoolExpression(input) { HasSideEffet = false };
        }
    }

    public class VB_Exp
    : VbNativeFunction
    {
        public VB_Exp(IVBScopeObject context)
            : base(context, "Exp")
        {
        }

        public override DExpression Evaluate(params DExpression[] args)
        {
            DExpression arg1 = args.FirstOrDefault();

            double inputVal;

            if (!Converter.TryGetDoubleValue(arg1, out inputVal))
                return DefaultExpression(args);

            double value = Math.Exp(inputVal);

            return new DMathExpression<double>(value) { HasSideEffet = false };
        }
    }


    public class VB_Round
    : VbNativeFunction
    {
        public VB_Round(IVBScopeObject context)
            : base(context, "Round")
        {
        }

        public override DExpression Evaluate(params DExpression[] args)
        {
            return Evaluate(
                args.FirstOrDefault(),
                args.Skip(1).FirstOrDefault(), 
                args);
        }

        public DExpression Evaluate(DExpression number, DExpression numDecimal, DExpression[] args)
        {
            double numberVal;

            if (!Converter.TryGetDoubleValue(number, out numberVal))
                return DefaultExpression(args);

            int numDecimalVal;

            if (numDecimal != null)
            {
                if (!Converter.TryGetInt32Value(numDecimal, out numDecimalVal))
                    return DefaultExpression(args);


                double ret = Math.Round(numberVal, numDecimalVal);

                return new DMathExpression<double>(ret) { HasSideEffet = false };
            }

            double ret2 = Math.Round(numberVal);

            return new DMathExpression<double>(ret2) { HasSideEffet = false };
        }
    }


    public class VB_Rnd
    : VbNativeFunction
    {
        public VB_Rnd(IVBScopeObject context)
            : base(context, "Rnd")
        {
        }

        public override DExpression Evaluate(params DExpression[] args)
        {
            //DExpression arg1 = args.FirstOrDefault();

            return DefaultExpression(args);
        }
    }

    public class VB_Sin
    : VbNativeFunction
    {
        public VB_Sin(IVBScopeObject context)
            : base(context, "Sin")
        {
        }

        public override DExpression Evaluate(params DExpression[] args)
        {
            DExpression arg1 = args.FirstOrDefault();

            double input;
            if (!Converter.TryGetDoubleValue(arg1, out input))
                return DefaultExpression(args);


            double value = Math.Sin(input);

            return new DMathExpression<double>(value) { HasSideEffet = false };
        }
    }

    public class VB_Tan
        : VbNativeFunction
    {
        public VB_Tan(IVBScopeObject context)
            : base(context, "Tan")
        {
        }

        public override DExpression Evaluate(params DExpression[] args)
        {
            DExpression arg1 = args.FirstOrDefault();

            double input;
            if (!Converter.TryGetDoubleValue(arg1, out input))
                return DefaultExpression(args);


            double value = Math.Tan(input);

            return new DMathExpression<double>(value) { HasSideEffet = false };
        }
    }


    public class VB_Atn
    : VbNativeFunction
    {
        public VB_Atn(IVBScopeObject context)
            : base(context, "Atn")
        {
        }

        public override DExpression Evaluate(params DExpression[] args)
        {
            DExpression arg1 = args.FirstOrDefault();

            double input;
            if (!Converter.TryGetDoubleValue(arg1, out input))
                return DefaultExpression(args);


            double value = Math.Atan(input);

            return new DMathExpression<double>(value) { HasSideEffet = false };
        }

    }

    public class VB_Asc 
        : VbNativeFunction
    {
        public VB_Asc(IVBScopeObject context)
            : base(context, "Asc")
        {
        }

        public override DExpression Evaluate(params DExpression[] args)
        {
            DExpression arg1 = args.FirstOrDefault();

            string chrInput;

            if (!Converter.TryGetStringValue(arg1, out chrInput))
                return DefaultExpression(args);

            int value = VbUtils.Asc(chrInput);

            return new DMathExpression<int>(value) { HasSideEffet = false };
        }

    }
    public class VB_AscB
        : VbNativeFunction
    {
        public VB_AscB(IVBScopeObject context)
            : base(context, "AscB")
        {
        }

        public override DExpression Evaluate(params DExpression[] args)
        {
            DExpression arg1 = args.FirstOrDefault();

            string chrInput;

            if (!Converter.TryGetStringValue(arg1, out chrInput))
                return DefaultExpression(args);

            byte value = (byte)(int)(chrInput[0] & 0x000000FF);

            return new DMathExpression<byte>(value) { HasSideEffet = false };
        }

    }
    public class VB_AscW
    : VbNativeFunction
    {
        public VB_AscW(IVBScopeObject context)
            : base(context, "AscW")
        {
        }

        public override DExpression Evaluate(params DExpression[] args)
        {
            DExpression arg1 = args.FirstOrDefault();

            string chrInput;

            if (!Converter.TryGetStringValue(arg1, out chrInput))
                return DefaultExpression(args);

            int value = chrInput[0];

            return new DMathExpression<int>(value) { HasSideEffet = false };
        }

    }

    public class VB_Hex_S
        : VB_Hex
    {
        public VB_Hex_S(IVBScopeObject context)
            : base(context, "Hex$")
        {
        }
    }

    public class VB_Hex
    : VbNativeFunction
    {
        public VB_Hex(IVBScopeObject context, string name)
            : base(context, name)
        {
        }
        public VB_Hex(IVBScopeObject context)
            : base(context, "Hex")
        {
        }

        public override DExpression Evaluate(params DExpression[] args)
        {
            DExpression arg1 = args.FirstOrDefault();

            long input;
            if (!Converter.TryGetInt64Value(arg1, out input))
                return DefaultExpression(args);


            string hexStr = $"{input:X}";

            return new DSimpleStringExpression(hexStr, null);
        }
    }

    public class VB_Log 
        : VbNativeFunction
    {
        public VB_Log(IVBScopeObject context)
            : base(context, "Log")
        {
        }

        public override DExpression Evaluate(params DExpression[] args)
        {
            DExpression arg1 = args.FirstOrDefault();

            double input;
            if (!Converter.TryGetDoubleValue(arg1, out input))
                return DefaultExpression(args);

            return new DMathExpression<double>(Math.Log(input)) { HasSideEffet = false };
        }

    }


    public class VB_Val
    : VbNativeFunction
    {
        public VB_Val(IVBScopeObject context)
            : base(context, "Val")
        {
        }

        public override DExpression Evaluate(params DExpression[] args)
        {
            DExpression arg1 = args.FirstOrDefault();

            double? res = null;
            if (arg1 is IStringExpression)
            {
                string str = arg1.ToValueString();

                try
                {
                    bool hadValError;
                    res = VbUtils.Val(str, false, out hadValError);
                } 
                catch
                {
                    // If engine is VBA, return 0, if engine is vbscript, should throw exception
                    res = 0;
                }
            }

            if (arg1 is DMathExpression)
            {
                DMathExpression arg1_c = (DMathExpression)arg1;

                res = Convert.ToDouble(arg1_c.GetValueObject());
            }

            if (!res.HasValue)
                return DefaultExpression(args);

            return new DMathExpression<double>(res) { HasSideEffet = false };
        }

    }

    public class VB_CLng
    : VbNativeFunction
    {
        public VB_CLng(IVBScopeObject context)
            : base(context, "CLng")
        {
        }

        public override DExpression Evaluate(params DExpression[] args)
        {
            DExpression arg1 = args.FirstOrDefault();

            Int32 input;
            if (!Converter.TryGetInt32Value(arg1, out input))
                return DefaultExpression(args);

            return new DMathExpression<Int32>(input) { HasSideEffet = false };
        }

    }

    //public class VB_Execute
    //    : VbNativeFunction
    //{
    //    public VB_Execute(IVBScopeObject context)
    //        : base(context, "Execute")
    //    {
    //    }

    //    public override DExpression Evaluate(params DExpression[] args)
    //    {
    //        DExpression arg1 = args.FirstOrDefault();

    //        Console.WriteLine($"{Name}({arg1.ToExpressionString()})");

    //        return new DCodeBlock($"{Name}({arg1.ToExpressionString()})");
    //    }

    //}

    //public class VB_MsgBox 
    //    : VbNativeFunction
    //{
    //    public VB_MsgBox(IVBScopeObject context)
    //        : base(context, "MsgBox")
    //    {
    //    }

    //    public override DExpression Evaluate(params DExpression[] args)
    //    {
    //        DExpression arg1 = args.FirstOrDefault();

    //        Console.WriteLine($"{Name}({arg1.ToExpressionString()})");

    //        return new DCodeBlock($"{Name}({arg1.ToExpressionString()})");
    //    }

    //}

    public static class Converter
    {
        internal static DSimpleStringExpression ToStringExp(DExpression arg1)
        {
            throw new NotImplementedException();
        }

        internal static bool TryGetInt16Value(DExpression arg1, out Int16 output)
        {
            double dbl;

            if (TryGetSingleValue(arg1, out dbl))
            {
                output = (Int16)Math.Round(dbl, 0);
                return true;
            }

            output = 0;
            return false;
        }

        internal static bool TryGetInt32Value(DExpression arg1, out Int32 output)
        {
            double dbl;

            if (TryGetDoubleValue(arg1, out dbl))
            {
                output = (int)Math.Round(dbl, 0);
                return true;
            }

            output = 0;
            return false;
        }

        internal static bool TryGetInt64Value(DExpression arg1, out Int64 output)
        {
            double dbl;

            if (TryGetDoubleValue(arg1, out dbl))
            {
                output = (Int64)Math.Round(dbl, 0);
                return true;
            }

            output = 0;
            return false;
        }

        internal static bool TryGetDoubleValue(DExpression arg1, out double doubleArg)
        {
            if (arg1 is IStringExpression)
            {
                try
                {
                    if (!arg1.IsValuable)
                    {
                        doubleArg = 0;
                        return false;
                    }

                    string str = arg1.ToValueString();

                    bool hadValError;
                    double? val = VbUtils.Val(str, false, out hadValError);

                    if (val.HasValue && !hadValError)
                    {
                        doubleArg = val.Value;
                        return true;
                    }
                }
                catch
                {
                    doubleArg = 0;
                    return false;
                }
            }

            if (arg1 is DMathExpression)
            {
                DMathExpression arg1_c = (DMathExpression)arg1;

                doubleArg = Convert.ToDouble(arg1_c.GetValueObject());
                return true;
            }


            doubleArg = 0;
            return false;
        }

        internal static bool TryGetSingleValue(DExpression arg1, out double doubleArg)
        {
            if (arg1 is IStringExpression)
            {
                string str = arg1.ToValueString();

                try
                {
                    bool hadValError;
                    float? val = (float)VbUtils.Val(str, true, out hadValError);

                    if (val.HasValue && !hadValError)
                    {
                        doubleArg = val.Value;
                        return true;
                    }
                }
                catch
                {
                    doubleArg = 0;
                    return false;
                }
            }

            if (arg1 is DMathExpression)
            {
                DMathExpression arg1_c = (DMathExpression)arg1;

                doubleArg = Convert.ToDouble(arg1_c.GetValueObject());
                return true;
            }


            doubleArg = 0;
            return false;
        }


        internal static bool TryGetStringValue(DExpression arg1, out string strArg)
        {
            if (arg1 is DCodeBlock)
            {
                strArg = null;
                return false;
            }

            if (arg1 is IStringExpression)
            {
                if (arg1.IsValuable)
                {
                    strArg = arg1.ToValueString();
                    return true;
                }
            }

            if (arg1 is DMathExpression)
            {
                if (arg1.IsValuable)
                {
                    strArg = arg1.ToValueString();
                    return true;
                }
            }


            if (arg1 is DEmptyVariable)
            {
                strArg = string.Empty;
                return true;
            }

            if (arg1 is DUndefinedVariable)
            {
                strArg = null;
                return false;
            }

            strArg = null;
            return false;
        }

        internal static bool TryGetBool(DExpression arg1, out bool output)
        {
            string outStr;
            if (TryGetStringValue(arg1, out outStr))
            {
                if (outStr.Equals("false", StringComparison.OrdinalIgnoreCase))
                {
                    output = false;
                    return true;
                }
                if (outStr.Equals("true", StringComparison.OrdinalIgnoreCase))
                {
                    output = true;
                    return true;
                }
            }


            double dbl;

            if (TryGetSingleValue(arg1, out dbl))
            {
                output = dbl != 0;
                return true;
            }

            output = false;
            return false;
        }
    }
}
