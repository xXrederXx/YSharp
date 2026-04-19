using Xunit;
using YSharp.Common;
using YSharp.Lexer;

namespace YSharp.Tests;

public class TokenTest
{
    [Fact]
    public void checkToString_whenValue_includeValue()
    {
        Token<string> token = new Token<string>(TokenType.STRING, "test", Position.Null, Position.Null);
        string result = token.ToString();

        Assert.Contains("test", result);
        Assert.Contains("STRING", result);
    }

    [Fact]
    public void checkToString_whenNoValue_includeValue()
    {
        Token<string> token = new Token<string>(TokenType.STRING, null, Position.Null, Position.Null);
        string result = token.ToString();

        Assert.Contains("STRING", result);
    }

    [Fact]
    public void checkHashCode_shouldIncludeValue()
    {
        Token<string> token = new Token<string>(TokenType.STRING, "test", Position.Null, Position.Null);
        BaseToken baseToken = new BaseToken(TokenType.STRING, Position.Null, Position.Null);
        Assert.NotEqual(baseToken.GetHashCode(), token.GetHashCode());
    }


    [Fact]
    public void checkEquals_whenSame_returnTrue()
    {
        Token<string> token1 = new Token<string>(TokenType.STRING, "test", Position.Null, Position.Null);
        Token<string> token2 = new Token<string>(TokenType.STRING, "test", Position.Null, Position.Null);

        Assert.True(token1.Equals(token2));
    }

    [Fact]
    public void checkEquals_whenDifferent_returnFalse()
    {
        Token<string> token1 = new Token<string>(TokenType.STRING, "test", Position.Null, Position.Null);
        Token<string> token2 = new Token<string>(TokenType.STRING, "test2", Position.Null, Position.Null);

        Assert.False(token1.Equals(token2));
    }

    [Fact]
    public void checkEquals_whenSameBoxed_returnTrue()
    {
        Token<string> token1 = new Token<string>(TokenType.STRING, "test", Position.Null, Position.Null);
        object token2 = new Token<string>(TokenType.STRING, "test", Position.Null, Position.Null);

        Assert.True(token1.Equals(token2));
    }

    [Fact]
    public void checkEquals_whenDifferentType_returnFalse()
    {
        Token<string> token1 = new Token<string>(TokenType.STRING, "test", Position.Null, Position.Null);
        object token2 = new Token<int>(TokenType.STRING, 2, Position.Null, Position.Null);

        Assert.False(token1.Equals(token2));
    }

    [Fact]
    public void checkEquals_whenNull_returnFalse()
    {
        Token<string> token1 = new Token<string>(TokenType.STRING, "test", Position.Null, Position.Null);

        Assert.False(token1.Equals(null));
    }
}
