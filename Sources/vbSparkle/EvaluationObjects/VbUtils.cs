using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace vbSparkle
{
    public class VbUtils
    {
        /// <summary>
        /// Returns the character associated with the specified character code.
        /// </summary>
        /// 
        /// <returns>
        /// Returns the character associated with the specified character code.
        /// </returns>
        /// <param name="CharCode">Required. An Integer expression representing the <paramref name="code point"/>, or character code, for the character.</param><exception cref="T:System.ArgumentException"><paramref name="CharCode"/> &lt; 0 or &gt; 255 for Chr.</exception><filterpriority>1</filterpriority>
        public static char Chr(int CharCode)
        {
            if (CharCode < (int)short.MinValue || CharCode > (int)ushort.MaxValue)
                throw new ArgumentException();

            if (CharCode >= 0 && CharCode <= (int)sbyte.MaxValue)
                return Convert.ToChar(CharCode);

            try
            {
                Encoding encoding = Encoding.GetEncoding(GetLocaleCodePage());

                if (encoding.IsSingleByte && (CharCode < 0 || CharCode > (int)byte.MaxValue))
                    throw new IndexOutOfRangeException();

                char[] chars = new char[2];
                byte[] bytes = new byte[2];
                Decoder decoder = encoding.GetDecoder();

                if (CharCode >= 0 && CharCode <= (int)byte.MaxValue)
                {
                    bytes[0] = checked((byte)(CharCode & (int)byte.MaxValue));
                    decoder.GetChars(bytes, 0, 1, chars, 0);
                }
                else
                {
                    bytes[0] = checked((byte)((CharCode & 65280) >> 8));
                    bytes[1] = checked((byte)(CharCode & (int)byte.MaxValue));
                    decoder.GetChars(bytes, 0, 2, chars, 0);
                }

                return chars[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Returns an Integer value representing the character code corresponding to a character.
        /// </summary>
        /// 
        /// <returns>
        /// Returns an Integer value representing the character code corresponding to a character.
        /// </returns>
        /// <param name="String">Required. Any valid Char or String expression. If <paramref name="String"/> is a String expression, only the first character of the string is used for input. If <paramref name="String"/> is Nothing or contains no characters, an <see cref="T:System.ArgumentException"/> error occurs.</param><filterpriority>1</filterpriority>
        public static int Asc(char String)
        {
            int num1 = Convert.ToInt32(String);
            if (num1 < 128)
                return num1;
            try
            {
                Encoding fileIoEncoding = GetFileIOEncoding();
                char[] chars = new char[1]
                {
      String
                };
                if (fileIoEncoding.IsSingleByte)
                {
                    byte[] bytes = new byte[1];
                    fileIoEncoding.GetBytes(chars, 0, 1, bytes, 0);
                    return (int)bytes[0];
                }
                byte[] bytes1 = new byte[2];
                if (fileIoEncoding.GetBytes(chars, 0, 1, bytes1, 0) == 1)
                    return (int)bytes1[0];
                if (BitConverter.IsLittleEndian)
                {
                    byte num2 = bytes1[0];
                    bytes1[0] = bytes1[1];
                    bytes1[1] = num2;
                }
                return (int)BitConverter.ToInt16(bytes1, 0);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Returns an Integer value representing the character code corresponding to a character.
        /// </summary>
        /// 
        /// <returns>
        /// Returns an Integer value representing the character code corresponding to a character.
        /// </returns>
        /// <param name="String">Required. Any valid Char or String expression. If <paramref name="String"/> is a String expression, only the first character of the string is used for input. If <paramref name="String"/> is Nothing or contains no characters, an <see cref="T:System.ArgumentException"/> error occurs.</param><filterpriority>1</filterpriority>
        public static int Asc(string String)
        {
            if (String == null || String.Length == 0)
                throw new ArgumentException();
            return Asc(String[0]);
        }

        internal static Encoding GetFileIOEncoding()
        {
            return Encoding.GetEncoding(GetLocaleCodePage()); //Encoding.Default;
        }

        internal static int GetLocaleCodePage()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            return Thread.CurrentThread.CurrentCulture.TextInfo.ANSICodePage;
        }

        public static int AscXX(char c)
        {
            int converted = c;
            if (converted >= 0x80)
            {
                byte[] buffer = new byte[2];
                // if the resulting conversion is 1 byte in length, just use the value
                if (System.Text.Encoding.Default.GetBytes(new char[] { c }, 0, 1, buffer, 0) == 1)
                {
                    converted = buffer[0];
                }
                else
                {
                    // byte swap bytes 1 and 2;
                    converted = buffer[0] << 16 | buffer[1];
                }
            }
            return converted;
        }

        public static int AscB(char c)
        {
            int converted = c;
            if (converted >= 0x80)
            {
                byte[] buffer = new byte[2];
                // if the resulting conversion is 1 byte in length, just use the value
                if (System.Text.Encoding.Default.GetBytes(new char[] { c }, 0, 1, buffer, 0) == 1)
                {
                    converted = buffer[0];
                }
                else
                {
                    // byte swap bytes 1 and 2;
                    converted = buffer[0] << 16 | buffer[1];
                }
            }
            return converted;
        }

       

        public static string StrValToExp(string value)
        {
            var tab = "    ";
            var chkNewLine = "& _\r\n" + tab + "\"\"";
            //var chkNullQuote1 = " & \"\"";
            //var chkNullQuote2 = "\"\" &";

            // Replace chars with intermediate representation
            value = value.Replace("\"\"", "{{{###esc-quote###}}}");
            value = value.Replace("\"", "{{{###quote###}}}");
            value = value.Replace("\r\n", "{{{###rn###}}}");
            value = value.Replace("\r", "{{{###r###}}}");
            value = value.Replace("\n", "{{{###n###}}}");

            // Replace intermediate representation by Visual Basic expression
            value = value.Replace("{{{###rn###}}}", "\" & vbCrLf & _\r\n" + tab + "\"");
            value = value.Replace("{{{###r###}}}", "\" & vbCr & _\r\n" + tab + "\"");
            value = value.Replace("{{{###n###}}}", "\" & vbLf & _\r\n" + tab + "\"");
            value = value.Replace("{{{###quote###}}}", "\"\"");
            value = value.Replace("{{{###esc-quote###}}}", "\"\"");

            value = $"\"{value}\"";


            if (value.EndsWith(chkNewLine))
                value = value.Substring(0, value.Length - chkNewLine.Length);

            value = value.Replace("& \"\" &", "&");

            //value = value.Replace("\" & vbCrLf & \"", "\" & vbCrLf &  _\r\n\"");

            return value.Trim();
        }

        internal static double ConvStrToDouble(string str, bool is16bit)
        {
            if (str.StartsWith("&"))
            {
                str = str.Substring(1).ToUpper();

                char prefix = str[0];
                switch(prefix)
                {
                    case 'H':
                        str = str.Substring(1).ToLower();

                        if (str.Length > 8)
                            str = str.Substring(str.Length - 8);

                        if (is16bit)
                            return Convert.ToInt16(str, 16);
                        else
                            return Convert.ToInt64(str, 16);
                    case 'B':
                        str = str.Substring(1).ToLower();

                        return Convert.ToInt32(str, 2);
                    case 'O':
                        str = str.Substring(1).ToLower();
                        break;
                }
                return Convert.ToInt32(str, 8);
            }
            else
            {
                if (string.IsNullOrEmpty(str))
                    return 0;

                return Convert.ToDouble(str);
            }
        }

        internal static double? Val(string str, bool is16bit, out bool hasValError)
        {
            str = str.Replace("\u202d", "");

            string prefix = "";
            string acceptedChars = "1234567890+-E.";
            string acceptedSeparators = ",";

            bool useScientificNotation = false;
            bool hasSeparator = false;

            hasValError = false;

            StringBuilder res = new StringBuilder();
            StringBuilder scie = new StringBuilder();

            foreach (var v in str)
            {
                var chr = char.ToUpper(v);

                if (chr == ' ')
                    continue;

                if (chr == '&')
                {
                    if (res.Length == 0)
                    {
                        prefix += chr;
                        continue;
                    }
                    else
                        break;
                }

                if (prefix == "&" && res.Length == 0)
                {
                    switch (chr)
                    {
                        case 'O':
                            prefix += chr;
                            acceptedChars = "01234567";
                            continue;
                        case 'H':
                            prefix += chr;
                            acceptedChars = "01234567890ABCDEF";
                            continue;
                        default:
                            acceptedChars = "01234567";
                            break;
                    }
                }

                if (acceptedSeparators.Contains(chr))
                {
                    if (!string.IsNullOrEmpty(prefix) || useScientificNotation)
                    {
                        hasValError = true;
                        throw new NotSupportedException();
                    } else
                    {
                        hasSeparator = true;
                        continue;
                    }

                }
                                
                if (acceptedChars.Contains(chr))
                {
                    if (string.IsNullOrEmpty(prefix) && chr == 'E')
                    {
                        useScientificNotation = true;
                        continue;
                    }

                    if (useScientificNotation)
                    {
                        if (chr == 'E')
                        {
                            hasValError = true;
                            break;
                        }

                        if (chr == '+' || chr == '-')
                        {
                            if( scie.Length == 0)
                            {
                                scie.Append(chr);
                                continue;
                            }
                            else
                            {
                                hasValError = true;
                                break;
                            }
                        }

                        scie.Append(chr);
                        continue;
                    }

                    res.Append(chr);
                }
                else
                {
                    hasValError = true;
                    break;
                }

            }

            if (res.Length > 0)
            {
                if (prefix.Length > 0)
                {
                    res.Insert(0, prefix);
                }

                if (scie.Length > 0)
                {
                    res.Append("e");
                    res.Append(scie);
                }
            }

            return ConvStrToDouble(res.ToString(), is16bit);
        }
    }
}
