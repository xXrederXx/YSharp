using YSharp.Core;
using YSharp.Types.Common;

namespace YSharp.Types.Interpreter.FunctionTypes;

public class VBaseFunction : Value
{
    public readonly string name;

    public VBaseFunction(string? name)
        : base()
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

    protected static RunTimeResult CheckArgs(
        List<string> argNames,
        List<Value> args,
        Context context
    )
    {
        RunTimeResult res = new();
        Error err = ValueHelper.IsRightLength(argNames.Count, args, context);
        if (err.IsError)
        {
            return res.Failure(err);
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

    protected static RunTimeResult CheckAndPopulateArgs(
        List<string> argNames,
        List<Value> args,
        Context execContext
    )
    {
        RunTimeResult res = new();
        res.Regrister(CheckArgs(argNames, args, execContext));
        if (res.ShouldReturn())
        {
            return res;
        }
        PopulateArgs(argNames, args, execContext);
        return res.Success(ValueNull.Instance);
    }
}
