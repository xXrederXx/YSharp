using YSharp.Tools.Debug;

namespace YSharp.Parser.Nodes;


public sealed class WhileNode : BaseNode
{
    public readonly BaseNode BodyNode;
    public readonly BaseNode ConditionNode;
    public readonly bool RetNull;
    public override NodeDebugInfo DebugInfo =>
        new(
            "while",
            NodeDebugShape.Ellipse,
            [(ConditionNode.DebugInfo, "condition"), (BodyNode.DebugInfo, "body")]
        );

    public WhileNode(BaseNode conditionNode, BaseNode bodyNode, bool retNull)
        : base(conditionNode.StartPos, bodyNode.EndPos)
    {
        this.ConditionNode = conditionNode;
        this.BodyNode = bodyNode;
        this.RetNull = retNull;
    }

    public override string ToString() => $"while {ConditionNode} do {BodyNode}";
}
