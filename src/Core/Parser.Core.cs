using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using YSharp.Types.AST;
using YSharp.Types.Lexer;

namespace YSharp.Core;

// the parser which is used to make the abstract syntax tree
public partial class Parser
{
    private readonly ImmutableArray<IToken> tokens;
    public int tokIndex = -1;
    public IToken currentToken;

    public Parser(List<IToken> tokens)
    {
        this.tokens = tokens.ToImmutableArray();
        currentToken = NullToken.Instance;
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
