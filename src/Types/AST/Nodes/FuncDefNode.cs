using YSharp.Types.Lexer;

namespace YSharp.Types.AST;

public sealed class FuncDefNode : BaseNode
{
    public readonly IToken[] ArgNameTokens;
    public readonly BaseNode BodyNode;
    public readonly bool RetNull;
    public readonly Token<string> VarNameTok;

    public FuncDefNode(
        Token<string> varNameTok,
        List<IToken> argNameTokens,
        BaseNode bodyNode,
        bool autoReturn
    )
        : base(varNameTok.StartPos, bodyNode.EndPos)
    {
        this.VarNameTok = varNameTok;
        this.ArgNameTokens = argNameTokens.ToArray();
        this.BodyNode = bodyNode;
        RetNull = autoReturn;
    }

    public override string ToString() =>
        $"Define Function {VarNameTok} with args {string.Join(", ", ArgNameTokens.Select(x => x.ToString()))} and do {BodyNode} (return null {RetNull})";
}