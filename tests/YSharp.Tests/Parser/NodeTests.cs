using Xunit;
using YSharp.Common;
using YSharp.Lexer;
using YSharp.Parser.Nodes;

namespace YSharp.Tests;

public class NodeTests
{
    [Theory]
    [MemberData(nameof(GetNodes))]
    public void checkNode_shouldNotThrow(Func<BaseNode> factory)
    {
        // check ctor
        Exception exception = Record.Exception(factory);
        Assert.Null(exception);

        // check debug info generation
        BaseNode node = factory.Invoke();
        exception = Record.Exception(() => node.DebugInfo);
        Assert.Null(exception);

        exception = Record.Exception(node.ToString);
        Assert.Null(exception);
    }

    public static TheoryData<Func<BaseNode>> GetNodes()
    {
        Token<string> stringTok = new Token<string>(
            TokenType.STRING,
            string.Empty,
            Position.Null,
            Position.Null
        );
        return new TheoryData<Func<BaseNode>>(
            () => new BinOpNode(NodeNull.Instance, NullToken.Instance, NodeNull.Instance),
            () => new BreakNode(Position.Null, Position.Null),
            () => new CallNode(NodeNull.Instance, [NodeNull.Instance]),
            () => new ContinueNode(Position.Null, Position.Null),
            () => new DotCallNode(stringTok, [NodeNull.Instance], NodeNull.Instance),
            () =>
                new ForNode(
                    stringTok,
                    NodeNull.Instance,
                    NodeNull.Instance,
                    NodeNull.Instance,
                    NodeNull.Instance,
                    false
                ),
            () => new FuncDefNode(stringTok, [NullToken.Instance], NodeNull.Instance, true),
            () =>
                new IfNode(
                    [new SubIfNode(NodeNull.Instance, NodeNull.Instance)],
                    NodeNull.Instance
                ),
            () => new ImportNode(stringTok, Position.Null, Position.Null),
            () => new ListNode([NodeNull.Instance], Position.Null, Position.Null),
            () =>
                new NumberNode(
                    new Token<double>(TokenType.NUMBER, 1.0, Position.Null, Position.Null)
                ),
            () => new ReturnNode(NodeNull.Instance, Position.Null, Position.Null),
            () => new StringNode(stringTok),
            () => new SubIfNode(NodeNull.Instance, NodeNull.Instance),
            () => new SuffixAssignNode(stringTok, TokenType.MM),
            () => new TryCatchNode(NodeNull.Instance, NodeNull.Instance, stringTok),
            () => new UnaryOpNode(NullToken.Instance, NodeNull.Instance),
            () => new VarAccessNode(stringTok),
            () => new VarAssignNode(stringTok, NodeNull.Instance),
            () => new DotVarAccessNode(stringTok, NodeNull.Instance),
            () => new WhileNode(NodeNull.Instance, NodeNull.Instance, true)
        );
    }
}
