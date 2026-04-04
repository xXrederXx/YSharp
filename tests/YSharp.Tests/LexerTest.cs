using Xunit;
using YSharp.Common;
using YSharp.Lexer;

namespace YSharp.Tests;

using LexerResult = Result<List<BaseToken>, Error>;

public class LexerTest
{
    [Theory]
    [InlineData("*=", TokenType.MUEQ)]
    [InlineData("*", TokenType.MUL)]
    [InlineData("/=", TokenType.DIEQ)]
    [InlineData("/", TokenType.DIV)]
    [InlineData("==", TokenType.EE)]
    [InlineData("=", TokenType.EQ)]
    [InlineData("<=", TokenType.LTE)]
    [InlineData("<", TokenType.LT)]
    [InlineData(">=", TokenType.GTE)]
    [InlineData(">", TokenType.GT)]
    void checkMakeDecision_shouldReturnRightTokens(string input, TokenType expected)
    {
        LexerResult result = new Lexer.Lexer(input, "TEST").MakeTokens();

        Assert.True(result.TryGetValue(out List<BaseToken> tokens));
        Assert.Equal(2, tokens.Count); // token + EOF Token
        Assert.Equal(expected, tokens.First().Type);
    }

    [Theory]
    [InlineData("+", TokenType.PLUS)]
    [InlineData("+=", TokenType.PLEQ)]
    [InlineData("++", TokenType.PP)]
    [InlineData("-", TokenType.MINUS)]
    [InlineData("-=", TokenType.MIEQ)]
    [InlineData("--", TokenType.MM)]
    [InlineData("->", TokenType.ARROW)]
    void checkMakePlusMinus_shouldReturnRightTokens(string input, TokenType expected)
    {
        LexerResult result = new Lexer.Lexer(input, "TEST").MakeTokens();

        Assert.True(result.TryGetValue(out List<BaseToken> tokens));
        Assert.Equal(2, tokens.Count); // token + EOF Token
        Assert.Equal(expected, tokens.First().Type);
    }

    [Fact]
    void checkMakeString_whenNormal_shouldReturnRightTokens()
    {
        LexerResult result = new Lexer.Lexer("\"aA bB cC 1 2 3 !\"", "TEST").MakeTokens();

        Assert.True(result.TryGetValue(out List<BaseToken> tokens));
        Assert.Equal(2, tokens.Count); // token + EOF Token
        Assert.Equal(TokenType.STRING, tokens.First().Type);

        Token<string> strToken = Assert.IsType<Token<string>>(tokens.First());
        Assert.Equal("aA bB cC 1 2 3 !", strToken.Value);
    }

    [Theory]
    [InlineData("\\n", "\n")]
    [InlineData("\\\\", "\\")]
    [InlineData("\\\"", "\"")]
    [InlineData("\\t", "\t")]
    void checkMakeString_whenValidEscapeChar_shouldReturnEscapedString(string escape, string expected)
    {
        LexerResult result = new Lexer.Lexer($"\"{escape}\"", "TEST").MakeTokens();

        Assert.True(result.TryGetValue(out List<BaseToken> tokens));
        Assert.Equal(2, tokens.Count); // token + EOF Token
        Assert.Equal(TokenType.STRING, tokens.First().Type);

        Token<string> strToken = Assert.IsType<Token<string>>(tokens.First());
        Assert.Equal(expected, strToken.Value);
    }

    [Fact]
    void checkMakeString_whenInvalidEscapeChar_shouldThrow()
    {
        LexerResult result = new Lexer.Lexer($"\"\\q\"", "TEST").MakeTokens();

        Assert.False(result.TryGetValue(out List<BaseToken> _));
        IllegalEscapeCharError error = Assert.IsType<IllegalEscapeCharError>(result.GetError());
        Assert.Contains("q", error.ToString());
    }

    [Fact]
    void checkMakeNotEquals_whenNormal_shouldReturnNETok()
    {
        LexerResult result = new Lexer.Lexer("!=", "TEST").MakeTokens();

        Assert.True(result.TryGetValue(out List<BaseToken> tokens));
        Assert.Equal(2, tokens.Count); // token + EOF Token
        Assert.Equal(TokenType.NE, tokens.First().Type);
    }

    [Fact]
    void checkMakeNotEquals_whenInvalid_shouldReturnError()
    {
        LexerResult result = new Lexer.Lexer($"!", "TEST").MakeTokens();

        Assert.False(result.TryGetValue(out List<BaseToken> _));
        ExpectedCharError error = Assert.IsType<ExpectedCharError>(result.GetError());
        Assert.Contains("=", error.ToString());
    }

    [Theory]
    [InlineData("    ")]
    [InlineData("\t\t\t")]
    void checkSkipWhiteSpace_shouldReturnEOFOnly(string input)
    {
        LexerResult result = new Lexer.Lexer($"{input}", "TEST").MakeTokens();

        Assert.True(result.TryGetValue(out List<BaseToken> tokens));
        Assert.Single(tokens);
        Assert.Equal(TokenType.EOF, tokens.First().Type);
    }

    [Theory]
    [InlineData("# some comment", 1)] // EOF
    [InlineData("# end ; 1", 3)] // NL + NUM + EOF
    [InlineData("# end \n 1", 3)] // NL + NUM + EOF
    [InlineData("# end \r 1", 3)] // NL + NUM + EOF
    [InlineData("# end # 1", 2)] // NUM + EOF
    void checkSkipComment(string input, int numExpectedTokens)
    {
        LexerResult result = new Lexer.Lexer($"{input}", "TEST").MakeTokens();

        Assert.True(result.TryGetValue(out List<BaseToken> tokens));
        Assert.Equal(numExpectedTokens, tokens.Count);
    }

    [Theory]
    [InlineData(": abc", 1)] // EOF
    [InlineData("# end ; 1", 3)] // NL + NUM + EOF
    [InlineData("# end \n 1", 3)] // NL + NUM + EOF
    [InlineData("# end \r 1", 3)] // NL + NUM + EOF
    [InlineData("# end # 1", 2)] // NUM + EOF
    void checkTypeAnnotation(string input, int numExpectedTokens)
    {
        LexerResult result = new Lexer.Lexer($"{input}", "TEST").MakeTokens();

        Assert.True(result.TryGetValue(out List<BaseToken> tokens));
        Assert.Equal(numExpectedTokens, tokens.Count);
    }

    [Fact]
    void shouldFailOnUnknownSymbol()
    {
        LexerResult result = new Lexer.Lexer($"😭", "TEST").MakeTokens();

        Assert.False(result.TryGetValue(out List<BaseToken> _));
        Assert.IsType<IllegalCharError>(result.GetError());
    }

    [Theory]
    [InlineData("123456789", 123456789)]
    [InlineData("123.456", 123.456)]
    [InlineData("123_456_789", 123_456_789)]
    void checkMakeNum_shouldReturnNum(string input, double expected)
    {
        LexerResult result = new Lexer.Lexer(input, "TEST").MakeTokens();

        Assert.True(result.TryGetValue(out List<BaseToken> tokens));
        Assert.Equal(2, tokens.Count); // token + EOF Token
        Assert.Equal(TokenType.NUMBER, tokens.First().Type);

        Token<double> strToken = Assert.IsType<Token<double>>(tokens.First());
        Assert.Equal(expected, strToken.Value);
    }

    [Fact]
    void checkMakeNum_shouldFailTwoDots()
    {
        LexerResult result = new Lexer.Lexer($"1.2.3", "TEST").MakeTokens();

        Assert.False(result.TryGetValue(out List<BaseToken> _));
        Assert.IsType<IllegalNumberFormat>(result.GetError());
    }
}
