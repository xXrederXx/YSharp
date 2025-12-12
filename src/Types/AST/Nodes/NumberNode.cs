using YSharp.Types.Lexer;

namespace YSharp.Types.AST;

public sealed class NumberNode : BaseNode
{
    public readonly Token<double> Tok;

    public NumberNode(Token<double> tok)
        : base(tok.StartPos, tok.EndPos)
    {
        this.Tok = tok;
    }

    public override string ToString() => Tok.ToString();
}