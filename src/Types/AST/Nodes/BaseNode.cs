using YSharp.Types.Common;

// ReSharper disable ConvertToPrimaryConstructor

namespace YSharp.Types.AST;

public abstract class BaseNode
{
    public Position EndPos;

    public Position StartPos;

    protected BaseNode(in Position startPos, in Position endPos)
    {
        StartPos = startPos;
        EndPos = endPos;
    }
}