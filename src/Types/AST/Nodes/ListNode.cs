using YSharp.Types.Common;

namespace YSharp.Types.AST;

public sealed class ListNode : BaseNode
{
    public readonly BaseNode[] ElementNodes;

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