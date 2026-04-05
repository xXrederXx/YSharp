using Xunit;
using YSharp.Common;
using YSharp.Lexer;

namespace YSharp.Tests;

public class BaseTokenTest
{
    [Fact]
    void checkToStringBase_includeTypw()
    {
        BaseToken token = new BaseToken(TokenType.STRING, Position.Null, Position.Null);
        string result = token.ToString();

        Assert.Contains("STRING", result);
    }


    [Fact]
    void checkEquals_whenSame_returnTrue()
    {
        BaseToken token1 = new BaseToken(TokenType.STRING, Position.Null, Position.Null);
        BaseToken token2 = new BaseToken(TokenType.STRING, Position.Null, Position.Null);

        Assert.True(token1.Equals(token2));
    }

    [Fact]
    void checkEquals_whenDifferent_returnFalse()
    {
        BaseToken token1 = new BaseToken(TokenType.STRING, Position.Null, Position.Null);
        BaseToken token2 = new BaseToken(TokenType.STRING, Position.Null.Advance(' '), Position.Null);

        Assert.False(token1.Equals(token2));
    }

    [Fact]
    void checkEquals_whenSameBoxed_returnTrue()
    {
        BaseToken token1 = new BaseToken(TokenType.STRING, Position.Null, Position.Null);
        object token2 = new BaseToken(TokenType.STRING, Position.Null, Position.Null);

        Assert.True(token1.Equals(token2));
    }

    [Fact]
    void checkEquals_whenDifferntBoxed_returnTrue()
    {
        BaseToken token1 = new BaseToken(TokenType.STRING, Position.Null, Position.Null);
        object token2 = 2;

        Assert.False(token1.Equals(token2));
    }

    [Fact]
    void checkEquals_whenDifferentType_returnFalse()
    {
        BaseToken token1 = new BaseToken(TokenType.STRING, Position.Null, Position.Null);
        object token2 = new BaseToken(TokenType.STRING, Position.Null.Advance(' '), Position.Null);

        Assert.False(token1.Equals(token2));
    }

    [Fact]
    void checkEquals_whenNull_returnFalse()
    {
        BaseToken token1 = new BaseToken(TokenType.STRING, Position.Null, Position.Null);

        Assert.False(token1.Equals(null));
    }
}
