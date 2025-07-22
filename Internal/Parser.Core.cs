using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using YSharp.Types;
using YSharp.Types.InternalTypes;
using YSharp.Utility;

namespace YSharp.Internal;

public class ParseResult
{
    public Error Error { get; private set; } = ErrorNull.Instance;
    public INode Node { get; private set; } = NodeNull.Instance;
    private int _advanceCount = 0;
    public int ToReverseCount { get; private set; } = 0;

    // Check if an error exists
    public bool HasError => Error.IsError;

    public void ResetError()
    {
        Error = ErrorNull.Instance;
    }

    // Try to register the result; if error is present, mark reversal
    public INode? TryRegister(ParseResult result)
    {
        if (result.HasError)
        {
            ToReverseCount = result._advanceCount;
            return null;
        }
        return Register(result);
    }

    // Register result and accumulate advances
    public INode Register(ParseResult result)
    {
        _advanceCount += result._advanceCount;
        Error = result.Error;
        return result.Node;
    }

    public bool SafeRegrister(ParseResult result, out INode node)
    {
        node = Register(result);
        return result.HasError;
    }

    // Register an advancement count increment
    public void Advance()
    {
        _advanceCount++;
    }

    // Return successful result with Node
    public ParseResult Success(INode node)
    {
        Node = node;
        return this;
    }

    // Return failure result and update error if not already set
    public ParseResult Failure(Error error)
    {
        if (!HasError || _advanceCount == 0)
        {
            Error = error;
        }
        //* For testing -> Console.WriteLine(error.ToString() + Node.ToString());
        return this;
    }

    public override string ToString()
    {
        return Node.ToString() ?? "null";
    }
}

// the parser which is used to make the abstract syntax tree
public partial class Parser
{
    private readonly ImmutableArray<IToken> tokens;
    public int tokIndex = -1;
    public IToken currentToken;

    // initalizer
    public Parser(List<IToken> tokens)
    {
        this.tokens = tokens.ToImmutableArray();
        currentToken = new Token<TokenNoValueType>(TokenType.NULL);
        AdvanceParser();
    }

    // goes to the next token
    private void AdvanceParser()
    {
        tokIndex++;
        UpdateCurrentTok();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AdvanceParser(ParseResult res)
    {
        tokIndex++;
        UpdateCurrentTok();
        res.Advance();
    }

    private IToken Reverse(int amount = 1)
    {
        tokIndex -= amount;
        UpdateCurrentTok();
        return currentToken;
    }

    private void UpdateCurrentTok()
    {
        if (tokIndex >= 0 && tokIndex < tokens.Length)
        {
            currentToken = tokens[tokIndex];
        }
    }

    // main function which parses all tokens
    public ParseResult Parse()
    {
        return Statements();
    }
}
