using YSharp.Tools.Debug;

namespace YSharp.Parser.Nodes;

public sealed class SubIfNode : BaseNode
{
    public readonly BaseNode Condition;
    public readonly BaseNode Expression;

    public override NodeDebugInfo DebugInfo =>
        new(
            "case",
            NodeDebugShape.Ellipse,
            [(Condition.DebugInfo, "condition"), (Expression.DebugInfo, "expression")]
        );

    public SubIfNode(BaseNode condition, BaseNode expression)
        : base(condition.StartPos, expression.EndPos)
    {
        this.Condition = condition;
        this.Expression = expression;
    }

    public override string ToString() => $"If {Condition} then {Expression}";
}
