using YSharp.Types.Common;
using YSharp.Utils.Dot;

namespace YSharp.Types.AST;

public sealed class NodeNull : BaseNode
{
    public static readonly NodeNull Instance = new();

    public override NodeDebugInfo DebugInfo => new("null", NodeDebugShape.Ellipse, []);
    private NodeNull()
        : base(Position.Null, Position.Null) { }


    public override string ToString() => "Null Node";
}