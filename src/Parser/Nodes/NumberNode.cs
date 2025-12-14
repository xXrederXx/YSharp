using YSharp.Lexer;
using YSharp.Tools.Debug;

namespace YSharp.Parser.Nodes;


public sealed class NumberNode : BaseNode
{
    public readonly Token<double> Tok;
    public override NodeDebugInfo DebugInfo => new($"{Tok.Value}", NodeDebugShape.Rectangle, []);

    public NumberNode(Token<double> tok)
        : base(tok.StartPos, tok.EndPos)
    {
        this.Tok = tok;
    }

    public override string ToString() => Tok.ToString();
}