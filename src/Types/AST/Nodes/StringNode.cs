using YSharp.Types.Lexer;

namespace YSharp.Types.AST;

public sealed class StringNode : BaseNode
{
    public readonly Token<string> Tok;

    public StringNode(Token<string> tok)
        : base(tok.StartPos, tok.EndPos)
    {
        this.Tok = tok;
    }

    public override string ToString() => Tok.ToString();
}