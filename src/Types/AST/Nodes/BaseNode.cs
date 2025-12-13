using YSharp.Types.Common;
using YSharp.Utils.Dot;

// ReSharper disable ConvertToPrimaryConstructor

namespace YSharp.Types.AST;

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
