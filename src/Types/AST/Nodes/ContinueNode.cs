using YSharp.Types.Common;
using YSharp.Utils.Dot;

namespace YSharp.Types.AST;

public sealed class ContinueNode : BaseNode
{
    public override NodeDebugInfo DebugInfo => new("continue", NodeDebugShape.Ellipse, []);
    public ContinueNode(in Position startPos, in Position endPos)
        : base(startPos, endPos) { }


    public override string ToString() => "Continue Node";
}