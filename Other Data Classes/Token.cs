namespace YSharp_2._0
{
    // Token types optimized using enum
    public enum TokenType
    {
        NULL,
        // Types
        INT,
        FLOAT,
        STRING,

        // Arithmetic
        PLUS,
        MINUS,
        MUL,
        DIV,
        POW,
        LPAREN,
        RPAREN,
        EQ,
        
        PP, // ++
        MM, // --
        PLEQ, // +=
        MIEQ, // -=
        MUEQ, // *=
        DIEQ, // /=


        // Comparison
        EE, // ==
        NE, // !=
        LT, // <
        GT, // >
        LTE, // <=
        GTE, // >=

        // Other
        IDENTIFIER,
        KEYWORD,
        COMMA,
        DOT,
        ARROW,
        LSQUARE, // [
        RSQUARE, // ]
        NEWLINE,
        EOF // End of file
    }

    // Keywords optimized with HashSet
    public static class TokenTypeHelper
    {
        private static readonly HashSet<string> Keywords =
        [
            "VAR", "AND", "OR", "NOT", "IF", "THEN", "ELIF", "ELSE",
            "FOR", "TO", "STEP", "WHILE", "FUN", "END", "RETURN", "CONTINUE", "BREAK"
        ];

        public static bool IsKeyword(string s) => Keywords.Contains(s);
    }

    // Token class optimized with enum and nullable reference types
    public class Token
    {
        public readonly TokenType Type;
        public readonly object? Value;
        public readonly Position StartPos;
        public readonly Position EndPos;

        // Constructor
        public Token(TokenType type, object value, Position startPos, Position endPos)
        {
            Type = type;
            Value = value;

            if (!startPos.IsNull)
            {
                StartPos = startPos;
                EndPos = StartPos; // If there is no end Position just assume it to be 1 char
                EndPos.Advance(' ');
            }

            if (!endPos.IsNull)
            {
                EndPos = endPos;
            }
        }
        public Token(TokenType type, Position startPos, Position endPos)
        {
            Type = type;
            Value = "";

            if (!startPos.IsNull)
            {
                StartPos = startPos;
                EndPos = StartPos; // If there is no end Position just assume it to be 1 char
                EndPos.Advance(' ');
            }

            if (!endPos.IsNull)
            {
                EndPos = endPos;
            }
        }
        public Token(TokenType type)
        {
            Type = type;
            Value = "";
        }

        // String Representation
        public override string ToString()
        {
            return Value != null ? $"{Type}:{Value}" : Type.ToString();
        }

        public bool Matches(TokenType type, string value)
        {
            if(this.Value is null){
                return this.Type.Equals(type);
            }
            return Type == type && (Value as string) == value;
        }
    }
}
