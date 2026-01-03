using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using YSharp.Lexer;


namespace YSharp.Parser;


// the parser which is used to make the abstract syntax tree
public partial class Parser
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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AdvanceParser(ParseResult res)
    {
        tokIndex++;
        UpdateCurrentTok();
        res.Advance();
    }

    public ParseResult Parse() => Statements();

    private BaseToken Reverse(int amount = 1)
    {
        tokIndex -= amount;
        UpdateCurrentTok();
        return currentToken;
    }

    private void UpdateCurrentTok()
    {
        if (tokIndex >= 0 && tokIndex < tokens.Length) currentToken = tokens[tokIndex];
    }
}