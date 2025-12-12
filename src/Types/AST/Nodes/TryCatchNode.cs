using YSharp.Types.Lexer;

namespace YSharp.Types.AST;

public sealed class TryCatchNode : BaseNode
{
    public readonly BaseNode CatchNode;
    public readonly Token<string> CatchVarName;
    public readonly BaseNode TryNode;

    public TryCatchNode(BaseNode tryNode, BaseNode catchNode, Token<string> catchVarName)
        : base(tryNode.StartPos, catchNode is NodeNull _ ? tryNode.EndPos : catchNode.EndPos)
    {
        TryNode = tryNode;
        CatchNode = catchNode;
        CatchVarName = catchVarName;
    }

    public override string ToString() => $"Try {TryNode} Catch {CatchNode} as {CatchVarName}";
}