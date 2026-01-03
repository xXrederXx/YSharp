using YSharp.Common;
using YSharp.Lexer;
using YSharp.Tools.Debug;

namespace YSharp.Parser.Nodes;

public sealed class ForNode : BaseNode
{
    public readonly BaseNode BodyNode;
    public readonly BaseNode EndValueNode;
    public readonly bool RetNull;
    public readonly BaseNode StartValueNode;
    public readonly BaseNode StepValueNode;
    public readonly Token<string> VarNameTok;
    public override NodeDebugInfo DebugInfo =>
        new(
            $"for ({VarNameTok.Value})",
            NodeDebugShape.Ellipse,
            [
                (StartValueNode.DebugInfo, "start"),
                (StepValueNode.DebugInfo, "step"),
                (EndValueNode.DebugInfo, "end"),
                (BodyNode.DebugInfo, "body"),
                (StartValueNode.DebugInfo, "start"),
            ]
        );

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
        this.StepValueNode = stepValueNode ?? new NumberNode(new Token<double>(TokenType.NUMBER, 1, Position.Null, Position.Null));
        this.BodyNode = bodyNode;
        this.RetNull = retNull;
    }

    public override string ToString() =>
        $"for {VarNameTok} start {StartValueNode} end {EndValueNode} with step {StepValueNode} do {BodyNode} and return Null {RetNull}";
}
