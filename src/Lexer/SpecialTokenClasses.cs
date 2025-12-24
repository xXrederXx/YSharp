using YSharp.Common;

namespace YSharp.Lexer;

public class TokenNoValue : IToken, IEquatable<TokenNoValue>
{
    public Position EndPos { get; }
    public Position StartPos { get; }
    public TokenType Type { get; }

    public TokenNoValue(TokenType type, in Position startPos, in Position endPos)
    {
        Type = type;
        StartPos = startPos;
        EndPos = endPos;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(EndPos, StartPos, Type);
    }

    public override bool Equals(object? obj)
    {
        if (obj is not TokenNoValue token)
            return false;
        return Equals(token);
    }

    public bool Equals(TokenNoValue? other)
    {
        if (other is null)
            return false;
        return EndPos == other.EndPos && StartPos == other.StartPos && Type == other.Type;
    }
}

public sealed class NullToken : TokenNoValue
{
    public static readonly NullToken Instance = new();

    private NullToken()
        : base(TokenType.NULL, Position.Null, Position.Null) { }
}
