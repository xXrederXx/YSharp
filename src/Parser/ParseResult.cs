using YSharp.Common;
using YSharp.Parser.Nodes;

namespace YSharp.Parser;

/// <summary>
/// This is the result of  a Parser Operation. It carries Errors or Nodes. In addition it has a advance and reverse count
/// </summary>
public class ParseResult
{
    /// <summary>
    /// Error if set, ErrorNull otherwise
    /// </summary>
    public Error Error { get; private set; } = ErrorNull.Instance;
    public bool HasError => Error.IsError;

    /// <summary>
    /// Node if set, NodeNull otherwise
    /// </summary>
    public BaseNode Node { get; private set; } = NodeNull.Instance;
    public int ToReverseCount { get; private set; }
    private int _advanceCount;

    /// <summary>
    /// Advances the Count by one.
    /// </summary>
    public void Advance()
    {
        _advanceCount++;
    }

    /// <summary>
    /// Indicates that an operation failed. It sets or overrides the error and returns it self.
    /// </summary>
    /// <param name="error">The generated error from the operation</param>
    /// <returns>it returns itself</returns>
    public ParseResult Failure(Error error)
    {
        Error = error;
        return this;
    }

    /// <summary>
    /// Register takes another result and essentialy adds it to itself. It combines the advance counts and overrides the error.
    /// </summary>
    /// <param name="result">The other result</param>
    /// <returns>The node from the other result</returns>
    public BaseNode Register(ParseResult result)
    {
        _advanceCount += result._advanceCount;
        Error = result.Error;
        return result.Node;
    }

    /// <summary>
    /// Indicates that an operation succeeded. It sets a node which was generated during the operation and returns itself.
    /// </summary>
    /// <param name="node">Generated Node</param>
    /// <returns>it returns itself</returns>
    public ParseResult Success(BaseNode node)
    {
        Node = node;
        return this;
    }

    public override string ToString() =>
        $"Node: {Node} / Error: {Error} / AdvanceCount: {_advanceCount}";
}
