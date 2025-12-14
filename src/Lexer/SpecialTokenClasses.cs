using YSharp.Common;

namespace YSharp.Lexer;

public sealed class TokenNoValue : Token<TokenNoValueType>
{
    public TokenNoValue(TokenType type, in Position startPos, in Position endPos)
        : base(type, TokenNoValueType.Instance, startPos, endPos) { }
}

public sealed class NullToken : Token<TokenNoValueType>
{
    public static readonly NullToken Instance = new();

    private NullToken()
        : base(TokenType.NULL, TokenNoValueType.Instance, Position.Null, Position.Null) { }
}

public class TokenNoValueType
{
    public static readonly TokenNoValueType Instance = new();

    private TokenNoValueType() { }
}
