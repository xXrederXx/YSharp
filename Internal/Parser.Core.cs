using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using YSharp.Types;
using YSharp.Types.InternalTypes;

namespace YSharp.Internal;

public class ParseResult
{
    public Error Error { get; private set; } = ErrorNull.Instance;
    public INode Node { get; private set; } = NodeNull.Instance;
    public int ToReverseCount { get; private set; } = 0;
    public bool HasError => Error.IsError;
    private int _advanceCount = 0;

    public void ResetError()
    {
        Error = ErrorNull.Instance;
    }

    public INode? TryRegister(ParseResult result)
    {
        if (result.HasError)
        {
            ToReverseCount = result._advanceCount;
            return null;
        }
        return Register(result);
    }

    public INode Register(ParseResult result)
    {
        _advanceCount += result._advanceCount;
        Error = result.Error;
        return result.Node;
    }

    public void Advance()
    {
        _advanceCount++;
    }

    public ParseResult Success(INode node)
    {
        Node = node;
        return this;
    }

    public ParseResult Failure(Error error)
    {
        if (!HasError || _advanceCount == 0)
        {
            Error = error;
        }
        return this;
    }

    public override string ToString() =>
        $"Node: {Node} / Error: {Error} / AdvanceCount: {_advanceCount}";
}

// the parser which is used to make the abstract syntax tree
public partial class Parser
{
    private readonly ImmutableArray<IToken> tokens;
    public int tokIndex = -1;
    public IToken currentToken;

    public Parser(List<IToken> tokens)
    {
        this.tokens = tokens.ToImmutableArray();
        currentToken = new Token<TokenNoValueType>(TokenType.NULL);
        AdvanceParser();
    }

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

    public ParseResult Parse()
    {
        return Statements();
    }
}
