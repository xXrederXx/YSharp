using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using YSharp.Common;
using YSharp.Lexer;
using YSharp.Parser;
using YSharp.Runtime;
using YSharp.Runtime.Functions;
using YSharp.Runtime.Primatives.Bool;
using YSharp.Runtime.Utils.Math;
using YSharp.Util;

namespace YSharp.Benchmarks;

public static class BenchHelp
{
    public static readonly ParseResult astL;
    public static readonly ParseResult astM;

    public static readonly ParseResult astS;
    public static readonly ParseResult astXL;

    public static readonly string SampleText =
        "FUN func(a, b, c, d, e, f, g);\n PRINT(a + b + c); VAR x =d+ e + f + g; PRINT(x); END; 1 + 2 FUN add(a, b); RETURN a + b;END;FUN adda(a, b);  RETURN a + b;END;;;1+2; VAR list = [1,2,3,4,5,6,6,7,7,8,8,8,9,9,9,4,42,3243,43,64535,6434,43,3]; 1 + 2 + 3+ 44 +45 + 3 + 43 +4";

    public static readonly string Text100000Char;
    public static readonly string Text10000Char;
    public static readonly string Text50000Char;

    public static readonly string Text5000Char;
    public static readonly List<BaseToken> TokenL;
    public static readonly List<BaseToken> TokenM;

    public static readonly List<BaseToken> TokenS;
    public static readonly List<BaseToken> TokenXL;

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

        TokenS = new Lexer.Lexer(Text5000Char, "BenchHelp").MakeTokens().Item1;
        TokenM = new Lexer.Lexer(Text10000Char, "BenchHelp").MakeTokens().Item1;
        TokenL = new Lexer.Lexer(Text50000Char, "BenchHelp").MakeTokens().Item1;
        TokenXL = new Lexer.Lexer(Text100000Char, "BenchHelp").MakeTokens().Item1;

        Parser.Parser parserS = new(TokenS);
        astS = parserS.Parse();
        Parser.Parser parserM = new(TokenM);
        astM = parserM.Parse();
        astL = new Parser.Parser(TokenL).Parse();
        astXL = new Parser.Parser(TokenXL).Parse();
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

    public static void LogData()
    {
        Console.WriteLine("\nBENCH HELPER DATA");
        Console.WriteLine(
            $"SAMPLE TEXT ({SampleText.Length} Char):\n{BetterString(SampleText)}"
        );

        Console.WriteLine("\nTEXT VARIANTS:");
        Console.WriteLine(
            $"\tSHORT ({Text5000Char.Length} Char): \n\t {BetterString(Text5000Char)}"
        );
        Console.WriteLine(
            $"\tMEDIUM ({Text10000Char.Length} Char): \n\t {BetterString(Text10000Char)}"
        );
        Console.WriteLine(
            $"\tLONG ({Text50000Char.Length} Char): \n\t {BetterString(Text50000Char)}"
        );
        Console.WriteLine(
            $"\tEXTREME ({Text100000Char.Length} Char): \n\t {BetterString(Text100000Char)}"
        );

        Console.WriteLine("\nTOKEN LIST VARIANTS:");
        Console.WriteLine($"\tSHORT -> {TokenS.Count} Tokens (LAST: {TokenS[^2]})");
        Console.WriteLine($"\tMEDIUM -> {TokenM.Count} Tokens (LAST: {TokenM[^2]})");
        Console.WriteLine($"\tLONG -> {TokenL.Count} Tokens (LAST: {TokenL[^2]})");
        Console.WriteLine($"\tEXTREME -> {TokenXL.Count} Tokens (LAST: {TokenXL[^2]})");

        Console.WriteLine("\nAST VARIANTS:");
        Console.WriteLine($"\tSHORT -> {astS.Node.ToString()?.Length}");
        Console.WriteLine($"\tMEDIUM -> {astM.Node.ToString()?.Length}");
        Console.WriteLine($"\tLONG -> {astL.Node.ToString()?.Length}");
        Console.WriteLine($"\tEXTREME -> {astXL.Node.ToString()?.Length}");
    }

    public static void Run<T>(string changeDescription = "-")
    {
        Summary summary = BenchmarkRunner.Run<T>();
        BenchReportWriter.UpdateFiles<T>(changeDescription);
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

    private static void CheckString(string str)
    {
        RunClass runClass = new();
        (Value, Error) res = runClass.Run("BENCH-HELPER-CHECK", str);
        if (res.Item2.IsError)
        {
            Console.WriteLine(
                $"DID NOT PASS BENCH-HELPER-CHECK \n\n ERROR: \n\t{res.Item2} \n\n TEXT: \n {BetterString(str)}"
            );
        }
    }

    private static string GetText(int length)
    {
        string text = "";
        while (text.Length < length) text += SampleText;
        text = text.Substring(0, length);
        text = text.Substring(0, text.LastIndexOf(';'));
        return text;
    }
}
