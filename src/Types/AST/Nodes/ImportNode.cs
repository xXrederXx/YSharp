using YSharp.Types.Common;
using YSharp.Types.Lexer;

namespace YSharp.Types.AST;

public sealed class ImportNode : BaseNode
{
    public readonly Token<string> PathTok;

    public ImportNode(Token<string> pathTok, in Position startPos, in Position endPos)
        : base(startPos, endPos)
    {
        PathTok = pathTok;
    }

    public override string ToString() => $"Import {PathTok}";
}