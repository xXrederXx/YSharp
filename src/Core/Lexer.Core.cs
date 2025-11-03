using System.Runtime.CompilerServices;
using System.Text;
using YSharp.Types.Common;
using YSharp.Types.Lexer;
using YSharp.Utils;

namespace YSharp.Core;

public sealed partial class Lexer
{
    private readonly string text;
    private Position pos;
    private char current_char = char.MaxValue;
    private readonly StringBuilder stringBuilder = new();

    // Initalizer
    public Lexer(string text, string fileName)
    {
        this.text = text;

        // The .MaxValue enshures that the first advance call overflows to
        byte FileId = FileNameRegistry.GetFileId(fileName);
        pos = new Position(0, 0, 0, FileId);
        current_char = pos.Index < text.Length ? text[pos.Index] : char.MaxValue;
    }

    // this uptades to the next charachter
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void Advance()
    {
        pos = pos.Advance(current_char);
        current_char = pos.Index < text.Length ? text[pos.Index] : char.MaxValue;
    }

    private static bool IsValidIdentifierChar(char c) => char.IsLetterOrDigit(c) || c == '_';


    // generates all tokens
    public (List<IToken>, Error) MakeTokens()
    {
        List<IToken> tokens = [];

        while (current_char != char.MaxValue)
        {
            if (current_char is ' ' or '\t')
            {
                // Discards spaces and tabs
                Advance();
            }
            else if (current_char == '#')
            {
                Advance();
                SkipComment();
            }
            else if (current_char == ':')
            {
                Advance();
                SkipTypeAnotation();
            }
            else if (current_char is ';' or '\n' or '\r')
            {
                tokens.Add(new TokenNoValue(TokenType.NEWLINE, pos, pos));
                Advance();
            }
            else if (current_char == '.')
            {
                tokens.Add(new TokenNoValue(TokenType.DOT, pos, pos));
                Advance();
            }
            else if (char.IsDigit(current_char)) // Check for digits (int)
            {
                (IToken tok, Error err) = MakeNumber();
                if (err.IsError)
                {
                    return ([], err);
                }
                tokens.Add(tok);
            }
            else if (char.IsLetter(current_char)) // Check for letters
            {
                tokens.Add(MakeIdentifier());
            }
            else if (current_char == '"')
            {
                (IToken tok, Error err) = MakeString();
                if (err.IsError)
                {
                    return ([], err);
                }
                tokens.Add(tok);
            }
            // Arithmetic
            else if (current_char == '+')
            {
                tokens.Add(MakePlus());
            }
            else if (current_char == '-')
            {
                tokens.Add(MakeMinus());
            }
            else if (current_char == '*')
            {
                tokens.Add(MakeDecicion('=', TokenType.MUEQ, TokenType.MUL));
                Advance();
            }
            else if (current_char == '/')
            {
                tokens.Add(MakeDecicion('=', TokenType.DIEQ, TokenType.DIV));
                Advance();
            }
            else if (current_char == '^')
            {
                tokens.Add(new TokenNoValue(TokenType.POW, pos, pos));
                Advance();
            }
            else if (current_char == '(')
            {
                tokens.Add(new TokenNoValue(TokenType.LPAREN, pos, pos));
                Advance();
            }
            else if (current_char == ')')
            {
                tokens.Add(new TokenNoValue(TokenType.RPAREN, pos, pos));
                Advance();
            }
            // Comparison
            else if (current_char == '!')
            {
                (IToken, Error) res = MakeNotEquals();
                if (res.Item2.IsError)
                {
                    return ([], res.Item2);
                }
                tokens.Add(res.Item1);
                Advance();
            }
            else if (current_char == '=')
            {
                tokens.Add(MakeDecicion('=', TokenType.EE, TokenType.EQ));
                Advance();
            }
            else if (current_char == '<')
            {
                tokens.Add(MakeDecicion('=', TokenType.LTE, TokenType.LT));
                Advance();
            }
            else if (current_char == '>')
            {
                tokens.Add(MakeDecicion('=', TokenType.GTE, TokenType.GT));
                Advance();
            }
            // Other
            else if (current_char == ',')
            {
                tokens.Add(new TokenNoValue(TokenType.COMMA, pos, pos));
                Advance();
            }
            else if (current_char == '[')
            {
                tokens.Add(new TokenNoValue(TokenType.LSQUARE, pos, pos));
                Advance();
            }
            else if (current_char == ']')
            {
                tokens.Add(new TokenNoValue(TokenType.RSQUARE, pos, pos));
                Advance();
            }
            else
            {
                // Not a valid token, return an error
                return (new List<IToken>(), new IllegalCharError(pos, current_char));
            }
        }
        tokens.Add(new TokenNoValue(TokenType.EOF, pos, pos)); // Add the End Of File token
        return (tokens, ErrorNull.Instance);
    }
}
