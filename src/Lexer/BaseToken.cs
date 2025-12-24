using FastEnumUtility;
using YSharp.Common;

namespace YSharp.Lexer;

public class BaseToken : IEquatable<BaseToken>
{
    public readonly Position EndPos;
    public readonly Position StartPos;
    public readonly TokenType Type;

    public BaseToken(TokenType type, in Position startPos, in Position endPos)
    {
        Type = type;
        StartPos = startPos;
        EndPos = endPos;
    }

    public override string ToString() => Type.FastToString();

    public override int GetHashCode()
    {
        return HashCode.Combine(EndPos, StartPos, Type);
    }

    public override bool Equals(object? obj)
    {
        if (obj is not BaseToken token)
            return false;
        return Equals(token);
    }

    public bool Equals(BaseToken? other)
    {
        if (other is null)
            return false;
        return EndPos == other.EndPos && StartPos == other.StartPos && Type == other.Type;
    }
}

public sealed class NullToken : BaseToken
{
    public static readonly NullToken Instance = new();

    private NullToken()
        : base(TokenType.NULL, Position.Null, Position.Null) { }
}
