namespace YSharp_2._0;

public static class BuiltInFunctionsTable
{
    public static readonly BuiltInFunction print = new("print");
    public static readonly BuiltInFunction input = new("input");
    public static readonly BuiltInFunction run = new("run");
    public static readonly BuiltInFunction timetorun = new("timetorun");
    public static readonly BuiltInFunction time = new("time");

}

public class BuiltInFunction : BaseFunction
{
    private readonly Dictionary<string, List<string>> functionArgs = [];
    public BuiltInFunction(string name) : base(name)
    {
        functionArgs.Add("print", new List<string>(["value"]));
        functionArgs.Add("input", new List<string>([]));
        functionArgs.Add("run", new List<string>(["fileName"]));
        functionArgs.Add("timetorun", new List<string>(["fileName"]));
        functionArgs.Add("time", new List<string>([]));
    }

    public RunTimeResult execute(List<Value> args)
    {
        RunTimeResult res = new();
        Context execContext = GeneratContext();

        res.Regrister(CheckAndPopulateArgs(functionArgs[name], args, execContext));
        if (res.ShouldReturn())
        {
            return res;
        }

        Value? returnValue = null;
        switch (name)
        {
            case "print":
                returnValue = res.Regrister(execPrint(execContext));
                break;
            case "input":
                returnValue = res.Regrister(execInput(execContext));
                break;
            case "run":
                returnValue = res.Regrister(execRun(execContext));
                break;
            case "timetorun":
                returnValue = res.Regrister(execTimeToRun(execContext));
                break;
            case "time":
                returnValue = res.Regrister(execTime(execContext));
                break;
            default:
                no_Visit_methed();
                break;
        }

        if (res.ShouldReturn())
        {
            return res;
        }
        return res.Success(returnValue ?? ValueNull.Instance);
    }
    public RunTimeResult execPrint(Context execContext)
    {
        if (execContext.symbolTable == null)
        {
            return new RunTimeResult().Failure(new RunTimeError(Position._null, "Symbol table is null", context));
        }
        string value = execContext.symbolTable.Get("value").ToString() ?? " - ";
        value = value.Replace('"', ' ');
        Console.WriteLine(value);
        return new RunTimeResult().Success(ValueNull.Instance);
    }
    public static RunTimeResult execInput(Context execContext)
    {
        string inp = Console.ReadLine() ?? "";
        return new RunTimeResult().Success(new String(inp));
    }
    public RunTimeResult execRun(Context execContext)
    {
        if (execContext.symbolTable == null)
        {
            return new RunTimeResult().Failure(new InternalError("symboltable is null"));
        }

        Value fileNameValue = execContext.symbolTable.Get("fileName");
        if (fileNameValue.GetType() != typeof(String))
        {
            return new RunTimeResult().Failure(new RunTimeError(startPos, "first arg must be string", execContext));
        }
        string fileName = ((String)fileNameValue).value;
        string script;
        try
        {
            script = File.ReadAllText(fileName).Replace("\r\n", "\n");
        }
        catch (Exception e)
        {
            return new RunTimeResult().Failure(new RunTimeError(startPos, $"Failed to load script '{fileName}'\n" + e.ToString(), execContext));
        }
        RunClass runClass = new();
        ValueError res = runClass.Run(fileName, script);
        if (res.error.IsError)
        {
            return new RunTimeResult().Failure(res.error);
        }
        return new RunTimeResult().Success(ValueNull.Instance);
    }
    public RunTimeResult execTimeToRun(Context execContext)
    {
       if (execContext.symbolTable == null)
        {
            return new RunTimeResult().Failure(new InternalError("symboltable is null"));
        }

        Value fileNameValue = execContext.symbolTable.Get("fileName");
        if (fileNameValue.GetType() != typeof(String))
        {
            return new RunTimeResult().Failure(new RunTimeError(startPos, "first arg must be string", execContext));
        }
        string fileName = ((String)fileNameValue).value;
        string script;
        try
        {
            script = File.ReadAllText(fileName).Replace("\r\n", "\n");
        }
        catch (Exception e)
        {
            return new RunTimeResult().Failure(new RunTimeError(startPos, $"Failed to load script '{fileName}'\n" + e.ToString(), execContext));
        }
        RunClass runClass = new();
        (Value, Error, List<long>) res = runClass.RunTimed(fileName, script);
        if (res.Item2.IsError)
        {
            return new RunTimeResult().Failure(res.Item2);
        }
        string str = "Init Lexer, Create Tokens, Init Parser, Create AST, Init Interpreter, Init Context, Run Interpreter \n" + string.Join(',', res.Item3);
        return new RunTimeResult().Success(new String(str));
    }
    public static RunTimeResult execTime(Context execContext)
    {
        return new RunTimeResult().Success(new DateTime());
    }
    private void no_Visit_methed()
    {
        throw new NotImplementedException($"No execution method for {name}");
    }

    public override BuiltInFunction copy()
    {
        BuiltInFunction copy = new(name);
        copy.SetPos(startPos, endPos);
        copy.SetContext(context);
        return copy;
    }
    public override string ToString()
    {
        return $"<built-in function {name}>";
    }
}
