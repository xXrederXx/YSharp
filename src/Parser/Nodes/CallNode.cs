using YSharp.Tools.Debug;

namespace YSharp.Parser.Nodes;

public sealed class CallNode : BaseNode
{
    public readonly BaseNode[] ArgNodes;
    public readonly BaseNode NodeToCall;
    public override NodeDebugInfo DebugInfo =>
        new(
            "call",
            NodeDebugShape.Ellipse,
            ArgNodes
                .Select((x, i) => (x.DebugInfo, $"arg[{i}]"))
                .Prepend((NodeToCall.DebugInfo, "Called"))
                .ToList()
        );

    public CallNode(BaseNode nodeToCall, List<BaseNode> argNodes)
        : base(nodeToCall.StartPos, argNodes.Count > 0 ? argNodes[^1].EndPos : nodeToCall.EndPos)
    {
        this.NodeToCall = nodeToCall;
        this.ArgNodes = argNodes.ToArray();
    }

    public override string ToString() =>
        $"{NodeToCall} -> " + string.Join(',', ArgNodes.Select(x => x.ToString()));
}
