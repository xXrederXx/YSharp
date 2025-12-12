using YSharp.Types.Common;

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

// NumberNode implements INode

// This node represents a string token

// This node represents a list of elements

// This node represents a binary operation

// This node represents a unary operation

// This node represents a variable access

// This node represents a variable assignment

// This node represents a dot (.) variable access

// This node represents a function call using dot notation

// This node represents an if statement

// This node represents a for loop

// This node represents a while loop

// This node represents a function definition

// This node represents a function call

// This node represents a return statement

// This node represents a continue statement

// This node represents a break statement