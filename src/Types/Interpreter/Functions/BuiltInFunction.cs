using YSharp.Types.Common;
using YSharp.Types.Interpreter.Internal;
using YSharp.Types.Interpreter.Primitives;
using YSharp.Utils;

namespace YSharp.Types.Interpreter.Function;

public static class BuiltInFunctionsTable{
    public static readonly VBuiltInFunction input = new("input");
    public static readonly VBuiltInFunction print = new("print");
    public static readonly VBuiltInFunction run = new("run");
    public static readonly VBuiltInFunction time = new("time");
    public static readonly VBuiltInFunction timetorun = new("timetorun");
}

public sealed class VBuiltInFunction : VBaseFunction{
    private readonly Dictionary<string, List<string>> functionArgs = [];

    public VBuiltInFunction(string name)
        : base(name)
    {
        functionArgs.Add("print", new List<string>(["value"]));
        functionArgs.Add("input", new List<string>([]));
        functionArgs.Add("run", new List<string>(["fileName"]));
        functionArgs.Add("timetorun", new List<string>(["fileName"]));
        functionArgs.Add("time", new List<string>([]));
    }

    private static RunTimeResult ExecInput(Context execContext)
    {
        Console.Write("> ");
        string inp = Console.ReadLine() ?? "";
        return new RunTimeResult().Success(new VString(inp));
    }

    private static RunTimeResult ExecTime() => new RunTimeResult().Success(new VDateTime());

    public override VBuiltInFunction Copy()
    {
        VBuiltInFunction copy = new(name);
        copy.SetPos(startPos, endPos);
        copy.SetContext(context);
        return copy;
    }

    private RunTimeResult ExecPrint(Context execContext)
    {
        if (execContext.symbolTable == null)
        {
            return new RunTimeResult().Failure(
                new InternalSymbolTableError(context)
            );
        }

        string value = execContext.symbolTable.Get("value").ToString() ?? " - ";
        value = value.Replace('"', ' ');
        Console.WriteLine(value);
        return new RunTimeResult().Success(ValueNull.Instance);
    }

    private RunTimeResult ExecRun(Context execContext)
    {
        if (execContext.symbolTable == null)
            return new RunTimeResult().Failure(new InternalSymbolTableError(execContext));

        Value fileNameValue = execContext.symbolTable.Get("fileName");
        if (fileNameValue is not VString)
        {
            return new RunTimeResult().Failure(
                new WrongTypeError(startPos, "first arg must be string", execContext)
            );
        }

        string fileName = ((VString)fileNameValue).value;
        Console.WriteLine(fileName);
        string script;
        try
        {
            script = File.ReadAllText(fileName).Replace("\r\n", "\n");
        }
        catch (Exception e)
        {
            return new RunTimeResult().Failure(
                new FileReadError(
                    startPos,
                    $"Failed to read script '{fileName}'\n" + e,
                    execContext
                )
            );
        }

        RunClass runClass = new();
        ValueAndError res = runClass.Run(fileName, script);
        if (res.Error.IsError) return new RunTimeResult().Failure(res.Error);
        return new RunTimeResult().Success(ValueNull.Instance);
    }

    private RunTimeResult ExecTimeToRun(Context execContext)
    {
        if (execContext.symbolTable == null)
            return new RunTimeResult().Failure(new InternalSymbolTableError(execContext));

        Value fileNameValue = execContext.symbolTable.Get("fileName");
        if (fileNameValue.GetType() != typeof(VString))
        {
            return new RunTimeResult().Failure(
                new WrongTypeError(startPos, "first arg must be string", execContext)
            );
        }

        string fileName = ((VString)fileNameValue).value;
        string script;
        try
        {
            script = File.ReadAllText(fileName).Replace("\r\n", "\n");
        }
        catch (Exception e)
        {
            return new RunTimeResult().Failure(
                new FileReadError(
                    startPos,
                    $"Failed to load script '{fileName}'\n" + e,
                    execContext
                )
            );
        }

        RunClass runClass = new();
        (Value, Error, List<long>) res = runClass.RunTimed(fileName, script);
        if (res.Item2.IsError) return new RunTimeResult().Failure(res.Item2);
        string str =
            "Init Lexer, Create Tokens, Init Parser, Create AST, Init Context, Run Interpreter, Whole Time (ms) \n"
            + string.Join(',', res.Item3);
        return new RunTimeResult().Success(new VString(str));
    }

    public override RunTimeResult Execute(List<Value> args)
    {
        RunTimeResult res = new();
        Context execContext = GeneratContext();

        res.Regrister(CheckAndPopulateArgs(functionArgs[name], args, execContext));
        if (res.ShouldReturn()) return res;

        Value returnValue = ValueNull.Instance;
        switch (name)
        {
            case "print":
                returnValue = res.Regrister(ExecPrint(execContext));
                break;
            case "input":
                returnValue = res.Regrister(ExecInput(execContext));
                break;
            case "run":
                returnValue = res.Regrister(ExecRun(execContext));
                break;
            case "timetorun":
                returnValue = res.Regrister(ExecTimeToRun(execContext));
                break;
            case "time":
                returnValue = res.Regrister(ExecTime());
                break;
            default:
                No_Visit_methed();
                break;
        }

        if (res.ShouldReturn()) return res;
        return res.Success(returnValue);
    }

    public override string ToString() => $"<built-in function {name}>";

    private void No_Visit_methed()
    {
        throw new NotImplementedException($"No execution method for {name}");
    }
}