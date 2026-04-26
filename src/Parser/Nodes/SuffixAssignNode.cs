using YSharp.Lexer;
using YSharp.Tools.Debug;

namespace YSharp.Parser.Nodes;


public sealed class SuffixAssignNode : BaseNode
{
    public readonly TokenType Type;
    public readonly string VarName;

    public override NodeDebugInfo DebugInfo =>
        new($"SufAssign: {VarName} (Add? {Type})", NodeDebugShape.Ellipse, []);

    public SuffixAssignNode(Token<string> varName, TokenType type)
        : base(varName.StartPos, varName.EndPos)
    {
        this.VarName = varName.Value;
        this.Type = type;
    }
}
