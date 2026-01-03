using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using YSharp.Lexer;

namespace YSharp.Parser;

// the parser which is used to make the abstract syntax tree
public sealed partial class Parser
{
    public BaseToken currentToken;
    public int tokIndex;
    private readonly ImmutableArray<BaseToken> tokens;

    public Parser(List<BaseToken> tokens)
    {
        this.tokens = tokens.ToImmutableArray();
        currentToken = NullToken.Instance;
        tokIndex = 0;
        UpdateCurrentTok();
    }

    public ParseResult Parse() => Statements();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void AdvanceParser(ParseResult res)
    {
        tokIndex++;
        UpdateCurrentTok();
        res.Advance();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private BaseToken Reverse(int amount = 1)
    {
        tokIndex -= amount;
        UpdateCurrentTok();
        return currentToken;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void UpdateCurrentTok()
    {
        if (tokIndex >= 0 && tokIndex < tokens.Length)
            currentToken = tokens[tokIndex];
    }
}
