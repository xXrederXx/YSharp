using YSharp.Types.Common;

namespace YSharp.Types.AST;

public sealed class BreakNode : BaseNode
{
    public BreakNode(in Position startPos, in Position endPos)
        : base(startPos, endPos) { }

    public override string ToString() => "Break Node";
}