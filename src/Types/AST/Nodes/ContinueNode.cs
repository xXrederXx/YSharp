using YSharp.Types.Common;

namespace YSharp.Types.AST;

public sealed class ContinueNode : BaseNode
{
    public ContinueNode(in Position startPos, in Position endPos)
        : base(startPos, endPos) { }

    public override string ToString() => "Continue Node";
}