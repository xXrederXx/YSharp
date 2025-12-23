using YSharp.Common;

namespace YSharp.Lexer;

public class TokenNoValue : IToken
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
}

public sealed class NullToken : TokenNoValue
{
    public static readonly NullToken Instance = new();

    private NullToken()
        : base(TokenType.NULL, Position.Null, Position.Null) { }
}
