namespace YSharp.Types.AST;

public sealed class IfNode : BaseNode
{
    public readonly SubIfNode[] Cases;
    public readonly BaseNode ElseNode;

    public IfNode(List<SubIfNode> cases, BaseNode elseNode)
        : base(
            cases[0].Condition.StartPos,
            elseNode != NodeNull.Instance ? elseNode.EndPos : cases[^1].EndPos
        )
    {
        this.Cases = cases.ToArray();
        this.ElseNode = elseNode;
    }

    public override string ToString() =>
        $"All If Cases: {string.Join(", ", Cases.Select(x => x.ToString()))} Else: {ElseNode}";
}