using YSharp.Common;
using YSharp.Lexer;

namespace YSharp.Tests;

public static class TestingConstans
{
    public const double DOUBLE_PRECISION = 1e-12;
    public static readonly TimeSpan TIME_PRECISION = new TimeSpan(0, 0, 0, 0, 50); // 50ms
    public static Token<string> MakeToken(string value) => new Token<string>(TokenType.STRING, value, Position.Null, Position.Null);
}
