using YSharp.Common;
using YSharp.Tools.Debug;

namespace YSharp.Parser.Nodes;

public sealed class ReturnNode : BaseNode
{
    public readonly BaseNode? NodeToReturn;

    public override NodeDebugInfo DebugInfo =>
        new(
            "return",
            NodeDebugShape.Ellipse,
            NodeToReturn is not null ? [(NodeToReturn.DebugInfo, "to return")] : []
        );

    public ReturnNode(BaseNode? nodeToReturn, in Position startPos, in Position endPos)
        : base(startPos, endPos)
    {
        this.NodeToReturn = nodeToReturn;
    }

    public override string ToString() => $"return {NodeToReturn}";
}
