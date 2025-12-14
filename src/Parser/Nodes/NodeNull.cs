using YSharp.Common;
using YSharp.Tools.Debug;

namespace YSharp.Parser.Nodes;


public sealed class NodeNull : BaseNode
{
    public static readonly NodeNull Instance = new();

    public override NodeDebugInfo DebugInfo => new("null", NodeDebugShape.Ellipse, []);
    private NodeNull()
        : base(Position.Null, Position.Null) { }


    public override string ToString() => "Null Node";
}