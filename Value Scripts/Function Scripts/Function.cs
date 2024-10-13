namespace YSharp_2._0;

public class Function(string? name, Node bodyNode, List<string> argNames, bool autoReturn) : BaseFunction(name)
{
    public readonly Node bodyNode = bodyNode;
    public readonly List<string> argNames = argNames;
    public readonly bool autoReturn = autoReturn;

    /// <summary>
    /// This executes the code in the function.
    /// </summary>
    /// <param name="args"></param>
    /// <returns>A Runtime Result</returns>
    public RunTimeResult Execute(List<Value> args)
    {
        RunTimeResult res = new();
        Interpreter interpreter = new();
        Context execContext = GeneratContext();

        res.Regrister(CheckAndPopulateArgs(argNames, args, execContext));
        if (res.ShouldReturn())
        {
            return res;
        }

        Value value = res.Regrister(interpreter.Visit(bodyNode, execContext));
        if (res.ShouldReturn() && res.funcReturnValue is ValueNull)
        {
            return res;
        }
        Value retValue = autoReturn ? value : res.funcReturnValue;
        return res.Success(retValue);
    }

    public override Function copy()
    {
        Function copy = new(name, bodyNode, argNames, autoReturn);
        copy.SetContext(context);
        copy.SetPos(startPos, endPos);
        return copy;
    }
    public override string ToString()
    {
        return $"<func {name}>";
    }
}

