using YSharp.Types.Common;

namespace YSharp.Types.AST;

public class ParseResult{
    public Error Error { get; private set; } = ErrorNull.Instance;
    public bool HasError => Error.IsError;
    public BaseNode Node { get; private set; } = NodeNull.Instance;
    public int ToReverseCount { get; private set; }
    private int _advanceCount;

    public void Advance()
    {
        _advanceCount++;
    }

    public ParseResult Failure(Error error)
    {
        if (!HasError || _advanceCount == 0) Error = error;
        return this;
    }

    public BaseNode Register(ParseResult result)
    {
        _advanceCount += result._advanceCount;
        Error = result.Error;
        return result.Node;
    }

    public void ResetError()
    {
        Error = ErrorNull.Instance;
    }

    public ParseResult Success(BaseNode node)
    {
        Node = node;
        return this;
    }

    public override string ToString() =>
        $"Node: {Node} / Error: {Error} / AdvanceCount: {_advanceCount}";

    public BaseNode? TryRegister(ParseResult result)
    {
        if (result.HasError)
        {
            ToReverseCount = result._advanceCount;
            return null;
        }

        return Register(result);
    }
}