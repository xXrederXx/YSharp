using System.Text;
using YSharp.Types.AST;

namespace YSharp.Utils;

public static class AstDotExporter
{
    private static int _idCounter = 0;
    private static readonly Dictionary<BaseNode, int> _nodeIds = new();

    public static void ExportToDot(BaseNode root, string outputPath)
    {
        _idCounter = 0;
        _nodeIds.Clear();
        var sb = new StringBuilder();

        sb.AppendLine("digraph AST {");
        sb.AppendLine("rankdir=LR;");

        Traverse(root, sb);

        sb.AppendLine("}");
        File.WriteAllText(outputPath, sb.ToString());
    }

    private static int Traverse(BaseNode node, StringBuilder sb)
    {
        if (node == null)
            return -1;

        if (_nodeIds.ContainsKey(node))
            return _nodeIds[node];

        int id = _idCounter++;
        _nodeIds[node] = id;
        string label = node.GetType().Name;

        // Add token info for relevant node types
        label += node switch
        {
            NumberNode n => $"\\n{n.tok}",
            StringNode s => $"\\n{s.tok}",
            VarAccessNode v => $"\\n{v.varNameTok}",
            VarAssignNode va => $"\\n{va.varNameTok}",
            BinOpNode bin => $"\\n{bin.opTok}",
            UnaryOpNode un => $"\\n{un.opTok}",
            DotVarAccessNode dv => $"\\n{dv.varNameTok}",
            DotCallNode dc => $"\\n{dc.funcNameTok}",
            ForNode fn => $"\\n{fn.varNameTok}",
            FuncDefNode fd => fd.varNameTok != null ? $"\\n{fd.varNameTok}" : "",
            TryCatchNode tc => $"\\n{tc.ChatchVarName}",
            ImportNode im => $"\\n{im.PathTok}",
            _ => string.Empty,
        };

        sb.AppendLine($"  node{id} [label=\"{label}\"];");

        foreach (var (child, edgeLabel) in GetChildrenWithLabels(node))
        {
            int childId = Traverse(child, sb);
            if (childId != -1)
                sb.AppendLine($"  node{id} -> node{childId} [label=\"{edgeLabel}\"];");
        }
        return id;
    }

    private static IEnumerable<(BaseNode child, string label)> GetChildrenWithLabels(BaseNode node)
    {
        switch (node)
        {
            case ListNode ln:
                return ln.elementNodes.Select((n, i) => (n, $"elem[{i}]"));
            case BinOpNode bin:
                return [(bin.leftNode, "left"), (bin.rightNode, "right")];
            case UnaryOpNode un:
                return [(un.node, "operand")];
            case VarAssignNode va:
                return [(va.valueNode, "value")];
            case DotVarAccessNode dv:
                return [(dv.parent, "parent")];
            case DotCallNode dc:
                return dc.argNodes.Select((n, i) => (n, $"arg[{i}]")).Append((dc.parent, "parent"));
            case SubIfNode sif:
                return [(sif.condition, "condition"), (sif.expression, "expression")];
            case IfNode ifn:
                var cases = ifn.cases.SelectMany(
                    (c, i) =>
                        new[]
                        {
                            (c.condition, $"case[{i}]-cond"),
                            (c.expression, $"case[{i}]-expr"),
                        }
                );
                return cases.Append((ifn.elseNode, "else"));
            case ForNode fn:
                return new[]
                {
                    (fn.startValueNode, "start"),
                    (fn.endValueNode, "end"),
                    (fn.stepValueNode, "step"),
                    (fn.bodyNode, "body"),
                }.Where(t => t.Item1 != null)!;
            case WhileNode wn:
                return [(wn.conditionNode, "condition"), (wn.bodyNode, "body")];
            case FuncDefNode fd:
                return [(fd.bodyNode, "body")];
            case CallNode cn:
                return cn
                    .argNodes.Select((n, i) => (n, $"arg[{i}]"))
                    .Append((cn.nodeToCall, "callee"));
            case ReturnNode rn:
                return rn.nodeToReturn != null
                    ? [(rn.nodeToReturn, "return")]
                    : Enumerable.Empty<(BaseNode, string)>();
            case TryCatchNode tc:
                return [(tc.TryNode, "try"), (tc.CatchNode, "catch")];
            case ImportNode im:
                return Enumerable.Empty<(BaseNode, string)>();
            default:
                return Enumerable.Empty<(BaseNode, string)>();
        }
    }
}
