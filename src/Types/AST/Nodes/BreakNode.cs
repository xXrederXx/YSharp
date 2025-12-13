using YSharp.Types.Common;
using YSharp.Utils.Dot;

namespace YSharp.Types.AST;

public sealed class BreakNode : BaseNode
{
    public BreakNode(in Position startPos, in Position endPos)
        : base(startPos, endPos) { }

    public override NodeDebugInfo DebugInfo => new ("BREAK", NodeDebugShape.Ellipse, []);

    public override string ToString() => "Break Node";
}