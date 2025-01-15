using System.Text;
using YSharp.Types.InternalTypes;

namespace YSharp.Internal;

public class Lexer
{
    private readonly string text;
    private Position pos;
    private char current_char = char.MaxValue;

    // Initalizer
    public Lexer(string text, string fileName)
    {
        this.text = text;
        pos = new Position(-1, 0, -1, fileName); // -1 because Advance auto increments
        Advance();
    }

    // this uptades to the next charachter
    private void Advance()
    {
        pos.Advance(current_char);
        current_char = pos.Index < text.Length ? text[pos.Index] : char.MaxValue;
    }

    // this is used to make a number token of type int or float
    private (Token<double>, Error) MakeNumber()
    {
        StringBuilder stringBuilder = new();
        bool hasDot = false;
        Position posStart = pos;

        while (
            current_char != char.MaxValue
            && (char.IsDigit(current_char) || current_char == '.' || current_char == '_')
        )
        { // while the char is a number or a dot
            if (current_char == '_')
            {
                Advance();
                continue;
            }

            if (current_char == '.')
            {
                if (hasDot)
                {
                    return (
                        new(TokenType.NULL),
                        new IllegalCharError(posStart, "You can't have 2 dots in a number")
                    ); // cant have 2 dots in a number
                }

                hasDot = true;
                stringBuilder.Append('.');
            }
            else
            { // just a number
                stringBuilder.Append(current_char);
            }
            Advance();
        }
        return (
            new Token<double>(
                TokenType.FLOAT,
                double.Parse(stringBuilder.ToString()),
                posStart,
                pos
            ),
            ErrorNull.Instance
        );
    }

    private Token<string> MakeIdentifier()
    {
        StringBuilder stringBuilder = new();
        Position posStart = pos;

        while (
            current_char != char.MaxValue
            && (char.IsLetter(current_char) || char.IsDigit(current_char) || current_char == '_')
        )
        { // if current char is letter, digit or _
            stringBuilder.Append(current_char);
            Advance();
        }

        string idStr = stringBuilder.ToString();
        if (TokenTypeHelper.IsKeyword(idStr))
        {
            return new Token<string>(TokenType.KEYWORD, idStr, posStart, pos);
        }
        else
        {
            return new Token<string>(TokenType.IDENTIFIER, idStr, posStart, pos);
        }
    }

    private (Token<TokenNoValueType>, Error) MakeNotEquals()
    {
        Position posStart = pos;
        Advance();

        if (current_char == '=')
        {
            Advance();
            return (
                new Token<TokenNoValueType>(TokenType.NE, startPos: posStart, endPos: pos),
                ErrorNull.Instance
            );
        }
        Advance();
        return (
            new Token<TokenNoValueType>(TokenType.NULL),
            new ExpectedCharError(posStart, "Expected '=' after '!'")
        );
    }

    private Token<TokenNoValueType> MakeDecicion(
        char checkChar,
        TokenType TTTrue,
        TokenType TTFalse
    )
    {
        Position posStart = pos;
        Advance();

        if (current_char == checkChar)
        {
            Advance();
            return new Token<TokenNoValueType>(TTTrue, startPos: posStart, endPos: pos);
        }
        return new Token<TokenNoValueType>(TTFalse, startPos: posStart, endPos: pos);
    }

    private Token<string> MakeString()
    {
        string value = string.Empty;
        Position startPos = pos;
        bool escapeThisChar = false;

        Dictionary<char, char> escapeChars = new() { { 'n', '\n' }, { 't', '\t' } };

        Advance();
        while (current_char != char.MaxValue && (current_char != '"') || escapeThisChar)
        {
            if (escapeThisChar)
            {
                if (!escapeChars.TryGetValue(current_char, out char _char))
                {
                    _char = current_char; // if null
                }

                value += _char;
                escapeThisChar = false;
            }
            else
            {
                if (current_char == '\\')
                {
                    escapeThisChar = true;
                }
                else
                {
                    value += current_char;
                }
            }
            Advance();
        }
        Advance();
        return new Token<string>(TokenType.STRING, value, startPos, pos);
    }

    private void SkipComent()
    {
        Advance();
        while (current_char is not '\n' and not '\r' and not char.MaxValue)
        {
            Advance();
        }
        Advance();
    }

    private Token<TokenNoValueType> SkipTypeAnotation()
    {
        Advance();
        while (current_char is not '=' and not char.MaxValue)
        {
            Advance();
        }
        Advance();
        return new Token<TokenNoValueType>(TokenType.EQ, pos, Position.Null);
    }

    private Token<TokenNoValueType> MakePlus()
    {
        Position startPos = pos;
        Advance();

        if (current_char == '+')
        { // it is ++
            Advance();
            return new Token<TokenNoValueType>(TokenType.PP, startPos, pos);
        }
        if (current_char == '=')
        { // its +=
            Advance();
            return new Token<TokenNoValueType>(TokenType.PLEQ, startPos, pos);
        }
        return new Token<TokenNoValueType>(TokenType.PLUS, startPos, pos);
    }

    private Token<TokenNoValueType> MakeMinus()
    {
        Position startPos = pos;
        Advance();

        if (current_char == '-')
        { // it is --
            Advance();
            return new Token<TokenNoValueType>(TokenType.MM, startPos, pos);
        }
        if (current_char == '=')
        { // its -=
            Advance();
            return new Token<TokenNoValueType>(TokenType.MIEQ, startPos, pos);
        }
        if (current_char == '>')
        {
            Advance();
            return new Token<TokenNoValueType>(TokenType.ARROW, startPos, pos);
        }
        return new Token<TokenNoValueType>(TokenType.MINUS, startPos, pos);
    }

    // generates all tokens
    public (List<IToken>, Error) MakeTokens()
    {
        List<IToken> tokens = [];
        int parenthesesCount = 0; // to check if there are unclosed parentesies
        int squarBracketCount = 0; // to check if there are unclosed square brackets

        while (current_char != char.MaxValue)
        {
            if (current_char is ' ' or '\t')
            {
                // Discards spaces and tabs
                Advance();
            }
            else if (current_char == '#')
            {
                SkipComent();
            }
            else if (current_char == ':')
            {
                tokens.Add(SkipTypeAnotation());
            }
            else if (current_char is ';' or '\n' or '\r')
            {
                tokens.Add(new Token<TokenNoValueType>(TokenType.NEWLINE, pos, Position.Null));
                Advance();
            }
            else if (current_char == '.')
            {
                tokens.Add(new Token<TokenNoValueType>(TokenType.DOT, pos, Position.Null));
                Advance();
            }
            else if (char.IsDigit(current_char)) // Check for digits (int)
            {
                (Token<double>, Error) res = MakeNumber();
                if (res.Item2.IsError)
                {
                    return ([], res.Item2);
                }
                tokens.Add(res.Item1);
            }
            else if (char.IsLetter(current_char)) // Check for letters
            {
                tokens.Add(MakeIdentifier());
            }
            else if (current_char == '"')
            {
                tokens.Add(MakeString());
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
            }
            else if (current_char == '/')
            {
                tokens.Add(MakeDecicion('=', TokenType.DIEQ, TokenType.DIV));
            }
            else if (current_char == '^')
            {
                tokens.Add(new Token<TokenNoValueType>(TokenType.POW, pos, Position.Null));
                Advance();
            }
            else if (current_char == '(')
            {
                parenthesesCount++;
                tokens.Add(new Token<TokenNoValueType>(TokenType.LPAREN, pos, Position.Null));
                Advance();
            }
            else if (current_char == ')')
            {
                parenthesesCount--;
                tokens.Add(new Token<TokenNoValueType>(TokenType.RPAREN, pos, Position.Null));
                Advance();
            }
            // Comparison
            else if (current_char == '!')
            {
                (Token<TokenNoValueType>, Error) res = MakeNotEquals();
                if (res.Item2.IsError)
                {
                    return ([], res.Item2);
                }
                tokens.Add(res.Item1);
            }
            else if (current_char == '=')
            {
                tokens.Add(MakeDecicion('=', TokenType.EE, TokenType.EQ));
            }
            else if (current_char == '<')
            {
                tokens.Add(MakeDecicion('=', TokenType.LTE, TokenType.LT));
            }
            else if (current_char == '>')
            {
                tokens.Add(MakeDecicion('=', TokenType.GTE, TokenType.GT));
            }
            // Other
            else if (current_char == ',')
            {
                tokens.Add(new Token<TokenNoValueType>(TokenType.COMMA, pos, Position.Null));
                Advance();
            }
            else if (current_char == '[')
            {
                squarBracketCount++;
                tokens.Add(new Token<TokenNoValueType>(TokenType.LSQUARE, pos, Position.Null));
                Advance();
            }
            else if (current_char == ']')
            {
                squarBracketCount--;
                tokens.Add(new Token<TokenNoValueType>(TokenType.RSQUARE, pos, Position.Null));
                Advance();
            }
            else
            {
                // Not a valid token, return an error
                char invalidChar = current_char;
                Position posStart = pos;
                Advance();
                return (
                    new List<IToken>(),
                    new IllegalCharError(posStart, "Invalid char: " + invalidChar.ToString())
                );
            }
        }
        if (parenthesesCount != 0)
        {
            return (
                [],
                new UnclosedBracketsError(
                    Position.Null,
                    "There are more or less Open parentheses than closed ones"
                )
            );
        }
        if (squarBracketCount != 0)
        {
            return (
                [],
                new UnclosedBracketsError(
                    Position.Null,
                    "There are more or less Open square brackets than closed ones"
                )
            );
        }

        tokens.Add(new Token<TokenNoValueType>(TokenType.EOF, pos, Position.Null)); // Add the End Of File token
        return (tokens, ErrorNull.Instance);
    }
}
