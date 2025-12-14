namespace YSharp.Lexer;


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