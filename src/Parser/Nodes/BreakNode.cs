using YSharp.Common;
using YSharp.Tools.Debug;

namespace YSharp.Parser.Nodes;

public sealed class BreakNode : BaseNode
{
    public BreakNode(in Position startPos, in Position endPos)
        : base(startPos, endPos) { }

    public override NodeDebugInfo DebugInfo => new("BREAK", NodeDebugShape.Ellipse, []);

    public override string ToString() => "Break Node";
}
