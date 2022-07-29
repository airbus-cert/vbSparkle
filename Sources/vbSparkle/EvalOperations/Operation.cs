using System;
using System.Collections.Generic;
using MathNet.Symbolics;
using vbSparkle.EvaluationObjects;

namespace vbSparkle
{
    internal abstract class Operation
    {
        public abstract DExpression Add(DExpression left, DExpression right);
        public abstract DExpression Subtract(DExpression left, DExpression right);
        public abstract DExpression Multiply(DExpression left, DExpression right);
        public abstract DExpression Divide(DExpression left, DExpression right);
        public abstract DExpression Modulo(DExpression left, DExpression right);
        public abstract DExpression Exp(DExpression left, DExpression right);

        public static DExpression DoOperation(
            string operator_n, 
            DExpression leftExp, 
            DExpression rightExp,
            bool unaryOperation = false)
        {
            operator_n = FormatOperator(operator_n);


            if (operator_n == "&")
            {
                return StrConcat(leftExp, rightExp);
            }

            if (
                operator_n == "+" &&
                leftExp is IStringExpression &&
                rightExp is IStringExpression)
            {
                return StrConcat(leftExp, rightExp);
            }



            DExpression result = null;


            SymbolicExpression leftSym = leftExp?.GetSymExp();
            SymbolicExpression rightSym = rightExp?.GetSymExp();
            SymbolicExpression result2 = null;

            if (!unaryOperation && IsValueExpression(leftSym))
                return GetExplicitExpressionCodeBlock(operator_n, leftExp, rightExp);

            if (IsValueExpression(rightSym))
                return GetExplicitExpressionCodeBlock(operator_n, leftExp, rightExp);


            if (unaryOperation)
            {
                switch (operator_n)
                {
                    case "+":
                        result2 = rightSym;
                        break;
                    case "-":
                        result2 = - rightSym;
                        break;
                    case "Not":
                        return new DBoolExpression(rightSym.RealNumberValue == 0);
                }
            }
            else
            {
                switch (operator_n)
                {
                    case "+":
                        result2 = leftSym + rightSym;
                        break;
                    case "-":
                        result2 = leftSym - rightSym;
                        break;
                    case "*":
                        result2 = leftSym * rightSym;
                        break;
                    case "/":
                        result2 = leftSym / rightSym;
                        break;
                    case "^":
                        result2 = leftSym.Pow(rightSym);
                        break;
                    case "=":
                        return new DBoolExpression(leftSym.RealNumberValue == rightSym.RealNumberValue);
                    case "<>":
                        return new DBoolExpression(leftSym.RealNumberValue != rightSym.RealNumberValue);
                    case "<=":
                        return new DBoolExpression(leftSym.RealNumberValue <= rightSym.RealNumberValue);
                    case ">=":
                        return new DBoolExpression(leftSym.RealNumberValue >= rightSym.RealNumberValue);
                    case "And":
                        return new DBoolExpression((long)leftSym.RealNumberValue & (long)rightSym.RealNumberValue);
                    case "Or":
                        return new DBoolExpression((long)leftSym.RealNumberValue | (long)rightSym.RealNumberValue);
                    case "Xor":
                        return new DBoolExpression((long)leftSym.RealNumberValue ^ (long)rightSym.RealNumberValue);
                    case "Mod":
                        return new DMathExpression<double>(leftSym.RealNumberValue % rightSym.RealNumberValue);
                    case "Like":
                        return GetExplicitExpressionCodeBlock(operator_n, leftExp, rightExp);
                    //case "Mod":

                    //    //result2 = leftSym.;
                    //    //result2 = ;
                    //    break;
                    //case "&":
                    //    //result2 = ;
                    //    break;
                    default:
                        return GetExplicitExpressionCodeBlock(operator_n, leftExp, rightExp);
                }
            }

            

            try
            {
                return new DMathExpression<double>(result2);
            }
            catch
            {
                return GetExplicitExpressionCodeBlock(operator_n, leftExp, rightExp);
            }

        }

        private static bool IsValueExpression(SymbolicExpression sym)
        {
            if (sym == null)
                return true;

            return !(sym.Expression.IsNumber == true || sym.Expression.IsConstant || sym.Expression.IsApproximation);
        }

        private static string FormatOperator(string op)
        {
            switch(op.ToLower())
            {
                case "and":
                    return "And";
                case "or":
                    return "Or";
                case "xor":
                    return "Xor";
                case "not":
                    return "Not";
                case "mod":
                    return "Mod";
                case "andalso":
                    return "AndAlso";
                case "orelse":
                    return "OrElse";
                case "isfalse":
                    return "IsFalse";
                case "istrue":
                    return "IsTrue";
                case "is":
                    return "Is";
                case "isnot":
                    return "IsNot";
                case "like":
                    return "Like";
                default:
                    return op;
            }
        }

        private static DCodeBlock GetExplicitExpressionCodeBlock(string operator_n, DExpression leftExp, DExpression rightExp)
        {
            if (leftExp == null)
                return new DCodeBlock($"{operator_n} {rightExp.ToExpressionString()}");

            return new DCodeBlock($"{leftExp.ToExpressionString()} {operator_n} {rightExp.ToExpressionString()}");
        }

        private static DExpression StrConcat(DExpression leftExp, DExpression rightExp)
        {
            DComplexStringExpression leftStrExp;

            if (leftExp is DComplexStringExpression)
            {
                //leftStrExp = leftExp as DComplexStringExpression; CAUSE BAD SIDE EFFECTS
                leftStrExp = new DComplexStringExpression(leftExp);
            }
            else
            {
                leftStrExp = new DComplexStringExpression(leftExp);
            }

            leftStrExp.Concat(rightExp);

            return leftStrExp;
        }
    }
}