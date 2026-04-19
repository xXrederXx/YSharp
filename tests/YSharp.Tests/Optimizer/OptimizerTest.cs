using Xunit;
using YSharp.Common;
using YSharp.Lexer;
using YSharp.Parser;
using YSharp.Parser.Nodes;
using YSharp.Runtime.Collections.List;
using YSharp.Runtime.Primitives.Number;

namespace YSharp.Tests;

public class OptimizerTest
{
    [Theory]
    [InlineData("2 + 5", 7)]
    [InlineData("2 * 5", 10)]
    [InlineData("2 - 5", -3)]
    [InlineData("6 / 2", 3)]
    [InlineData("6 / (1 + 1)", 3)]
    [InlineData("5^2", 25)]
    void checkNumberFolding(string expression, double expected)
    {
        Result<List<BaseToken>, Error> lexerResult = new Lexer.Lexer(
            expression,
            "<test>"
        ).MakeTokens();

        Assert.True(lexerResult.TryGetValue(out List<BaseToken> tokens));

        ParseResult parserResult = new Parser.Parser(tokens).Parse();

        Assert.False(parserResult.HasError);

        BaseNode optimized = Optimizer.Optimizer.Visit(parserResult.Node);

        ListNode expressionList = Assert.IsType<ListNode>(optimized);
        Assert.NotEmpty(expressionList.ElementNodes);
        NumberNode node = Assert.IsType<NumberNode>(expressionList.ElementNodes.First());
        Assert.Equal(expected, node.Tok.Value, TestingConstans.DOUBLE_PRECISION);
    }

    [Fact]
    void checkStringFolding()
    {
        Result<List<BaseToken>, Error> lexerResult = new Lexer.Lexer(
            "\"Hello\" + \" World\"",
            "<test>"
        ).MakeTokens();

        Assert.True(lexerResult.TryGetValue(out List<BaseToken> tokens));

        ParseResult parserResult = new Parser.Parser(tokens).Parse();

        Assert.False(parserResult.HasError);

        BaseNode optimized = Optimizer.Optimizer.Visit(parserResult.Node);

        ListNode expressionList = Assert.IsType<ListNode>(optimized);
        Assert.NotEmpty(expressionList.ElementNodes);
        StringNode node = Assert.IsType<StringNode>(expressionList.ElementNodes.First());
        Assert.Equal("Hello World", node.Tok.Value);
    }
}
