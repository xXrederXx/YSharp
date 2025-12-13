using YSharp.Types.Common;
using YSharp.Types.Lexer;
using YSharp.Utils.Dot;

namespace YSharp.Types.AST;

public sealed class ImportNode : BaseNode
{
    public readonly Token<string> PathTok;

    public override NodeDebugInfo DebugInfo =>
        new($"import ({PathTok.Value})", NodeDebugShape.Ellipse, []);

    public ImportNode(Token<string> pathTok, in Position startPos, in Position endPos)
        : base(startPos, endPos)
    {
        PathTok = pathTok;
    }

    public override string ToString() => $"Import {PathTok}";
}
