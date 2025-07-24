using System.Collections.Immutable;
using YSharp.Types.Common;
using YSharp.Types.Lexer;

namespace YSharp.AST;

// Interface definition
public interface INode
{
    Position StartPos { get; set; }
    Position EndPos { get; set; }
}

public class NodeNull : INode
{
    public Position StartPos
    {
        get => Position.Null;
        set => _ = Position.Null;
    }
    public Position EndPos
    {
        get => Position.Null;
        set => _ = Position.Null;
    }

    public static readonly NodeNull Instance = new();

    private NodeNull() { }

    public override string ToString() => "Null Node";
}

// NumberNode implements INode
public class NumberNode(Token<double> tok) : INode
{
    public readonly Token<double> tok = tok;

    // Implement properties, not fields
    public Position StartPos { get; set; } = tok.StartPos;
    public Position EndPos { get; set; } = tok.EndPos;

    public override string ToString() => tok.ToString();
}

// This node represents a string token
public class StringNode(Token<string> tok) : INode
{
    public readonly Token<string> tok = tok;

    public Position StartPos { get; set; } = tok.StartPos;
    public Position EndPos { get; set; } = tok.EndPos;

    public override string ToString() => tok.ToString();
}

// This node represents a list of elements
public class ListNode(List<INode> elementNodes, Position posStart, Position posEnd) : INode
{
    public readonly ImmutableList<INode> elementNodes = elementNodes.ToImmutableList();

    public Position StartPos { get; set; } = posStart;
    public Position EndPos { get; set; } = posEnd;

    public override string ToString()
    {
        return "[" + string.Join(',', elementNodes.Select(x => x.ToString())) + "]";
    }
}

// This node represents a binary operation
public class BinOpNode(INode leftNode, IToken opTok, INode rightNode) : INode
{
    public readonly INode leftNode = leftNode;
    public readonly IToken opTok = opTok;
    public readonly INode rightNode = rightNode;

    public Position StartPos { get; set; } = leftNode.StartPos;
    public Position EndPos { get; set; } = rightNode.EndPos;

    public override string ToString() => $"({leftNode}, {opTok}, {rightNode})";
}

// This node represents a unary operation
public class UnaryOpNode(IToken opTok, INode node) : INode
{
    public readonly IToken opTok = opTok;
    public readonly INode node = node;

    public Position StartPos { get; set; } = opTok.StartPos;
    public Position EndPos { get; set; } = node.EndPos;

    public override string ToString() => $"({opTok}, {node})";
}

// This node represents a variable access
public class VarAccessNode(Token<string> varNameTok) : INode
{
    public readonly Token<string> varNameTok = varNameTok;
    public bool fromCall = false;

    public Position StartPos { get; set; } = varNameTok.StartPos;
    public Position EndPos { get; set; } = varNameTok.EndPos;

    public override string ToString() => $"Access {varNameTok.Value}";
}

// This node represents a variable assignment
public class VarAssignNode(Token<string> varNameTok, INode valueNode) : INode
{
    public readonly Token<string> varNameTok = varNameTok;
    public readonly INode valueNode = valueNode;

    public Position StartPos { get; set; } = varNameTok.StartPos;
    public Position EndPos { get; set; } = valueNode.EndPos;

    public override string ToString()
    {
        return varNameTok.Value + " = " + valueNode;
    }
}

// This node represents a dot (.) variable access
public class DotVarAccessNode(Token<string> varNameTok, INode parent) : INode
{
    public readonly Token<string> varNameTok = varNameTok;
    public readonly INode parent = parent;

    public Position StartPos { get; set; } = varNameTok.StartPos;
    public Position EndPos { get; set; } = varNameTok.EndPos;

    public override string ToString() => $"Access {varNameTok.Value} from {parent}";
}

// This node represents a function call using dot notation
public class DotCallNode(Token<string> funcNameTok, List<INode> argNodes, INode parent) : INode
{
    public readonly Token<string> funcNameTok = funcNameTok;
    public readonly ImmutableList<INode> argNodes = argNodes.ToImmutableList();
    public readonly INode parent = parent;

    public Position StartPos { get; set; } = funcNameTok.StartPos;
    public Position EndPos { get; set; } =
        argNodes.Count > 0 ? argNodes[^1].EndPos : funcNameTok.EndPos;

    public override string ToString() =>
        $"Calling {funcNameTok} on {parent} with args: {string.Join(", ", argNodes.Select(x => x.ToString()))}";
}

public class SubIfNode(INode Condition, INode Expression) : INode
{
    public readonly INode condition = Condition;
    public readonly INode expression = Expression;

    public Position StartPos { get; set; } = Condition.StartPos;
    public Position EndPos { get; set; } = Expression.EndPos;

    public override string ToString() => $"If {condition} then {expression}";
}

// This node represents an if statement
public class IfNode(List<SubIfNode> Cases, INode ElseNode) : INode
{
    public readonly ImmutableList<SubIfNode> cases = Cases.ToImmutableList();
    public readonly INode elseNode = ElseNode;

    public Position StartPos { get; set; } = Cases[0].condition.StartPos;
    public Position EndPos { get; set; } = ElseNode != null ? ElseNode.EndPos : Cases[^1].EndPos;

    public override string ToString() =>
        $"All If Casses: {string.Join(", ", cases.Select(x => x.ToString()))} Else: {elseNode}";
}

// This node represents a for loop
public class ForNode(
    Token<string> varNameTok,
    INode startValueNode,
    INode endValueNode,
    INode? stepValueNode,
    INode bodyNode,
    bool retNull
) : INode
{
    public readonly Token<string> varNameTok = varNameTok;
    public readonly INode startValueNode = startValueNode;
    public readonly INode endValueNode = endValueNode;
    public readonly INode? stepValueNode = stepValueNode;
    public readonly INode bodyNode = bodyNode;
    public readonly bool retNull = retNull;

    public Position StartPos { get; set; } = varNameTok.StartPos;
    public Position EndPos { get; set; } = bodyNode.EndPos;

    public override string ToString() =>
        $"for {varNameTok} start {startValueNode} end {endValueNode} with step {stepValueNode} do {bodyNode} and return Null {retNull}";
}

// This node represents a while loop
public class WhileNode(INode conditionNode, INode bodyNode, bool retNull) : INode
{
    public readonly INode conditionNode = conditionNode;
    public readonly INode bodyNode = bodyNode;
    public readonly bool retNull = retNull;

    public Position StartPos { get; set; } = conditionNode.StartPos;
    public Position EndPos { get; set; } = bodyNode.EndPos;

    public override string ToString() => $"while {conditionNode} do {bodyNode}";
}

// This node represents a function definition
public class FuncDefNode : INode
{
    public readonly Token<string> varNameTok;
    public readonly ImmutableList<IToken> argNameToks;
    public readonly INode bodyNode;
    public readonly bool retNull;

    public FuncDefNode(
        Token<string> varNameTok,
        List<IToken> argNameToks,
        INode bodyNode,
        bool autoReturn
    )
    {
        this.varNameTok = varNameTok;
        this.argNameToks = (argNameToks ?? []).ToImmutableList();
        this.bodyNode = bodyNode;
        retNull = autoReturn;

        if (varNameTok is not null)
        {
            StartPos = varNameTok.StartPos;
        }
        else if (argNameToks?.Count > 0)
        {
            StartPos = argNameToks[0].StartPos;
        }
        else
        {
            StartPos = bodyNode.StartPos;
        }
        EndPos = bodyNode.EndPos;
    }

    public Position StartPos { get; set; }
    public Position EndPos { get; set; }

    public override string ToString() =>
        $"Define Function {varNameTok} with args {string.Join(", ", argNameToks.Select(x => x.ToString()))} and do {bodyNode} (return null {retNull})";
}

// This node represents a function call
public class CallNode(INode nodeToCall, List<INode> argNodes) : INode
{
    public readonly INode nodeToCall = nodeToCall;
    public readonly ImmutableList<INode> argNodes = argNodes.ToImmutableList();

    public Position StartPos { get; set; } = nodeToCall.StartPos;
    public Position EndPos { get; set; } =
        argNodes.Count > 0 ? argNodes[^1].EndPos : nodeToCall.EndPos;

    public override string ToString() =>
        $"{nodeToCall} -> " + string.Join(',', argNodes.Select(x => x.ToString()));
}

// This node represents a return statement
public class ReturnNode(INode? nodeToReturn, Position startPos, Position endPos) : INode
{
    public readonly INode? nodeToReturn = nodeToReturn;

    public Position StartPos { get; set; } = startPos;
    public Position EndPos { get; set; } = endPos;

    public override string ToString() => $"return {nodeToReturn}";
}

// This node represents a continue statement
public class ContinueNode(Position startPos, Position endPos) : INode
{
    public Position StartPos { get; set; } = startPos;
    public Position EndPos { get; set; } = endPos;

    public override string ToString() => $"Continue Node";
}

// This node represents a break statement
public class BreakNode(Position startPos, Position endPos) : INode
{
    public Position StartPos { get; set; } = startPos;
    public Position EndPos { get; set; } = endPos;

    public override string ToString() => $"Break Node";
}

public class TryCatchNode(INode tryNode, INode catchNode, Token<string> catchVarName) : INode
{
    public readonly INode TryNode = tryNode;
    public readonly INode CatchNode = catchNode;
    public readonly Token<string> ChatchVarName = catchVarName;

    public Position StartPos { get; set; } = tryNode.StartPos;
    public Position EndPos { get; set; } =
        catchNode is NodeNull _ ? tryNode.EndPos : catchNode.EndPos;

    public override string ToString() => $"Try {TryNode} Catch {CatchNode} as {ChatchVarName}";
}

public class ImportNode(Token<string> pathTok, Position startPos, Position endPos) : INode
{
    public Position StartPos { get; set; } = startPos;
    public Position EndPos { get; set; } = endPos;
    public Token<string> PathTok = pathTok;

    public override string ToString() => $"Import {PathTok}";
}
