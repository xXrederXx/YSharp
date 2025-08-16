using YSharp.Types.AST;

namespace YSharp.Types.Interpreter.Function;

public sealed class VFunction(
    string? name,
    BaseNode bodyNode,
    List<string> argNames,
    bool autoReturn
) : VBaseFunction(name)
{
    public readonly BaseNode bodyNode = bodyNode;
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
        Context execContext = GeneratContext();

        res.Regrister(CheckAndPopulateArgs(argNames, args, execContext));
        if (res.ShouldReturn())
        {
            return res;
        }

        Value value = res.Regrister(Core.Interpreter.Visit(bodyNode, execContext));
        if (res.ShouldReturn() && res.funcReturnValue is ValueNull)
        {
            return res;
        }
        Value retValue = autoReturn ? value : res.funcReturnValue;
        return res.Success(retValue);
    }

    public override VFunction Copy()
    {
        VFunction copy = new(name, bodyNode, argNames, autoReturn);
        copy.SetContext(context);
        copy.SetPos(startPos, endPos);
        return copy;
    }

    public override string ToString()
    {
        return $"<func {name}>";
    }
}
