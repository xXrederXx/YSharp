using YSharp.Types.Common;

namespace YSharp.Types.AST;

public sealed class ReturnNode : BaseNode
{
    public readonly BaseNode? NodeToReturn;

    public ReturnNode(BaseNode? nodeToReturn, in Position startPos, in Position endPos)
        : base(startPos, endPos)
    {
        this.NodeToReturn = nodeToReturn;
    }

    public override string ToString() => $"return {NodeToReturn}";
}