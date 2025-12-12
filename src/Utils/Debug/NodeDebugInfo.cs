namespace YSharp.Utils.Dot;

public record NodeDebugInfo(
    string Label,
    NodeDebugShape Shape,
    List<(NodeDebugInfo child, string edgeName)> Children       
    );