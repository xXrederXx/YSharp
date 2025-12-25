using YSharp.Common;

namespace YSharp.Lexer;

public sealed class NullToken : BaseToken
{
    public static readonly NullToken Instance = new();

    private NullToken()
        : base(TokenType.NULL, Position.Null, Position.Null) { }
}
