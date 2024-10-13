namespace YSharp_2._0;

public class BaseFunction : Value
{
    public readonly string name;
    public BaseFunction(string? name) : base()
    {
        if (name == null)
        {
            this.name = "<anonymous>";
        }
        else
        {
            this.name = name;
        }
    }
    protected Context GeneratContext()
    {
        Context newContext = new(name, context, startPos);

        if (newContext.parent == null)
        {
            Console.WriteLine("Imposible, parent of child context is null");
        }
        else
        {
            newContext.symbolTable = new SymbolTable().parent = newContext.parent.symbolTable;
        }

        return newContext;
    }

    protected RunTimeResult CheckArgs(List<string> argNames, List<Value> args)
    {
        RunTimeResult res = new();

        if (args.Count > argNames.Count)
        {
            return res.Failure(new RunTimeError(startPos, $" {args.Count - argNames.Count} too many args passed into {name}", context));
        }
        if (args.Count < argNames.Count)
        {
            return res.Failure(new RunTimeError(startPos, $" {argNames.Count - args.Count} too few args passed into {name}", context));
        }
        return res.Success(ValueNull.Instance);
    }
    protected static void PopulateArgs(List<string> argNames, List<Value> args, Context execContext)
    {
        if (execContext.symbolTable == null)
        {
            Console.WriteLine("Symbol table is null");
            return;
        }
        for (int i = 0; i < args.Count; i++)
        {
            string argName = argNames[i];
            Value argValue = args[i];
            argValue.SetContext(execContext);
            execContext.symbolTable.Set(argName, argValue);
        }
    }
    protected RunTimeResult CheckAndPopulateArgs(List<string> argNames, List<Value> args, Context execContext)
    {
        RunTimeResult res = new();
        res.Regrister(CheckArgs(argNames, args));
        if (res.ShouldReturn())
        {
            return res;
        }
        PopulateArgs(argNames, args, execContext);
        return res.Success(ValueNull.Instance);
    }

}

