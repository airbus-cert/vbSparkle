[![.NET](https://github.com/airbus-cert/vbSparkle/actions/workflows/dotnet.yml/badge.svg)](https://github.com/airbus-cert/vbSparkle/actions/workflows/dotnet.yml)
[![NuGet](https://img.shields.io/nuget/v/vbSparkle)](https://www.nuget.org/packages/vbSparkle/)
[![NuGet Download](https://img.shields.io/nuget/dt/vbSparkle)](https://www.nuget.org/packages/vbSparkle/)

# What is vbSparkle ?
**vbSparkle** is a source-to-source multi-platform ***Visual Basic deobfuscator*** based on partial-evaluation and is mainly dedicated to the analysis of malicious code written in VBScript and VBA (Office Macro).

It is written in native **C#** and provides a **.Net Standard library**, and works on Windows, Linux, MacOS, etc.. 

The parsing of Visual Basic Script and VBA is processed through the use of [**ANTLR**](https://www.antlr.org/) grammar & lexer parsers.

# Setup
## Package manager
`Install-Package vbSparkle`

## .Net CLI
`dotnet add package vbSparkle`

# How to use ?
## As a library
Once you've add the **vbSparkle** library to your project, you just have to call the function `PrettifyEncoded` from the static class `VbPartialEvaluator`:
```vb
string obfuscatedVB = "msgbox(chr(65)&chr(66)&(chr(67))"
string deobfuscated = VbPartialEvaluator.PrettifyEncoded(obfuscatedVB);
```
... will return `MsgBox("ABC")`.

## As a CLI
The attached project `vbSparkle.CLI` is an exemple of use of vbSparkle as a CLI.
The current exemple take either a path as an argument or a full binary in `StdIn`, and return the deobfuscated result.
![cli-exemple](/Resources/cli-exemple.JPG)


```
  -p, --path                (Group: input) Path of directory or script file(s)
                            to be deobfuscated.

  --stdin                   (Group: input) (Default: false) Read from stdin

  -o, --output              File offset.

  --sym-rename-mode         (Default: None) Define how symbols can be renamed.
                            Valid values: None, Variables, Constants, All

  --junk-code-processing    (Default: Comment) Define how junk code should be
                            processed. Valid values: Nothing, Remove, Comment

  -i, --indent-spacing      (Default: 4) Defines the number of spaces taken into
                            account for the indentation of the code.
```

## Web UI
The attached project `vbSparkle.Web` is an exemple of use of vbSparkle within a Web UI.
![web-ui](/Resources/webUI.PNG)

# Why to write a VBScript de-obfuscator based on partial-evaluation ?
VBScript and VBA code obfuscation are popular among attackers and allow to evade detection measures, antivirus, firewalls, EDRs, and allows to make malware analysis more difficult.

Thus it is common to see obfuscated VBScript in the very first stage of a machine compromission.

A typical VBScript code obfuscation technique is to encapsulate the script in an obfuscated string and execute it via the `Execute("...code..." function)` after it is decoded / evaluated.

Let's see an example:
```vb
Execute(chr( 456127/3833 ) & chr( 817650/7110 ) & chr( 759429/7671 ) & chr( 834138/7317 ) & chr( 57960/552 ) & chr( 2549-2437 ) & chr( 1078568/9298 ) & chr( 5143-5097 ) & chr( -8682+8783 ) & chr( 529-430 ) & chr( 673088/6472 ) & chr( 761682/6862 ) & chr( 302336/9448 ) & chr( 9427-9393 ) & chr( 410176/3944 ) & chr( -5271+5372 ) & chr( 796608/7376 ) & chr( 2896-2788 ) & chr( 318792/2872 ) & chr( 1076-1042 ) &  vbcrlf  ) 
```

Here, the attacker made use of:
- `chr(CHAR_CODE)` function to evaluate a string character (eg. `chr(65) = "A"`)
- arithmetic operation (eg. `456127/3833`) to evaluate the char value
- string concatenation `&` to assemble all decoded characters
- call to `Execute(..)` function to execute the decoded script.

If we follow the script execution:
- `456127/3833 => 119`
- `chr(119) => "w"`
- `817650/7110 => 115`
- `chr(115) => "s"`
- `"w" & "s" => "ws"`
- `Execute("ws.....`

Here is the original script:
```wscript.echo "hello"```

A manual deobfuscation is made very difficult and time-consuming on large scripts.
A common deobfuscation technique with VBScript is simply to replace the `Execute()` function, by a `WScript.Echo(`, and then run the script to get the output.

This last technique is simple but has some disadvantages:
- requires at least one VM / sandbox to execute the script securely;
- the automation of this technique is dangerous and random;
- it does not make it possible to understand the entire code and its side effects;
- some conditional tasks can be missed, depending on the running environment;

And, at the time this project started their was no alternative to these both techniques.

# Writing a Visual Basic partial-evaluator

Partial-evaluation is not a new concept, it's mostly used by compiler for code-optimization in pre-compilation processus (eg. https://en.wikipedia.org/wiki/Partial_evaluation). 

A partial evaluator reads a program (called the subject program) along with some of the inputs for that program, and evaluates only the parts of the program that depend on the inputs provided. 
Once all these parts have been evaluated, the remaining program (called the residual program) is emitted as output.

For exemple, a pre-compilation process would transform the following subjet code:
```vb
var1 = 3*3 
var2 = sin(arg1)
var3 = var1 + func_xx(2 * var1 + var2)
```
Into the following residual code:
```vb
var2 = sin(arg1)
var3 = 9 + func_xx(18 + var2)
```
The code has been partially evaluated before compilation, to optimize performance.

We see that this process simplifies the code, and eliminates unnecessary operations.
It's interesting because the obfuscation hides the code by adding code and operations unnecessary for the execution of the code.

If we inspect our initial sample, step by step:
- arithmetic operation like: `456127/3833` can be partially evaluated: no side effect
- `chr(119)` can be partially evaluated: 119 is a constant, and this is a native function, no side effect
- "w" & "s" can be partially evaluated: it's constant char concatenation, no side effect
- "Execute(" has known side effect (script execution), it won't be evaluated, except if functions side effect can be managed / emulated

A partial evaluation of that sample would result in:
```Execute("wscript.echo ""hello""" & vbCrLf)```

And guess what ? Oh we just de-obfuscated our sample.

# Theory is good, but in practice ?
Well, in practice, it's far more complicated:
- To achieve a partial evaluator, you need a parser.
- To get a parser, you need a lexer, and a grammar.

Hopefully, ANTLR is a powerful parser generator for reading, processing, executing, or translating structured text or binary files. It's known to be widely used to build languages, tools, and frameworks. From a grammar, ANTLR generates a parser that can build parse trees and also generates a listener interface (or visitor) that makes it easy to respond to the recognition of phrases of interest.

ANTLR can generate lexers, parsers, tree parsers, and combined lexer-parsers. Parsers can automatically generate parse trees or abstract syntax trees which can be further processed with tree parsers. ANTLR provides a single consistent notation for specifying lexers, parsers, and tree parsers.

By default, ANTLR reads a grammar and generates a recognizer for the language defined by the grammar (i.e., a program that reads an input stream and generates an error if the input stream does not conform to the syntax specified by the grammar).

In order to do something useful with the language, actions can be attached to grammar elements in the grammar. These actions are written in the programming language in which the recognizer is being generated. When the recognizer is being generated, the actions are embedded in the source code of the recognizer at the appropriate points.

# How it works, basically
Thanks to [**ANTLR**](https://www.antlr.org/), it was fast & easy to generate a first version of the AST, the lexer, and the parser thanks to existing grammar written by [Ulrich Wolffgang](https://github.com/antlr/grammars-v4/blob/master/vba/vba.g4).

***Railroad sample from the VB grammar:***
![Railroad Diagram](/Resources/railroad_diagram.PNG)

This grammar was not fully working with some specific Visual Basic syntaxes (inline code, some operators issues) and some change were needed to provide a full VBS & VBA compatible grammar. The modified grammar can be found here : [Visual Basic Grammar](https://github.cert.corp/CERT/vbSparkle/blob/master/Sources/vbSparkle/VBScript.g4)

Here is an example of visual parse tree of the following piece of code:

```vbscript
Msgbox("Hello world " & Chr(33), vbCritical, "The Title")
```

![Parse tree sample](/Resources/parse-tree-sample.PNG)

Once the parser and the AST is generated, we can start using it and *explore* our code:
```c#
using Antlr4.Runtime;
using System.Linq;

namespace vbSparkle
{
    public class PartialEvaluator
    {
        public static string Prettify(string script)
        {
            var inputStream = new AntlrInputStream(script);

            // Create the lexer
            var lexer = new VBScriptLexer(inputStream);

            // Instanciate of common token from lexer
            var commonTokenStream = new CommonTokenStream(lexer);

            // Create the parser
            var parser = new VBScriptParser(commonTokenStream);

            // Get the start rule (root element of our AST) 
            VBScriptParser.StartRuleContext stContext = parser.startRule();

            // Instanciate the visitor
            VbAnalyser analyser = new VbAnalyser();

            // Visit the AST
            analyser.Visit(stContext);

            // Root AST element of a vbScript being a "Module", we get it
            var module = analyser.Modules.FirstOrDefault();

            // Then we partially evaluate our module
            string result = module.Prettify(partialEvaluation: true);
            return result;
        }
    }
}
```

A visitor is in charge of crossing over the AST, and create each node partial-evaluator/prettifier.
These last will be in charge of evaluating recursively:
![Workflow](/Resources/workflow.png)

For exemple, here is our visitor, which instanciate a VbModule evaluator:
```c#
    public class VbAnalyser
    {
        public List<VbModule> Modules { get; set; } = new List<VbModule>();

        internal void Visit(VBScriptParser.StartRuleContext stContext)
        {
            var moduleContext = stContext.module();

            VbModule module = new VbModule(this, moduleContext);
            Modules.Add(module);
        }
    }
```

Here is an exemple of node with Prettify function:
```c#
    public class VbOnGotoStatement : VbStatement<VBScriptParser.OnGoToStmtContext>
    {
        public VBValueStatement OnValue { get; set; }
        public VBValueStatement[] GotoValues { get; set; }

        public VbOnGotoStatement(IVBScopeObject context, VBScriptParser.OnGoToStmtContext bloc)
            : base(context, bloc)
        {
            OnValue = VBValueStatement.Get(context, bloc.valueStmt().First());
            GotoValues = bloc.valueStmt().Skip(1).Select(v => VBValueStatement.Get(context, v)).ToArray();
        }

        public override DExpression Prettify(bool partialEvaluation = false)
        {
            return new DCodeBlock($"On {OnValue.Prettify(partialEvaluation)} Goto {string.Join(", ", GotoValues.Select(v=> v.Prettify(partialEvaluation)))}");
        }
    }
```

When the node is visited thanks to constructor `VbOnGotoStatement(`, it recursively visit sub-node and create instance of their own visitors. 
Then, when the Prettify method is called, sub-element are prettified with same partial evaluation level.

## Credits
- This project is under copyright of the **Airbus CERT** and distributed under the **Apache 2.0 license**
