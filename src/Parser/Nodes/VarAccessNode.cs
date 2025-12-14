using YSharp.Lexer;
using YSharp.Tools.Debug;

namespace YSharp.Parser.Nodes;


public sealed class VarAccessNode : BaseNode
{
    public readonly Token<string> VarNameTok;
    public bool FromCall = false;

    public override NodeDebugInfo DebugInfo =>
        new($"VarAcc {VarNameTok.Value}", NodeDebugShape.Ellipse, []);

    public VarAccessNode(Token<string> varNameTok)
        : base(varNameTok.StartPos, varNameTok.EndPos)
    {
        this.VarNameTok = varNameTok;
    }

    public override string ToString() => $"Access {VarNameTok.Value}";
}
