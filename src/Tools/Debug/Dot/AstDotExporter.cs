using System.Text;
using YSharp.Parser.Nodes;

namespace YSharp.Tools.Debug.Dot;

public static class AstDotExporter{
    private static readonly Dictionary<NodeDebugInfo, int> _nodeIds = new();
    private static int _idCounter;

    public static void ExportToDot(BaseNode root, string outputPath)
    {
        _idCounter = 0;
        _nodeIds.Clear();
        StringBuilder sb = new();

        sb.AppendLine("digraph AST {");
        sb.AppendLine("rankdir=LR;");

        Traverse(root.DebugInfo, sb);

        sb.AppendLine("}");
        File.WriteAllText(outputPath, sb.ToString());
    }


    private static int Traverse(NodeDebugInfo node, StringBuilder sb)
    {
        if (_nodeIds.TryGetValue(node, out int value))
            return value;

        int id = _idCounter++;
        _nodeIds[node] = id;

        // Add token info for relevant node types
        string label = node.Label;
        string shape = node.Shape switch
        {
            NodeDebugShape.Diamond => "diamond",
            NodeDebugShape.Rectangle => "rect",
            NodeDebugShape.Hexagon => "hexagon",
            NodeDebugShape.Ellipse => "ellipse",
            NodeDebugShape.SpecialSquare => "Msquare",
            _ => throw new NotImplementedException(),
        };

        sb.AppendLine($"  node{id} [label=\"{label}\" shape=\"{shape}\"];");

        foreach ((NodeDebugInfo child, string edgeLabel) in node.Children)
        {
            int childId = Traverse(child, sb);
            if (childId != -1)
                sb.AppendLine($"  node{id} -> node{childId} [label=\"{edgeLabel}\"];");
        }

        return id;
    }
}