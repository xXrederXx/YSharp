using System.Text;
using FastEnumUtility;
using YSharp.Types.AST;

namespace YSharp.Utils.Debug.Dot;

public static class AstDotExporter{
    private static readonly Dictionary<BaseNode, int> _nodeIds = new();
    private static int _idCounter;

    public static void ExportToDot(BaseNode root, string outputPath)
    {
        _idCounter = 0;
        _nodeIds.Clear();
        StringBuilder sb = new();

        sb.AppendLine("digraph AST {");
        sb.AppendLine("rankdir=LR;");

        Traverse(root, sb);

        sb.AppendLine("}");
        File.WriteAllText(outputPath, sb.ToString());
    }

    private static IEnumerable<(BaseNode child, string label)> GetChildrenWithLabels(BaseNode node)
    {
        switch (node)
        {
            case ListNode ln:
                return ln.ElementNodes.Select((n, i) => (n, $"elem[{i}]"));
            case BinOpNode bin:
                return [(bin.LeftNode, "left"), (bin.RightNode, "right")];
            case UnaryOpNode un:
                return [(un.Node, "operand")];
            case VarAssignNode va:
                return [(va.ValueNode, "value")];
            case DotVarAccessNode dv:
                return [(dv.Parent, "parent")];
            case DotCallNode dc:
                return dc.ArgNodes.Select((n, i) => (n, $"arg[{i}]")).Append((parent: dc.Parent, "parent"));
            case SubIfNode sif:
                return [(sif.Condition, "condition"), (sif.Expression, "expression")];
            case IfNode ifn:
                IEnumerable<(BaseNode, string)> cases = ifn.Cases.SelectMany((c, i) =>
                    new[]
                    {
                        (condition: c.Condition, $"case[{i}]-cond"),
                        (expression: c.Expression, $"case[{i}]-expr")
                    }
                );
                if (ifn.ElseNode is not NodeNull)
                    cases = cases.Append((elseNode: ifn.ElseNode, "else"));
                return cases;
            case ForNode fn:
                return new[]
                {
                    (startValueNode: fn.StartValueNode, "start"),
                    (endValueNode: fn.EndValueNode, "end"),
                    (stepValueNode: fn.StepValueNode, "step"),
                    (bodyNode: fn.BodyNode, "body")
                }.Where(t => t.Item1 != null)!;
            case WhileNode wn:
                return [(wn.ConditionNode, "condition"), (wn.BodyNode, "body")];
            case FuncDefNode fd:
                return [(fd.BodyNode, "body")];
            case CallNode cn:
                return cn
                    .ArgNodes.Select((n, i) => (n, $"arg[{i}]"))
                    .Prepend((nodeToCall: cn.NodeToCall, "callee"));
            case ReturnNode rn:
                return rn.NodeToReturn != null
                    ? [(rn.NodeToReturn, "return")]
                    : Enumerable.Empty<(BaseNode, string)>();
            case TryCatchNode tc:
                return [(tc.TryNode, "try"), (tc.CatchNode, "catch")];
            case ImportNode im:
                return Enumerable.Empty<(BaseNode, string)>();
            default:
                return Enumerable.Empty<(BaseNode, string)>();
        }
    }

    private static int Traverse(BaseNode node, StringBuilder sb)
    {
        if (_nodeIds.TryGetValue(node, out int value))
            return value;

        int id = _idCounter++;
        _nodeIds[node] = id;

        // Add token info for relevant node types
        string label = node switch
        {
            NumberNode n => $"{n.Tok.Value}",
            StringNode n => $"\\\"{n.Tok.Value}\\\"",
            VarAccessNode n => $"Accsess {n.VarNameTok.Value}",
            VarAssignNode n => $"Assign {n.VarNameTok.Value}",
            BinOpNode n => $"BinOp: {n.OpTok.Type.FastToString()}",
            UnaryOpNode n => $"UnOp: {n.OpTok.Type.FastToString()}",
            DotVarAccessNode n => $"DotAccsess: {n.VarNameTok.Value}",
            DotCallNode n => $"DotCall: {n.FuncNameTok.Value}",
            ForNode n => $"For: {n.VarNameTok}",
            FuncDefNode n => n.VarNameTok != null
                ? $"DEF: {n.VarNameTok.Value}()"
                : "<anonymous-func>",
            TryCatchNode n => $"TryCatch: {n.CatchVarName}",
            ImportNode n => $"Import: {n.PathTok}",
            SuffixAssignNode n => $"SufAssign: {n.VarName} (Add? {n.IsAdd})",
            CallNode n => "CALL",
            IfNode n => "IF",
            ListNode n => "LIST",
            _ => node.GetType().Name
        };
        string shape = node switch
        {
            IfNode => "diamond",
            NumberNode => "rect",
            StringNode => "rect",
            ListNode => "Msquare",
            BinOpNode => "hexagon",
            _ => "ellipse"
        };

        sb.AppendLine($"  node{id} [label=\"{label}\" shape=\"{shape}\"];");

        foreach ((BaseNode child, string edgeLabel) in GetChildrenWithLabels(node))
        {
            int childId = Traverse(child, sb);
            if (childId != -1)
                sb.AppendLine($"  node{id} -> node{childId} [label=\"{edgeLabel}\"];");
        }

        return id;
    }
}