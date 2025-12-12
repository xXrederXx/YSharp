using YSharp.Types.Common;
using YSharp.Types.Lexer;
// ReSharper disable ConvertToPrimaryConstructor

namespace YSharp.Types.AST;

public abstract class BaseNode
{
    public Position EndPos;

    public Position StartPos;

    protected BaseNode(in Position startPos, in Position endPos)
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
    public readonly Token<double> Tok;

    public NumberNode(Token<double> tok)
        : base(tok.StartPos, tok.EndPos)
    {
        this.Tok = tok;
    }

    public override string ToString() => Tok.ToString();
}

// This node represents a string token
public sealed class StringNode : BaseNode
{
    public readonly Token<string> Tok;

    public StringNode(Token<string> tok)
        : base(tok.StartPos, tok.EndPos)
    {
        this.Tok = tok;
    }

    public override string ToString() => Tok.ToString();
}

// This node represents a list of elements
public sealed class ListNode : BaseNode
{
    public readonly BaseNode[] ElementNodes;

    public ListNode(List<BaseNode> elementNodes, in Position posStart, in Position posEnd)
        : base(posStart, posEnd)
    {
        this.ElementNodes = elementNodes.ToArray();
    }

    public override string ToString()
    {
        return "[" + string.Join(',', ElementNodes.Select(x => x.ToString())) + "]";
    }
}

// This node represents a binary operation
public sealed class BinOpNode : BaseNode
{
    public readonly BaseNode LeftNode;
    public readonly IToken OpTok;
    public readonly BaseNode RightNode;

    public BinOpNode(BaseNode leftNode, IToken opTok, BaseNode rightNode)
        : base(leftNode.StartPos, rightNode.EndPos)
    {
        this.LeftNode = leftNode;
        this.OpTok = opTok;
        this.RightNode = rightNode;
    }

    public override string ToString() => $"({LeftNode}, {OpTok}, {RightNode})";
}

// This node represents a unary operation
public sealed class UnaryOpNode : BaseNode
{
    public readonly BaseNode Node;
    public readonly IToken OpTok;

    public UnaryOpNode(IToken opTok, BaseNode node)
        : base(opTok.StartPos, node.EndPos)
    {
        this.OpTok = opTok;
        this.Node = node;
    }

    public override string ToString() => $"({OpTok}, {Node})";
}

// This node represents a variable access
public sealed class VarAccessNode : BaseNode
{
    public readonly Token<string> VarNameTok;
    public bool FromCall = false;

    public VarAccessNode(Token<string> varNameTok)
        : base(varNameTok.StartPos, varNameTok.EndPos)
    {
        this.VarNameTok = varNameTok;
    }

    public override string ToString() => $"Access {VarNameTok.Value}";
}

// This node represents a variable assignment
public sealed class VarAssignNode : BaseNode
{
    public readonly BaseNode ValueNode;
    public readonly Token<string> VarNameTok;

    public VarAssignNode(Token<string> varNameTok, BaseNode valueNode)
        : base(varNameTok.StartPos, valueNode.EndPos)
    {
        this.VarNameTok = varNameTok;
        this.ValueNode = valueNode;
    }

    public override string ToString() => VarNameTok.Value + " = " + ValueNode;
}

// This node represents a dot (.) variable access
public sealed class DotVarAccessNode : BaseNode
{
    public readonly BaseNode Parent;
    public readonly Token<string> VarNameTok;

    public DotVarAccessNode(Token<string> varNameTok, BaseNode parent)
        : base(varNameTok.StartPos, varNameTok.EndPos)
    {
        this.Parent = parent;
        this.VarNameTok = varNameTok;
    }

    public override string ToString() => $"Access {VarNameTok.Value} from {Parent}";
}

// This node represents a function call using dot notation
public sealed class DotCallNode : BaseNode
{
    public readonly BaseNode[] ArgNodes;
    public readonly Token<string> FuncNameTok;
    public readonly BaseNode Parent;

    public DotCallNode(Token<string> funcNameTok, List<BaseNode> argNodes, BaseNode parent)
        : base(funcNameTok.StartPos, argNodes.Count > 0 ? argNodes[^1].EndPos : funcNameTok.EndPos)
    {
        this.FuncNameTok = funcNameTok;
        this.ArgNodes = argNodes.ToArray();
        this.Parent = parent;
    }

    public override string ToString() =>
        $"Calling {FuncNameTok} on {Parent} with args: {string.Join(", ", ArgNodes.Select(x => x.ToString()))}";
}

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

// This node represents an if statement
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

// This node represents a for loop
public sealed class ForNode : BaseNode
{
    public readonly BaseNode BodyNode;
    public readonly BaseNode EndValueNode;
    public readonly bool RetNull;
    public readonly BaseNode StartValueNode;
    public readonly BaseNode? StepValueNode;
    public readonly Token<string> VarNameTok;

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
        this.VarNameTok = varNameTok;
        this.StartValueNode = startValueNode;
        this.EndValueNode = endValueNode;
        this.StepValueNode = stepValueNode;
        this.BodyNode = bodyNode;
        this.RetNull = retNull;
    }

    public override string ToString() =>
        $"for {VarNameTok} start {StartValueNode} end {EndValueNode} with step {StepValueNode} do {BodyNode} and return Null {RetNull}";
}

// This node represents a while loop
public sealed class WhileNode : BaseNode
{
    public readonly BaseNode BodyNode;
    public readonly BaseNode ConditionNode;
    public readonly bool RetNull;

    public WhileNode(BaseNode conditionNode, BaseNode bodyNode, bool retNull)
        : base(conditionNode.StartPos, bodyNode.EndPos)
    {
        this.ConditionNode = conditionNode;
        this.BodyNode = bodyNode;
        this.RetNull = retNull;
    }

    public override string ToString() => $"while {ConditionNode} do {BodyNode}";
}

// This node represents a function definition
public sealed class FuncDefNode : BaseNode
{
    public readonly IToken[] ArgNameTokens;
    public readonly BaseNode BodyNode;
    public readonly bool RetNull;
    public readonly Token<string> VarNameTok;

    public FuncDefNode(
        Token<string> varNameTok,
        List<IToken> argNameTokens,
        BaseNode bodyNode,
        bool autoReturn
    )
        : base(varNameTok.StartPos, bodyNode.EndPos)
    {
        this.VarNameTok = varNameTok;
        this.ArgNameTokens = argNameTokens.ToArray();
        this.BodyNode = bodyNode;
        RetNull = autoReturn;
    }

    public override string ToString() =>
        $"Define Function {VarNameTok} with args {string.Join(", ", ArgNameTokens.Select(x => x.ToString()))} and do {BodyNode} (return null {RetNull})";
}

// This node represents a function call
public sealed class CallNode : BaseNode
{
    public readonly BaseNode[] ArgNodes;
    public readonly BaseNode NodeToCall;

    public CallNode(BaseNode nodeToCall, List<BaseNode> argNodes)
        : base(nodeToCall.StartPos, argNodes.Count > 0 ? argNodes[^1].EndPos : nodeToCall.EndPos)
    {
        this.NodeToCall = nodeToCall;
        this.ArgNodes = argNodes.ToArray();
    }

    public override string ToString() =>
        $"{NodeToCall} -> " + string.Join(',', ArgNodes.Select(x => x.ToString()));
}

// This node represents a return statement
public sealed class ReturnNode : BaseNode
{
    public readonly BaseNode? NodeToReturn;

    public ReturnNode(BaseNode? nodeToReturn, in Position startPos, in Position endPos)
        : base(startPos, endPos)
    {
        this.NodeToReturn = nodeToReturn;
    }

    public override string ToString() => $"return {NodeToReturn}";
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
    public readonly Token<string> CatchVarName;
    public readonly BaseNode TryNode;

    public TryCatchNode(BaseNode tryNode, BaseNode catchNode, Token<string> catchVarName)
        : base(tryNode.StartPos, catchNode is NodeNull _ ? tryNode.EndPos : catchNode.EndPos)
    {
        TryNode = tryNode;
        CatchNode = catchNode;
        CatchVarName = catchVarName;
    }

    public override string ToString() => $"Try {TryNode} Catch {CatchNode} as {CatchVarName}";
}

public sealed class ImportNode : BaseNode
{
    public readonly Token<string> PathTok;

    public ImportNode(Token<string> pathTok, in Position startPos, in Position endPos)
        : base(startPos, endPos)
    {
        PathTok = pathTok;
    }

    public override string ToString() => $"Import {PathTok}";
}

public sealed class SuffixAssignNode : BaseNode
{
    public readonly bool IsAdd;
    public readonly string VarName;

    public SuffixAssignNode(Token<string> varName, bool isAdd)
        : base(varName.StartPos, varName.EndPos)
    {
        this.VarName = varName.Value;
        this.IsAdd = isAdd;
    }
}