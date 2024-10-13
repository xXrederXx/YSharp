
namespace YSharp_2._0;
public class Node
{
    public Position StartPos { get; protected set; }
    public Position EndPos { get; protected set; }
}

// This node represents a number token
public class NumberNode : Node
{
    public readonly Token tok;

    public NumberNode(Token tok)
    {
        this.tok = tok;
        StartPos = tok.StartPos;
        EndPos = tok.EndPos;
    }

    public override string ToString() => tok.ToString();
}

// This node represents a string token
public class StringNode : Node
{
    public readonly Token tok;

    public StringNode(Token tok)
    {
        this.tok = tok;
        StartPos = tok.StartPos;
        EndPos = tok.EndPos;
    }

    public override string ToString() => tok.ToString();
}

// This node represents a list of elements
public class ListNode : Node
{
    public readonly List<Node> elementNodes;

    public ListNode(List<Node> elementNodes, Position posStart, Position posEnd)
    {
        this.elementNodes = elementNodes;
        StartPos = posStart;
        EndPos = posEnd;
    }
    public override string ToString()
    {
        return "[" + string.Join(',', elementNodes) + "]";
    }
}

// This node represents a binary operation
public class BinOpNode : Node
{
    public readonly Node leftNode;
    public readonly Token opTok;
    public readonly Node rightNode;

    public BinOpNode(Node leftNode, Token opTok, Node rightNode)
    {
        this.leftNode = leftNode;
        this.opTok = opTok;
        this.rightNode = rightNode;

        StartPos = leftNode.StartPos;
        EndPos = rightNode.EndPos;
    }

    public override string ToString() => $"({leftNode}, {opTok}, {rightNode})";
}

// This node represents a unary operation
public class UnaryOpNode : Node
{
    public readonly Token opTok;
    public readonly Node node;

    public UnaryOpNode(Token opTok, Node node)
    {
        this.opTok = opTok;
        this.node = node;

        StartPos = opTok.StartPos;
        EndPos = node.EndPos;
    }

    public override string ToString() => $"({opTok}, {node})";
}

// This node represents a variable access
public class VarAccessNode : Node
{
    public readonly Token varNameTok;

    public VarAccessNode(Token varNameTok)
    {
        this.varNameTok = varNameTok;
        StartPos = varNameTok.StartPos;
        EndPos = varNameTok.EndPos;
    }
    public override string ToString()
    {
        return varNameTok.Value?.ToString() ?? "null";
    }
}

// This node represents a variable assignment
public class VarAssignNode : Node
{
    public readonly Token varNameTok;
    public readonly Node valueNode;

    public VarAssignNode(Token varNameTok, Node valueNode)
    {
        this.varNameTok = varNameTok;
        this.valueNode = valueNode;

        StartPos = varNameTok.StartPos;
        EndPos = valueNode.EndPos;
    }

    public override string ToString()
    {
        return varNameTok.Value + " = " + valueNode;
    }
}

// This node represents a dot (.) variable access
public class DotVarAccessNode : Node
{
    public readonly Token varNameTok;
    public readonly Node parent;

    public DotVarAccessNode(Token varNameTok, Node parent)
    {
        this.varNameTok = varNameTok;
        this.parent = parent;

        StartPos = varNameTok.StartPos;
        EndPos = varNameTok.EndPos;
    }
}

// This node represents a function call using dot notation
public class DotCallNode : Node
{
    public readonly Token funcNameTok;
    public readonly List<Node> argNodes;
    public readonly Node parent;

    public DotCallNode(Token funcNameTok, List<Node> argNodes, Node parent)
    {
        this.funcNameTok = funcNameTok;
        this.argNodes = argNodes;
        this.parent = parent;

        StartPos = funcNameTok.StartPos;
        EndPos = argNodes.Count > 0 ? argNodes[^1].EndPos : funcNameTok.EndPos;
    }
}

// This node represents an if statement
public class IfNode : Node
{
    public readonly List<IfExpresionCases> cases;
    public readonly ElseCaseData elseCase;

    public IfNode(List<IfExpresionCases> cases, ElseCaseData elseCase)
    {
        this.cases = cases;
        this.elseCase = elseCase;

        StartPos = cases[0].condition.StartPos;
        EndPos = elseCase.Node != null ? elseCase.Node.EndPos : cases[^1].condition.EndPos;
    }
}

// This node represents a for loop
public class ForNode : Node
{
    public readonly Token varNameTok;
    public readonly Node startValueNode;
    public readonly Node endValueNode;
    public readonly Node? stepValueNode;
    public readonly Node bodyNode;
    public readonly bool retNull;

    public ForNode(Token varNameTok, Node startValueNode, Node endValueNode, Node? stepValueNode, Node bodyNode, bool retNull)
    {
        this.varNameTok = varNameTok;
        this.startValueNode = startValueNode;
        this.endValueNode = endValueNode;
        this.stepValueNode = stepValueNode;
        this.bodyNode = bodyNode;
        this.retNull = retNull;

        StartPos = varNameTok.StartPos;
        EndPos = bodyNode.EndPos;
    }
}

// This node represents a while loop
public class WhileNode : Node
{
    public readonly Node conditionNode;
    public readonly Node bodyNode;
    public readonly bool retNull;

    public WhileNode(Node conditionNode, Node bodyNode, bool retNull)
    {
        this.conditionNode = conditionNode;
        this.bodyNode = bodyNode;
        this.retNull = retNull;

        StartPos = conditionNode.StartPos;
        EndPos = bodyNode.EndPos;
    }
}

// This node represents a function definition
public class FuncDefNode : Node
{
    public readonly Token? varNameTok;
    public readonly List<Token> argNameToks;
    public readonly Node bodyNode;
    public readonly bool retNull;

    public FuncDefNode(Token? varNameTok, List<Token> argNameToks, Node bodyNode, bool autoReturn)
    {
        this.varNameTok = varNameTok;
        this.argNameToks = argNameToks ?? [];
        this.bodyNode = bodyNode;
        this.retNull = autoReturn;

        if (varNameTok is not null)
        {
            this.StartPos = varNameTok.StartPos;
        }
        else if (argNameToks?.Count > 0)
        {
            this.StartPos = argNameToks[0].StartPos;
        }
        else
        {
            this.StartPos = bodyNode.StartPos;
        }
        EndPos = bodyNode.EndPos;
    }
}

// This node represents a function call
public class CallNode : Node
{
    public readonly Node nodeToCall;
    public readonly List<Node> argNodes;

    public CallNode(Node nodeToCall, List<Node> argNodes)
    {
        this.nodeToCall = nodeToCall;
        this.argNodes = argNodes;

        StartPos = nodeToCall.StartPos;
        EndPos = argNodes.Count > 0 ? argNodes[^1].EndPos : nodeToCall.EndPos;
    }
    public override string ToString()
    {
        return $"{nodeToCall} -> " + string.Join(',', argNodes);
    }
}

// This node represents a return statement
public class ReturnNode : Node
{
    public readonly Node? nodeToReturn;

    public ReturnNode(Node? nodeToReturn, Position startPos, Position endPos)
    {
        this.nodeToReturn = nodeToReturn;
        StartPos = startPos;
        EndPos = endPos;
    }
}

// This node represents a continue statement
public class ContinueNode : Node
{
    public ContinueNode(Position startPos, Position endPos)
    {
        StartPos = startPos;
        EndPos = endPos;
    }
}

// This node represents a break statement
public class BreakNode : Node
{
    public BreakNode(Position startPos, Position endPos)
    {
        StartPos = startPos;
        EndPos = endPos;
    }
}

