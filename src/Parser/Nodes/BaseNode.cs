using YSharp.Common;
using YSharp.Tools.Debug;

namespace YSharp.Parser.Nodes;

public abstract class BaseNode : INodeDescriptor
{
    public readonly Position EndPos;

    public readonly Position StartPos;

    public abstract NodeDebugInfo DebugInfo { get; init; }

    protected BaseNode(in Position startPos, in Position endPos)
    {
        StartPos = startPos;
        EndPos = endPos;
    }
}
