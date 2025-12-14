using YSharp.Common;
using YSharp.Tools.Debug;

namespace YSharp.Parser.Nodes;

public abstract class BaseNode : INodeDescriptor
{
    public Position EndPos;

    public Position StartPos;

    public abstract NodeDebugInfo DebugInfo { get; }

    protected BaseNode(in Position startPos, in Position endPos)
    {
        StartPos = startPos;
        EndPos = endPos;
    }
}
