using CommandLine;
using CommandLine.Text;

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using vbSparkle.Options;
using Colorful;
using System.Drawing;
using System.Threading.Tasks;
using System.Threading;

namespace vbSparkle.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            InitializeConsoleHeader();
            //1- disable auto generated help
            var parser = new Parser(with => with.HelpWriter = null);

            if (Console.IsInputRedirected)
            {
                //2- run parser and get result
                var parserResult = parser.ParseArguments<BaseOptions>(args);

                parserResult.WithNotParsed(errs => DisplayHelp(parserResult, errs));
                parserResult.WithParsed(opts => ProcessStdIn(opts));
            } 
            else
            {
                //2- run parser and get result
                var parserResult = parser.ParseArguments<Options>(args);

                parserResult.WithNotParsed(errs => DisplayHelp(parserResult, errs));
                parserResult.WithParsed(opts => RunOptionsAndReturnExitCode(opts));
            }

        }

        private static void InitializeConsoleHeader()
        {
            Console.ForegroundColor = Color.WhiteSmoke;
            string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            Console.ResetColor();
            Console.ReplaceAllColorsWithDefaults();
            Console.Title = "vbSparkle " + version;

            Console.WriteLine(
                    @"       _     __                  _    _      " + "\r\n" +
                    @"__   _| |__ / _\_ __   __ _ _ __| | _| | ___ " + "\r\n" +
                    @"\ \ / / '_ \\ \| '_ \ / _` | '__| |/ / |/ _ \" + "\r\n" +
                    @" \ V /| |_) |\ \ |_) | (_| | |  |   <| |  __/" + "\r\n" +
                    @"  \_/ |_.__/\__/ .__/ \__,_|_|  |_|\_\_|\___|" + "\r\n" +
                    @"               |_|               v" + version
                  , Color.AliceBlue);

            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine("Author(s): Sylvain Bruyere, Airbus CERT");
            Console.WriteLine("Copyright © Airbus CERT");
            Console.WriteLine("https://github.com/airbus-cert/vbSparkle");
            Console.WriteLine();
            Console.WriteLine();
        }

        private static void DisplayHelp<T>(ParserResult<T> result, IEnumerable<Error> errors)
        {
            var helpText = HelpText.AutoBuild(result, h =>
            {
                h.AdditionalNewLineAfterOption = true;
                h.Heading = string.Empty;
                h.Copyright = string.Empty;
                h.AddEnumValuesToHelpText = true;

                h.AddPreOptionsLine("Sample usage:");
                h.AddPreOptionsLine("> vbSparkle.CLI -p sample.vbs");

                return HelpText.DefaultParsingErrorsHandler(result, h);
            }, e => e);

            Console.WriteLine(helpText);

            Console.ReadLine();
        }

        private static void ProcessStdIn(BaseOptions opts)
        {
            using (var reader = new StreamReader(Console.OpenStandardInput(), Console.InputEncoding))
            {
                string fileContent = reader.ReadToEnd();

                string result = DeobfuscateContent(fileContent, opts);

                if (!string.IsNullOrWhiteSpace(opts.Output))
                    File.AppendAllText(opts.Output, result);
                else
                {
                    WriteSyntaxColoringConsoleCode(result);
                    // Console.Out.Write(result);
                }
            }
        }

        private static void RunOptionsAndReturnExitCode(Options opts)
        {
            if (opts.InputFiles.Count() > 0)
                foreach (var filename in opts.InputFiles)
                {
                    Console.WriteLine($"# Processing {filename} ...");
                    string fileContent = File.ReadAllText(filename);

                    string result = DeobfuscateContent(fileContent, opts);

                    if (!string.IsNullOrWhiteSpace(opts.Output))
                        File.AppendAllText(opts.Output, result);
                    else
                        WriteSyntaxColoringConsoleCode(result);

                }


            if (Console.IsInputRedirected)
            {
                ProcessStdIn(opts);
                return;
            }
        }

        private static void WriteSyntaxColoringConsoleCode(string result)
        {
            Stopwatch perfWatch = new Stopwatch();

            perfWatch.Start();

            string[] vbKeywords = new string[]
            {
                "As",
                "Binary",
                "ByRef",
                "ByVal",
                "Date",
                "Else",
                "Empty",
                "Error",
                "False",
                "For",
                "Friend",
                "Get",
                "Input",
                "Is",
                "Len",
                "Let",
                "Lock",
                "Me",
                "Mid",
                "New",
                "Next",
                "Nothing",
                "Null",
                "On",
                "Option",
                "Optional",
                "ParamArray",
                "Print",
                "Private",
                "Property",
                "PtrSafe",
                "Public",
                "Resume",
                "Seek",
                "Set",
                "Static",
                "Step",
                "String",
                "Then",
                "Time",
                "To",
                "True",
                "WithEvents",
                "Dim",
                "ReDim",
                "Preserve",
                "If",
                "Then",
                "Function",
                "Sub",
                "GoSub",
                "GoTo",
                "On",
                "Error",
                "Do",
                "Until",
                "End",
                "Exit",
                "While", 
                "Loop", 
                "And", 
                "Or",
                "\\+",
                "\\&",
                "\\=",
                "\\-",
                "\\*"
            };


            string[] funcKeywords = new string[]
            {
                "Mid",
                "Mid$",
                "Asc",
                "Asc$",
                "Chr",
                "Chr$",
                "UBound",
                "LBound",
                "Len",
                "UCase",
                "LCase"

            };

            StyleSheet styleSheet = new StyleSheet(Color.White);

            string[] dangerKeywords = new string[]
            {
                            "CreateObject",
                            "WScript.Shell",
                            "WScript.GetObject",

            };

            foreach (var v in dangerKeywords.Distinct().ToArray())
                styleSheet.AddStyle(v, Color.Red);

            foreach (var v in vbKeywords.Distinct().ToArray())
                styleSheet.AddStyle(v + "\\s+", Color.CornflowerBlue);

            foreach (var v in funcKeywords.Distinct().ToArray())
                styleSheet.AddStyle(v + "\\(", Color.LightBlue);



            //foreach (var v in funcKeywords.Distinct().ToArray())
            styleSheet.AddStyle("[a-zA-Z][\\w]*\\(", Color.SkyBlue);

            styleSheet.AddStyle("\\)", Color.LightBlue);

            styleSheet.AddStyle("\\\"(.*?)\\\"", Color.Orange);
            styleSheet.AddStyle("\\\'(.*?).*", Color.Green);

            Console.WriteLine(new string('═', 80));

            Console.WriteLineStyled(result + "\r\n", styleSheet);

            perfWatch.Stop();
            Console.ForegroundColor = Color.WhiteSmoke;

            Console.WriteLine(new string('═', 80));
            Console.WriteLine($"# Printed in {perfWatch.ElapsedMilliseconds} ms.");

        }

        private static string DeobfuscateContent(string content, BaseOptions opts)
        {
            Stopwatch perfWatch = new Stopwatch();

            perfWatch.Start();

            var result = VbPartialEvaluator.PrettifyEncoded(content, new EvaluatorOptions()
            {
                SymbolRenamingMode = opts.SymbolRenamingMode,
                JunkCodeProcessingMode = opts.JunkCodeProcessingMode,
                IndentSpacing = opts.IndentSpacing
            });

            perfWatch.Stop();
            Console.WriteLine($"# Computed in {perfWatch.ElapsedMilliseconds} ms.");

            return result;
        }
    }
}
