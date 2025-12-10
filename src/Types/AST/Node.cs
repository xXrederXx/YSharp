using YSharp.Types.Common;
using YSharp.Types.Lexer;

namespace YSharp.Types.AST;

// Interface definition
public class BaseNode
{
    public Position EndPos;

    public Position StartPos;

    public BaseNode(in Position startPos, in Position endPos)
    {
        StartPos = startPos;
        EndPos = endPos;
    }
}

public sealed class NodeNull : BaseNode
{
    public static readonly NodeNull Instance = new();

    private NodeNull()
        : base(Position.Null, Position.Null) { }

    public override string ToString() => "Null Node";
}

// NumberNode implements INode
public sealed class NumberNode : BaseNode
{
    public readonly Token<double> tok;

    public NumberNode(Token<double> tok)
        : base(tok.StartPos, tok.EndPos)
    {
        this.tok = tok;
    }

    public override string ToString() => tok.ToString();
}

// This node represents a string token
public sealed class StringNode : BaseNode
{
    public readonly Token<string> tok;

    public StringNode(Token<string> tok)
        : base(tok.StartPos, tok.EndPos)
    {
        this.tok = tok;
    }

    public override string ToString() => tok.ToString();
}

// This node represents a list of elements
public sealed class ListNode : BaseNode
{
    public readonly BaseNode[] elementNodes;

    public ListNode(List<BaseNode> elementNodes, in Position posStart, in Position posEnd)
        : base(posStart, posEnd)
    {
        this.elementNodes = elementNodes.ToArray();
    }

    public override string ToString()
    {
        return "[" + string.Join(',', elementNodes.Select(x => x.ToString())) + "]";
    }
}

// This node represents a binary operation
public sealed class BinOpNode : BaseNode
{
    public readonly BaseNode leftNode;
    public readonly IToken opTok;
    public readonly BaseNode rightNode;

    public BinOpNode(BaseNode leftNode, IToken opTok, BaseNode rightNode)
        : base(leftNode.StartPos, rightNode.EndPos)
    {
        this.leftNode = leftNode;
        this.opTok = opTok;
        this.rightNode = rightNode;
    }

    public override string ToString() => $"({leftNode}, {opTok}, {rightNode})";
}

// This node represents a unary operation
public sealed class UnaryOpNode : BaseNode
{
    public readonly BaseNode node;
    public readonly IToken opTok;

    public UnaryOpNode(IToken opTok, BaseNode node)
        : base(opTok.StartPos, node.EndPos)
    {
        this.opTok = opTok;
        this.node = node;
    }

    public override string ToString() => $"({opTok}, {node})";
}

// This node represents a variable access
public sealed class VarAccessNode : BaseNode
{
    public readonly Token<string> varNameTok;
    public bool fromCall = false;

    public VarAccessNode(Token<string> varNameTok)
        : base(varNameTok.StartPos, varNameTok.EndPos)
    {
        this.varNameTok = varNameTok;
    }

    public override string ToString() => $"Access {varNameTok.Value}";
}

// This node represents a variable assignment
public sealed class VarAssignNode : BaseNode
{
    public readonly BaseNode valueNode;
    public readonly Token<string> varNameTok;

    public VarAssignNode(Token<string> varNameTok, BaseNode valueNode)
        : base(varNameTok.StartPos, valueNode.EndPos)
    {
        this.varNameTok = varNameTok;
        this.valueNode = valueNode;
    }

    public override string ToString() => varNameTok.Value + " = " + valueNode;
}

// This node represents a dot (.) variable access
public sealed class DotVarAccessNode : BaseNode
{
    public readonly BaseNode parent;
    public readonly Token<string> varNameTok;

    public DotVarAccessNode(Token<string> varNameTok, BaseNode parent)
        : base(varNameTok.StartPos, varNameTok.EndPos)
    {
        this.parent = parent;
        this.varNameTok = varNameTok;
    }

    public override string ToString() => $"Access {varNameTok.Value} from {parent}";
}

// This node represents a function call using dot notation
public sealed class DotCallNode : BaseNode
{
    public readonly BaseNode[] argNodes;
    public readonly Token<string> funcNameTok;
    public readonly BaseNode parent;

    public DotCallNode(Token<string> funcNameTok, List<BaseNode> argNodes, BaseNode parent)
        : base(funcNameTok.StartPos, argNodes.Count > 0 ? argNodes[^1].EndPos : funcNameTok.EndPos)
    {
        this.funcNameTok = funcNameTok;
        this.argNodes = argNodes.ToArray();
        this.parent = parent;
    }

    public override string ToString() =>
        $"Calling {funcNameTok} on {parent} with args: {string.Join(", ", argNodes.Select(x => x.ToString()))}";
}

public sealed class SubIfNode : BaseNode
{
    public readonly BaseNode condition;
    public readonly BaseNode expression;

    public SubIfNode(BaseNode condition, BaseNode expression)
        : base(condition.StartPos, expression.EndPos)
    {
        this.condition = condition;
        this.expression = expression;
    }

    public override string ToString() => $"If {condition} then {expression}";
}

// This node represents an if statement
public sealed class IfNode : BaseNode
{
    public readonly SubIfNode[] cases;
    public readonly BaseNode elseNode;

    public IfNode(List<SubIfNode> cases, BaseNode elseNode)
        : base(
            cases[0].condition.StartPos,
            elseNode != NodeNull.Instance ? elseNode.EndPos : cases[^1].EndPos
        )
    {
        this.cases = cases.ToArray();
        this.elseNode = elseNode;
    }

    public override string ToString() =>
        $"All If Casses: {string.Join(", ", cases.Select(x => x.ToString()))} Else: {elseNode}";
}

// This node represents a for loop
public sealed class ForNode : BaseNode
{
    public readonly BaseNode bodyNode;
    public readonly BaseNode endValueNode;
    public readonly bool retNull;
    public readonly BaseNode startValueNode;
    public readonly BaseNode? stepValueNode;
    public readonly Token<string> varNameTok;

    public ForNode(
        Token<string> varNameTok,
        BaseNode startValueNode,
        BaseNode endValueNode,
        BaseNode? stepValueNode,
        BaseNode bodyNode,
        bool retNull
    )
        : base(varNameTok.StartPos, bodyNode.EndPos)
    {
        this.varNameTok = varNameTok;
        this.startValueNode = startValueNode;
        this.endValueNode = endValueNode;
        this.stepValueNode = stepValueNode;
        this.bodyNode = bodyNode;
        this.retNull = retNull;
    }

    public override string ToString() =>
        $"for {varNameTok} start {startValueNode} end {endValueNode} with step {stepValueNode} do {bodyNode} and return Null {retNull}";
}

// This node represents a while loop
public sealed class WhileNode : BaseNode
{
    public readonly BaseNode bodyNode;
    public readonly BaseNode conditionNode;
    public readonly bool retNull;

    public WhileNode(BaseNode conditionNode, BaseNode bodyNode, bool retNull)
        : base(conditionNode.StartPos, bodyNode.EndPos)
    {
        this.conditionNode = conditionNode;
        this.bodyNode = bodyNode;
        this.retNull = retNull;
    }

    public override string ToString() => $"while {conditionNode} do {bodyNode}";
}

// This node represents a function definition
public sealed class FuncDefNode : BaseNode
{
    public readonly IToken[] argNameToks;
    public readonly BaseNode bodyNode;
    public readonly bool retNull;
    public readonly Token<string> varNameTok;

    public FuncDefNode(
        Token<string> varNameTok,
        List<IToken> argNameToks,
        BaseNode bodyNode,
        bool autoReturn
    )
        : base(varNameTok.StartPos, bodyNode.EndPos)
    {
        this.varNameTok = varNameTok;
        this.argNameToks = argNameToks.ToArray();
        this.bodyNode = bodyNode;
        retNull = autoReturn;
    }

    public override string ToString() =>
        $"Define Function {varNameTok} with args {string.Join(", ", argNameToks.Select(x => x.ToString()))} and do {bodyNode} (return null {retNull})";
}

// This node represents a function call
public sealed class CallNode : BaseNode
{
    public readonly BaseNode[] argNodes;
    public readonly BaseNode nodeToCall;

    public CallNode(BaseNode nodeToCall, List<BaseNode> argNodes)
        : base(nodeToCall.StartPos, argNodes.Count > 0 ? argNodes[^1].EndPos : nodeToCall.EndPos)
    {
        this.nodeToCall = nodeToCall;
        this.argNodes = argNodes.ToArray();
    }

    public override string ToString() =>
        $"{nodeToCall} -> " + string.Join(',', argNodes.Select(x => x.ToString()));
}

// This node represents a return statement
public sealed class ReturnNode : BaseNode
{
    public readonly BaseNode? nodeToReturn;

    public ReturnNode(BaseNode? nodeToReturn, in Position startPos, in Position endPos)
        : base(startPos, endPos)
    {
        this.nodeToReturn = nodeToReturn;
    }

    public override string ToString() => $"return {nodeToReturn}";
}

// This node represents a continue statement
public sealed class ContinueNode : BaseNode
{
    public ContinueNode(in Position startPos, in Position endPos)
        : base(startPos, endPos) { }

    public override string ToString() => "Continue Node";
}

// This node represents a break statement
public sealed class BreakNode : BaseNode
{
    public BreakNode(in Position startPos, in Position endPos)
        : base(startPos, endPos) { }

    public override string ToString() => "Break Node";
}

public sealed class TryCatchNode : BaseNode
{
    public readonly BaseNode CatchNode;
    public readonly Token<string> ChatchVarName;
    public readonly BaseNode TryNode;

    public TryCatchNode(BaseNode tryNode, BaseNode catchNode, Token<string> catchVarName)
        : base(tryNode.StartPos, catchNode is NodeNull _ ? tryNode.EndPos : catchNode.EndPos)
    {
        TryNode = tryNode;
        CatchNode = catchNode;
        ChatchVarName = catchVarName;
    }

    public override string ToString() => $"Try {TryNode} Catch {CatchNode} as {ChatchVarName}";
}

public sealed class ImportNode : BaseNode
{
    public Token<string> PathTok;

    public ImportNode(Token<string> pathTok, in Position startPos, in Position endPos)
        : base(startPos, endPos)
    {
        PathTok = pathTok;
    }

    public override string ToString() => $"Import {PathTok}";
}

public sealed class SuffixAssignNode : BaseNode
{
    public bool isAdd;
    public string varName;

    public SuffixAssignNode(Token<string> varName, bool isAdd)
        : base(varName.StartPos, varName.EndPos)
    {
        this.varName = varName.Value;
        this.isAdd = isAdd;
    }
}