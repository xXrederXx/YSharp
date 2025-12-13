using YSharp.Types.Lexer;
using YSharp.Utils.Dot;

namespace YSharp.Types.AST;

public sealed class SuffixAssignNode : BaseNode
{
    public readonly bool IsAdd;
    public readonly string VarName;

    public override NodeDebugInfo DebugInfo =>
        new($"SufAssign: {VarName} (Add? {IsAdd})", NodeDebugShape.Ellipse, []);

    public SuffixAssignNode(Token<string> varName, bool isAdd)
        : base(varName.StartPos, varName.EndPos)
    {
        this.VarName = varName.Value;
        this.IsAdd = isAdd;
    }
}
