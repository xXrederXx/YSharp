using System.Text;
using FastEnumUtility;
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

        // Add token info for relevant node types
        string label = node switch
        {
            NumberNode n => $"{n.tok.Value}",
            StringNode n => $"\\\"{n.tok.Value}\\\"",
            VarAccessNode n => $"Accsess {n.varNameTok.Value}",
            VarAssignNode n => $"Assign {n.varNameTok.Value}",
            BinOpNode n => $"BinOp: {n.opTok.Type.FastToString()}",
            UnaryOpNode n => $"UnOp: {n.opTok.Type.FastToString()}",
            DotVarAccessNode n => $"DotAccsess: {n.varNameTok.Value}",
            DotCallNode n => $"DotCall: {n.funcNameTok.Value}",
            ForNode n => $"For: {n.varNameTok}",
            FuncDefNode n => n.varNameTok != null
                ? $"DEF: {n.varNameTok.Value}()"
                : "<anonymous-func>",
            TryCatchNode n => $"TryCatch: {n.ChatchVarName}",
            ImportNode n => $"Import: {n.PathTok}",
            SuffixAssignNode n => $"SufAssign: {n.varName} (Add? {n.isAdd})",
            CallNode n => $"CALL",
            IfNode n => $"IF",
            ListNode n => $"LIST",
            _ => node.GetType().Name,
        };
        string shape = node switch
        {
            IfNode => "diamond",
            NumberNode => "rect",
            StringNode => "rect",
            ListNode => "Msquare",
            BinOpNode => "hexagon",
            _ => "ellipse",
        };

        sb.AppendLine($"  node{id} [label=\"{label}\" shape=\"{shape}\"];");

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
                if (ifn.elseNode is not NodeNull)
                    cases.Append((ifn.elseNode, "else"));
                return cases;
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
