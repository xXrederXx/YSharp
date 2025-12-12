using YSharp.Types.Lexer;

namespace YSharp.Types.AST;

public sealed class DotCallNode : BaseNode
{
    public readonly BaseNode[] ArgNodes;
    public readonly Token<string> FuncNameTok;
    public readonly BaseNode Parent;

    public DotCallNode(Token<string> funcNameTok, List<BaseNode> argNodes, BaseNode parent)
        : base(funcNameTok.StartPos, argNodes.Count > 0 ? argNodes[^1].EndPos : funcNameTok.EndPos)
    {
        this.FuncNameTok = funcNameTok;
        this.ArgNodes = argNodes.ToArray();
        this.Parent = parent;
    }

    public override string ToString() =>
        $"Calling {FuncNameTok} on {Parent} with args: {string.Join(", ", ArgNodes.Select(x => x.ToString()))}";
}