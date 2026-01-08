using System.Runtime.CompilerServices;
using System.Text;
using YSharp.Common;
using YSharp.Util;

namespace YSharp.Lexer;

using LexerOperationResult = Result<BaseToken, Error>;
using LexerResult = Result<List<BaseToken>, Error>;

public sealed partial class Lexer
{
    private const char StopChar = char.MaxValue;
    private readonly StringBuilder stringBuilder = new();
    private readonly string text;
    private char currentChar;
    private Position pos;

    private static readonly Dictionary<char, TokenType> singelCharToken = new()
    {
        { '\n', TokenType.NEWLINE },
        { '\r', TokenType.NEWLINE },
        { ';', TokenType.NEWLINE },
        { '.', TokenType.DOT },
        { '(', TokenType.LPAREN },
        { ')', TokenType.RPAREN },
        { ',', TokenType.COMMA },
        { '[', TokenType.LSQUARE },
        { ']', TokenType.RSQUARE },
        { '^', TokenType.POW },
    };
    private readonly Dictionary<char, Func<BaseToken>> safeMultiCharToken;
    private readonly Dictionary<char, Func<LexerOperationResult>> unsafeMultiCharToken;
    private readonly Dictionary<char, Action?> skipCharToken;

    // Initalizer
    public Lexer(string text, string fileName)
    {
        this.text = text;

        // The .MaxValue enshures that the first advance call overflows to
        byte fileId = FileNameRegistry.GetFileId(fileName);
        pos = new Position(0, 0, 0, fileId);
        currentChar = pos.Index < text.Length ? text[pos.Index] : StopChar;

        safeMultiCharToken = new Dictionary<char, Func<BaseToken>>()
        {
            { '+', MakePlus },
            { '-', MakeMinus },
            { '*', () => MakeDecision('=', TokenType.MUEQ, TokenType.MUL) },
            { '/', () => MakeDecision('=', TokenType.DIEQ, TokenType.DIV) },
            { '=', () => MakeDecision('=', TokenType.EE, TokenType.EQ) },
            { '<', () => MakeDecision('<', TokenType.LTE, TokenType.LT) },
            { '>', () => MakeDecision('>', TokenType.GTE, TokenType.GT) },
        };

        unsafeMultiCharToken = new Dictionary<char, Func<LexerOperationResult>>()
        {
            { '"', MakeString },
            { '!', MakeNotEquals },
        };
        skipCharToken = new Dictionary<char, Action?>()
        {
            { ' ', null },
            { '\t', null },
            { '#', SkipComment },
            { ':', SkipTypeAnotation },
        };
    }

    // generates all tokens
    public LexerResult MakeTokens()
    {
        List<BaseToken> tokens = [];

        while (currentChar != StopChar)
            if (skipCharToken.TryGetValue(currentChar, out Action? action))
            {
                Advance();
                action?.Invoke();
            }
            else if (singelCharToken.TryGetValue(currentChar, out TokenType type))
            {
                tokens.Add(new BaseToken(type, pos, pos));
                Advance();
            }
            else if (unsafeMultiCharToken.TryGetValue(currentChar, out var unsafeFunc))
            {
                LexerOperationResult res = unsafeFunc.Invoke();
                if (!res.TryGetValue(out BaseToken tok))
                    return LexerResult.Fail(res.GetError());
                tokens.Add(tok);
            }
            else if (safeMultiCharToken.TryGetValue(currentChar, out Func<BaseToken>? safeFunc))
            {
                tokens.Add(safeFunc.Invoke());
            }
            else if (char.IsDigit(currentChar)) // Check for digits (int)
            {
                LexerOperationResult res = MakeNumber();
                if (!res.TryGetValue(out BaseToken tok))
                    return LexerResult.Fail(res.GetError());
                tokens.Add(tok);
            }
            else if (char.IsLetter(currentChar)) // Check for letters
            {
                tokens.Add(MakeIdentifier());
            }
            else
            {
                // Not a valid token, return an error
                return LexerResult.Fail(new IllegalCharError(pos, currentChar));
            }

        tokens.Add(new BaseToken(TokenType.EOF, pos, pos)); // Add the End Of File token
        return LexerResult.Succses(tokens);
    }

    private static bool IsValidIdentifierChar(char c) => char.IsLetterOrDigit(c) || c == '_';

    // this uptades to the next charachter
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void Advance()
    {
        pos = pos.Advance(currentChar);
        currentChar = pos.Index < text.Length ? text[pos.Index] : StopChar;
    }
}
