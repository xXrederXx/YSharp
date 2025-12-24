using System.Runtime.CompilerServices;
using System.Text;
using YSharp.Common;
using YSharp.Util;

namespace YSharp.Lexer;

public sealed partial class Lexer
{
    private readonly StringBuilder stringBuilder = new();
    private readonly string text;
    private char currentChar = char.MaxValue;
    private Position pos;

    // Initalizer
    public Lexer(string text, string fileName)
    {
        this.text = text;

        // The .MaxValue enshures that the first advance call overflows to
        byte fileId = FileNameRegistry.GetFileId(fileName);
        pos = new Position(0, 0, 0, fileId);
        currentChar = pos.Index < text.Length ? text[pos.Index] : char.MaxValue;
    }


    // generates all tokens
    public (List<BaseToken>, Error) MakeTokens()
    {
        List<BaseToken> tokens = [];

        while (currentChar != char.MaxValue)
            if (currentChar is ' ' or '\t')
            {
                // Discards spaces and tabs
                Advance();
            }
            else if (currentChar == '#')
            {
                Advance();
                SkipComment();
            }
            else if (currentChar == ':')
            {
                Advance();
                SkipTypeAnotation();
            }
            else if (currentChar is ';' or '\n' or '\r')
            {
                tokens.Add(new BaseToken(TokenType.NEWLINE, pos, pos));
                Advance();
            }
            else if (currentChar == '.')
            {
                tokens.Add(new BaseToken(TokenType.DOT, pos, pos));
                Advance();
            }
            else if (char.IsDigit(currentChar)) // Check for digits (int)
            {
                (BaseToken tok, Error err) = MakeNumber();
                if (err.IsError) return ([], err);
                tokens.Add(tok);
            }
            else if (char.IsLetter(currentChar)) // Check for letters
                tokens.Add(MakeIdentifier());
            else if (currentChar == '"')
            {
                (BaseToken tok, Error err) = MakeString();
                if (err.IsError) return ([], err);
                tokens.Add(tok);
            }
            // Arithmetic
            else if (currentChar == '+')
                tokens.Add(MakePlus());
            else if (currentChar == '-')
                tokens.Add(MakeMinus());
            else if (currentChar == '*')
                tokens.Add(MakeDecicion('=', TokenType.MUEQ, TokenType.MUL));
            else if (currentChar == '/')
                tokens.Add(MakeDecicion('=', TokenType.DIEQ, TokenType.DIV));
            else if (currentChar == '^')
            {
                tokens.Add(new BaseToken(TokenType.POW, pos, pos));
                Advance();
            }
            else if (currentChar == '(')
            {
                tokens.Add(new BaseToken(TokenType.LPAREN, pos, pos));
                Advance();
            }
            else if (currentChar == ')')
            {
                tokens.Add(new BaseToken(TokenType.RPAREN, pos, pos));
                Advance();
            }
            // Comparison
            else if (currentChar == '!')
            {
                (BaseToken, Error) res = MakeNotEquals();
                if (res.Item2.IsError) return ([], res.Item2);
                tokens.Add(res.Item1);
                Advance();
            }
            else if (currentChar == '=')
                tokens.Add(MakeDecicion('=', TokenType.EE, TokenType.EQ));
            else if (currentChar == '<')
                tokens.Add(MakeDecicion('=', TokenType.LTE, TokenType.LT));
            else if (currentChar == '>')
                tokens.Add(MakeDecicion('=', TokenType.GTE, TokenType.GT));
            // Other
            else if (currentChar == ',')
            {
                tokens.Add(new BaseToken(TokenType.COMMA, pos, pos));
                Advance();
            }
            else if (currentChar == '[')
            {
                tokens.Add(new BaseToken(TokenType.LSQUARE, pos, pos));
                Advance();
            }
            else if (currentChar == ']')
            {
                tokens.Add(new BaseToken(TokenType.RSQUARE, pos, pos));
                Advance();
            }
            else
            {
                // Not a valid token, return an error
                return (new List<BaseToken>(), new IllegalCharError(pos, currentChar));
            }

        tokens.Add(new BaseToken(TokenType.EOF, pos, pos)); // Add the End Of File token
        return (tokens, ErrorNull.Instance);
    }

    private static bool IsValidIdentifierChar(char c) => char.IsLetterOrDigit(c) || c == '_';

    // this uptades to the next charachter
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void Advance()
    {
        pos = pos.Advance(currentChar);
        currentChar = pos.Index < text.Length ? text[pos.Index] : char.MaxValue;
    }
}