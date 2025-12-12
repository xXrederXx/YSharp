using YSharp.Types.Common;

namespace YSharp.Types.AST;

public sealed class NodeNull : BaseNode
{
    public static readonly NodeNull Instance = new();

    private NodeNull()
        : base(Position.Null, Position.Null) { }

    public override string ToString() => "Null Node";
}