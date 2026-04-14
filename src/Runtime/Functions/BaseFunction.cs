using YSharp.Common;

namespace YSharp.Runtime.Functions;

public abstract class VBaseFunction : Value
{
    public readonly string name;

    public VBaseFunction(string? name)
    {
        if (name == null)
            this.name = "<anonymous>";
        else
            this.name = name;
    }

    public abstract RunTimeResult Execute(List<Value> args);

    protected static RunTimeResult CheckAndPopulateArgs(
        List<string> argNames,
        List<Value> args,
        Context execContext
    )
    {
        RunTimeResult res = new();
        res.Register(CheckArgs(argNames, args, execContext));
        if (res.ShouldReturn())
            return res;
        Error err = PopulateArgs(argNames, args, execContext);
        if (err.IsError)
        {
            return res.Failure(err);
        }
        return res.Success(ValueNull.Instance);
    }

    protected static RunTimeResult CheckArgs(
        List<string> argNames,
        List<Value> args,
        Context context
    )
    {
        RunTimeResult res = new();
        Error err = ValueHelper.IsRightLength(argNames.Count, args, context);
        if (err.IsError)
            return res.Failure(err);
        return res.Success(ValueNull.Instance);
    }

    protected static Error PopulateArgs(
        List<string> argNames,
        List<Value> args,
        Context execContext
    )
    {
        if (execContext.SymbolTable == null)
        {
            return new InternalSymbolTableError(execContext);
        }

        for (int i = 0; i < args.Count; i++)
        {
            string argName = argNames[i];
            Value argValue = args[i];
            argValue.SetContext(execContext);
            execContext.SymbolTable.Set(argName, argValue);
        }
        return ErrorNull.Instance;
    }

    protected Context GeneratContext()
    {
        Context newContext = new(
            name,
            Context,
            StartPos,
            new SymbolTable() { Parent = Context?.SymbolTable }
        );
        return newContext;
    }
}
