using YSharp.Types.Common;

namespace YSharp.Types.AST;

public class ParseResult
{
    public Error Error { get; private set; } = ErrorNull.Instance;
    public INode Node { get; private set; } = NodeNull.Instance;
    public int ToReverseCount { get; private set; } = 0;
    public bool HasError => Error.IsError;
    private int _advanceCount = 0;

    public void ResetError()
    {
        Error = ErrorNull.Instance;
    }

    public INode? TryRegister(ParseResult result)
    {
        if (result.HasError)
        {
            ToReverseCount = result._advanceCount;
            return null;
        }
        return Register(result);
    }

    public INode Register(ParseResult result)
    {
        _advanceCount += result._advanceCount;
        Error = result.Error;
        return result.Node;
    }

    public void Advance()
    {
        _advanceCount++;
    }

    public ParseResult Success(INode node)
    {
        Node = node;
        return this;
    }

    public ParseResult Failure(Error error)
    {
        if (!HasError || _advanceCount == 0)
        {
            Error = error;
        }
        return this;
    }

    public override string ToString() =>
        $"Node: {Node} / Error: {Error} / AdvanceCount: {_advanceCount}";
}
