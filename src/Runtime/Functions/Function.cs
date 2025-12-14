using YSharp.Parser.Nodes;

namespace YSharp.Runtime.Functions;

public sealed class VFunction(
    string? name,
    BaseNode bodyNode,
    List<string> argNames,
    bool autoReturn
) : VBaseFunction(name)
{
    public override VFunction Copy()
    {
        VFunction copy = new(name, bodyNode, argNames, autoReturn);
        copy.SetContext(context);
        copy.SetPos(startPos, endPos);
        return copy;
    }

    /// <summary>
    ///     This executes the code in the function.
    /// </summary>
    /// <param name="args"></param>
    /// <returns>A Runtime Result</returns>
    public override RunTimeResult Execute(List<Value> args)
    {
        RunTimeResult res = new();
        Context execContext = GeneratContext();

        res.Regrister(CheckAndPopulateArgs(argNames, args, execContext));
        if (res.ShouldReturn())
            return res;

        Value value = res.Regrister(Interpreter.Visit(bodyNode, execContext));
        if (res.ShouldReturn() && res.funcReturnValue is ValueNull)
            return res;
        Value retValue = autoReturn ? value : res.funcReturnValue;
        return res.Success(retValue);
    }

    public override string ToString() => $"<func {name}>";
}
