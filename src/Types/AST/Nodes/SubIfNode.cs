namespace YSharp.Types.AST;

public sealed class SubIfNode : BaseNode
{
    public readonly BaseNode Condition;
    public readonly BaseNode Expression;

    public SubIfNode(BaseNode condition, BaseNode expression)
        : base(condition.StartPos, expression.EndPos)
    {
        this.Condition = condition;
        this.Expression = expression;
    }

    public override string ToString() => $"If {Condition} then {Expression}";
}