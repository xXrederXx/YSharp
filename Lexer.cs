using System.Text;

namespace YSharp_2._0;


public class Lexer
{
    private readonly string text;
    private readonly string fileName;
    private Position pos;
    private char current_char = char.MaxValue;

    // Initalizer
    public Lexer(string text, string fileName)
    {
        this.text = text;
        this.fileName = fileName;
        pos = new Position(-1, 0, -1, fileName, text); // -1 because Advance auto increments
        Advance();
    }

    // this uptades to the next charachter
    private void Advance()
    {
        pos = pos.Advance(current_char);
        current_char = pos.Index < text.Length ? text[pos.Index] : char.MaxValue;
    }

    // this is used to make a number token of type int or float
    private (Token, Error) MakeNumber()
    {
        StringBuilder stringBuilder = new();
        bool hasDot = false;
        Position posStart = pos;

        while (current_char != char.MaxValue && (char.IsDigit(current_char) || current_char == '.' || current_char == '_'))
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
                    return (new(TokenType.NULL), new IllegalCharError(posStart, "You can't have 2 dots in a number")); // cant have 2 dots in a number
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

        if (hasDot)
        {
            return (new Token(TokenType.FLOAT, double.Parse(stringBuilder.ToString()), posStart, pos), NoError.Instance);
        }
        else
        {
            return (new Token(TokenType.INT, int.Parse(stringBuilder.ToString()), posStart, pos), NoError.Instance);
        }
    }
    private Token MakeIdentifier()
    {
        StringBuilder stringBuilder = new();
        Position posStart = pos;

        while (current_char != char.MaxValue && (char.IsLetter(current_char) || char.IsDigit(current_char) || current_char == '_'))
        { // if current char is letter, digit or _
            stringBuilder.Append(current_char);
            Advance();
        }

        string idStr = stringBuilder.ToString();
        if (TokenTypeHelper.IsKeyword(idStr))
        {
            return new Token(TokenType.KEYWORD, idStr, posStart, pos);
        }
        else
        {
            return new Token(TokenType.IDENTIFIER, idStr, posStart, pos);
        }
    }
    private (Token, Error) MakeNotEquals()
    {
        Position posStart = pos;
        Advance();

        if (current_char == '=')
        {
            Advance();
            return (new Token(TokenType.NE, startPos: posStart, endPos: pos), NoError.Instance);
        }
        Advance();
        return (new Token(TokenType.NULL), new ExpectedCharError(posStart, "Expected '=' after '!'"));
    }
    private Token MakeDecicion(char checkChar, TokenType TTTrue, TokenType TTFalse)
    {
        Position posStart = pos;
        Advance();

        if (current_char == checkChar)
        {
            Advance();
            return new Token(TTTrue, startPos: posStart, endPos: pos);
        }
        return new Token(TTFalse, startPos: posStart, endPos: pos);
    }
    private Token MakeString()
    {
        string value = string.Empty;
        Position startPos = pos;
        bool escapeThisChar = false;

        Dictionary<char, char> escapeChars = new()
        {
            { 'n', '\n' },
            { 't', '\t' }
        };


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
        return new Token(TokenType.STRING, value, startPos, pos);
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
    private Token SkipTypeAnotation()
    {
        Advance();
        while (current_char != '=' && current_char != char.MaxValue)
        {
            Advance();
        }
        Advance();
        return new Token(TokenType.EQ, pos, Position._null, Position._null);
    }

    private Token MakePlus(){
        Position startPos = pos;
        Advance();

        if(current_char == '+'){ // it is ++
            Advance();
            return new Token(TokenType.PP, startPos, pos);
        }
        if(current_char == '='){ // its +=
            Advance();
            return new Token(TokenType.PLEQ, startPos, pos);
        }
        return new Token(TokenType.PLUS, startPos, pos);
    }
    private Token MakeMinus(){
        Position startPos = pos;
        Advance();

        if(current_char == '-'){ // it is --
            Advance();
            return new Token(TokenType.MM, startPos, pos);
        }
        if(current_char == '='){ // its -=
            Advance();
            return new Token(TokenType.MIEQ, startPos, pos);
        }
        if (current_char == '>'){
            Advance();
            return new Token(TokenType.ARROW, startPos, pos);
        }
        return new Token(TokenType.MINUS, startPos, pos);
    }
    // generates all tokens
    public (List<Token>, Error) MakeTokens()
    {
        List<Token> tokens = [];
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
                tokens.Add(new Token(TokenType.NEWLINE, string.Empty, pos, Position._null));
                Advance();
            }
            else if (current_char == '.')
            {
                tokens.Add(new Token(TokenType.DOT, string.Empty, pos, Position._null));
                Advance();
            }
            else if (char.IsDigit(current_char)) // Check for digits (int)
            {
                (Token, Error) res = MakeNumber();
                if(res.Item2.IsError){
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
                tokens.Add(new Token(TokenType.POW, string.Empty, pos, Position._null));
                Advance();
            }
            else if (current_char == '(')
            {
                parenthesesCount++;
                tokens.Add(new Token(TokenType.LPAREN, string.Empty, pos, Position._null));
                Advance();
            }
            else if (current_char == ')')
            {
                parenthesesCount--;
                tokens.Add(new Token(TokenType.RPAREN, string.Empty, pos, Position._null));
                Advance();
            }
            // Comparison
            else if (current_char == '!')
            {
                (Token, Error) res = MakeNotEquals();
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
                tokens.Add(new Token(TokenType.COMMA, string.Empty, pos, Position._null));
                Advance();
            }
            else if (current_char == '[')
            {
                squarBracketCount++;
                tokens.Add(new Token(TokenType.LSQUARE, string.Empty, pos, Position._null));
                Advance();
            }
            else if (current_char == ']')
            {
                squarBracketCount--;
                tokens.Add(new Token(TokenType.RSQUARE, string.Empty, pos, Position._null));
                Advance();
            }
            else
            {
                // Not a valid token, return an error
                char invalidChar = current_char;
                Position posStart = pos;
                Advance();
                return (new List<Token>(), new IllegalCharError(posStart, "Invalid char: " + invalidChar.ToString()));
            }
        }
        if(parenthesesCount != 0){
            return ([], new UnclosedBracketsError(Position._null, "There are more or less Open parentheses than closed ones"));
        }
        if(squarBracketCount != 0){
            return ([], new UnclosedBracketsError(Position._null, "There are more or less Open square brackets than closed ones"));
        }

        tokens.Add(new Token(TokenType.EOF, string.Empty, pos, Position._null)); // Add the End Of File token
        return (tokens, NoError.Instance);
    }

}