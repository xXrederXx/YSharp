using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using YSharp.Lexer;


namespace YSharp.Parser;


// the parser which is used to make the abstract syntax tree
public partial class Parser
{
    public IToken currentToken;
    public int tokIndex = -1;
    private readonly ImmutableArray<IToken> tokens;

    public Parser(List<IToken> tokens)
    {
        this.tokens = tokens.ToImmutableArray();
        currentToken = NullToken.Instance;
        AdvanceParser();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AdvanceParser(ParseResult res)
    {
        tokIndex++;
        UpdateCurrentTok();
        res.Advance();
    }

    public ParseResult Parse() => Statements();

    private void AdvanceParser()
    {
        tokIndex++;
        UpdateCurrentTok();
    }

    private IToken Reverse(int amount = 1)
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