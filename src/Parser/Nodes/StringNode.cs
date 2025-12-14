using YSharp.Lexer;
using YSharp.Tools.Debug;

namespace YSharp.Parser.Nodes;


public sealed class StringNode : BaseNode
{
    public readonly Token<string> Tok;

    public override NodeDebugInfo DebugInfo =>
        new($"\\\"{Tok.Value}\\\"", NodeDebugShape.Rectangle, []);

    public StringNode(Token<string> tok)
        : base(tok.StartPos, tok.EndPos)
    {
        this.Tok = tok;
    }

    public override string ToString() => Tok.ToString();
}
