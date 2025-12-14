using YSharp.Common;
using YSharp.Tools.Debug;

namespace YSharp.Parser.Nodes;


public sealed class ListNode : BaseNode
{
    public readonly BaseNode[] ElementNodes;

    public override NodeDebugInfo DebugInfo =>
        new(
            "list",
            NodeDebugShape.SpecialSquare,
            ElementNodes.Select((x, i) => (x.DebugInfo, $"elem[{i}]")).ToList()
        );

    public ListNode(List<BaseNode> elementNodes, in Position posStart, in Position posEnd)
        : base(posStart, posEnd)
    {
        this.ElementNodes = elementNodes.ToArray();
    }

    public override string ToString()
    {
        return "[" + string.Join(',', ElementNodes.Select(x => x.ToString())) + "]";
    }
}
