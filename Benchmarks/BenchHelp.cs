using BenchmarkDotNet.Running;
using YSharp.Core;
using YSharp.Types.AST;
using YSharp.Types.Interpreter;
using YSharp.Types.Interpreter.ClassTypes;
using YSharp.Types.Interpreter.FunctionTypes;
using YSharp.Types.Lexer;
using YSharp.Utils;

namespace YSharp.Benchmarks;

public static class BenchHelp
{
    public static readonly string SampleText =
        "FUN func(a, b, c, d, e, f, g);\n PRINT(a + b + c); VAR x =d+ e + f + g; PRINT(x); END; 1 + 2 FUN add(a, b); RETURN a + b;END;FUN adda(a, b);  RETURN a + b;END;;;1+2; VAR list = [1,2,3,4,5,6,6,7,7,8,8,8,9,9,9,4,42,3243,43,64535,6434,43,3]; 1 + 2 + 3+ 44 +45 + 3 + 43 +4";

    public static readonly string Text5000Char;
    public static readonly string Text10000Char;
    public static readonly string Text50000Char;
    public static readonly string Text100000Char;

    public static readonly List<IToken> TokenS;
    public static readonly List<IToken> TokenM;
    public static readonly List<IToken> TokenL;
    public static readonly List<IToken> TokenXL;

    public static readonly ParseResult astS;
    public static readonly ParseResult astM;
    public static readonly ParseResult astL;
    public static readonly ParseResult astXL;

    static BenchHelp()
    {
        Text5000Char = GetText(5000);
        Text10000Char = GetText(10000) + ";END;1+1";
        Text50000Char = GetText(50000) + ";PRINT(a);END;1+1";
        Text100000Char = GetText(100000) + ";PRINT(a);END;1+1";

        CheckString(Text5000Char);
        CheckString(Text10000Char);
        CheckString(Text50000Char);
        CheckString(Text100000Char);

        TokenS = new Lexer(Text5000Char, "BenchHelp").MakeTokens().Item1;
        TokenM = new Lexer(Text10000Char, "BenchHelp").MakeTokens().Item1;
        TokenL = new Lexer(Text50000Char, "BenchHelp").MakeTokens().Item1;
        TokenXL = new Lexer(Text100000Char, "BenchHelp").MakeTokens().Item1;

        Parser parserS = new(TokenS);
        astS = parserS.Parse();
        Parser parserM = new(TokenM);
        astM = parserM.Parse();
        astL = new Parser(TokenL).Parse();
        astXL = new Parser(TokenXL).Parse();
    }

    public static SymbolTable GetSymbolTable()
    {
        SymbolTable SampleSymbolTable = new();

        SampleSymbolTable.Set("TRUE", new VBool(true));
        SampleSymbolTable.Set("FALSE", new VBool(false));

        SampleSymbolTable.Set("MATH", new VMath());
        SampleSymbolTable.Set("PRINT", BuiltInFunctionsTable.print);
        SampleSymbolTable.Set("INPUT", BuiltInFunctionsTable.input);
        SampleSymbolTable.Set("RUN", BuiltInFunctionsTable.run);
        SampleSymbolTable.Set("TIMETORUN", BuiltInFunctionsTable.timetorun);
        SampleSymbolTable.Set("TIME", BuiltInFunctionsTable.time);

        return SampleSymbolTable;
    }

    private static void CheckString(string str)
    {
        RunClass runClass = new();
        var res = runClass.Run("BENCH-HELPER-CHECK", str);
        if (res.Item2.IsError)
        {
            System.Console.WriteLine(
                $"DID NOT PASS BENCH-HELPER-CHECK \n\n ERROR: \n\t{res.Item2} \n\n TEXT: \n {BetterString(str)}"
            );
        }
    }

    private static string GetText(int length)
    {
        string text = "";
        while (text.Length < length)
        {
            text += SampleText;
        }
        text = text.Substring(0, length);
        text = text.Substring(0, text.LastIndexOf(';'));
        return text;
    }

    private static string BetterString(string str, int length = 120)
    {
        if (string.IsNullOrEmpty(str) || str.Length <= length)
            return str;

        int halfLength = length / 2;
        string start = str.Substring(0, halfLength);
        string end = str.Substring(str.Length - halfLength);
        return start + " ...... " + end;
    }

    public static void LogData()
    {
        System.Console.WriteLine("\nBENCH HELPER DATA");
        System.Console.WriteLine(
            $"SAMPLE TEXT ({SampleText.Length} Char):\n{BetterString(SampleText)}"
        );

        System.Console.WriteLine($"\nTEXT VARIANTS:");
        System.Console.WriteLine(
            $"\tSHORT ({Text5000Char.Length} Char): \n\t {BetterString(Text5000Char)}"
        );
        System.Console.WriteLine(
            $"\tMEDIUM ({Text10000Char.Length} Char): \n\t {BetterString(Text10000Char)}"
        );
        System.Console.WriteLine(
            $"\tLONG ({Text50000Char.Length} Char): \n\t {BetterString(Text50000Char)}"
        );
        System.Console.WriteLine(
            $"\tEXTREME ({Text100000Char.Length} Char): \n\t {BetterString(Text100000Char)}"
        );

        System.Console.WriteLine($"\nTOKEN LIST VARIANTS:");
        System.Console.WriteLine($"\tSHORT -> {TokenS.Count} Tokens (LAST: {TokenS[^2]})");
        System.Console.WriteLine($"\tMEDIUM -> {TokenM.Count} Tokens (LAST: {TokenM[^2]})");
        System.Console.WriteLine($"\tLONG -> {TokenL.Count} Tokens (LAST: {TokenL[^2]})");
        System.Console.WriteLine($"\tEXTREME -> {TokenXL.Count} Tokens (LAST: {TokenXL[^2]})");

        System.Console.WriteLine($"\nAST VARIANTS:");
        System.Console.WriteLine($"\tSHORT -> {astS.Node.ToString()?.Length}");
        System.Console.WriteLine($"\tMEDIUM -> {astM.Node.ToString()?.Length}");
        System.Console.WriteLine($"\tLONG -> {astL.Node.ToString()?.Length}");
        System.Console.WriteLine($"\tEXTREME -> {astXL.Node.ToString()?.Length}");
    }

    public static void Run<T>(string changeDescription = "-")
    {
        BenchmarkDotNet.Reports.Summary summary = BenchmarkRunner.Run<T>();
        BenchReportWriter.UpdateFiles<T>(changeDescription);
    }
}
