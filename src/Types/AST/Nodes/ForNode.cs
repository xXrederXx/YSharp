using YSharp.Types.Lexer;

namespace YSharp.Types.AST;

public sealed class ForNode : BaseNode
{
    public readonly BaseNode BodyNode;
    public readonly BaseNode EndValueNode;
    public readonly bool RetNull;
    public readonly BaseNode StartValueNode;
    public readonly BaseNode? StepValueNode;
    public readonly Token<string> VarNameTok;

    public ForNode(
        Token<string> varNameTok,
        BaseNode startValueNode,
        BaseNode endValueNode,
        BaseNode? stepValueNode,
        BaseNode bodyNode,
        bool retNull
    )
        : base(varNameTok.StartPos, bodyNode.EndPos)
    {
        this.VarNameTok = varNameTok;
        this.StartValueNode = startValueNode;
        this.EndValueNode = endValueNode;
        this.StepValueNode = stepValueNode;
        this.BodyNode = bodyNode;
        this.RetNull = retNull;
    }

    public override string ToString() =>
        $"for {VarNameTok} start {StartValueNode} end {EndValueNode} with step {StepValueNode} do {BodyNode} and return Null {RetNull}";
}