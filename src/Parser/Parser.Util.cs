using System.Runtime.CompilerServices;
using YSharp.Common;
using YSharp.Lexer;
using YSharp.Parser.Nodes;

namespace YSharp.Parser;

public partial class Parser
{
    public static bool TryCastToken<T>(
        BaseToken token,
        out Token<T> result,
        out InternalTokenCastError<T> error,
        [CallerMemberName] string membername = "NoMemberName"
    )
    {
        if (token is Token<T> casted)
        {
            result = casted;
            error = null!;

            return true;
        }

        result = null!;
        error = new InternalTokenCastError<T>(token, membername);

        return false;
    }

    public static bool TryCastTokenNoValue(
        BaseToken token,
        out BaseToken result,
        out InternalTokenCastError<BaseToken> error,
        [CallerMemberName] string membername = "NoMemberName"
    )
    {
        if (token is BaseToken casted)
        {
            result = casted;
            error = null!;

            return true;
        }

        result = null!;
        error = new InternalTokenCastError<BaseToken>(token, membername);

        return false;
    }

    public BaseNode GetBodyNode(ParseResult res)
    {
        if (currentToken.IsType(TokenType.NEWLINE))
        {
            AdvanceParser(res);
            return res.Register(Statements());
        }

        return res.Register(Statement());
    }

    public bool HasErrorButEnd(ParseResult res)
    {
        if (res.HasError && res.Error is not EndKeywordError)
            return true;
        res.ResetError();

        if (IsCurrentTokenNotKeyword(KeywordType.END))
        {
            res.Failure(new ExpectedKeywordError(currentToken.StartPos, "END"));
            return true;
        }

        AdvanceParser(res);
        return false;
    }

    private List<BaseNode> MakeArgs(ParseResult res)
    {
        List<BaseNode> argNodes = [];
        if (currentToken.IsNotType(TokenType.LPAREN))
        {
            res.Failure(new ExpectedTokenError(currentToken.StartPos, "'('"));
            return argNodes;
        }

        AdvanceParser(res);
        if (currentToken.IsType(TokenType.RPAREN))
        {
            // Means its empty
            AdvanceParser(res);
            return argNodes;
        }

        Reverse(1);
        do
        {
            AdvanceParser(res);

            argNodes.Add(res.Register(Expression()));
            if (res.HasError)
                return [];
        } while (currentToken.IsType(TokenType.COMMA));

        if (currentToken.IsNotType(TokenType.RPAREN))
        {
            res.Failure(new UnmatchedBracketError(currentToken.StartPos, ')'));
            return [];
        }

        AdvanceParser(res);
        return argNodes;
    }

    private ParseResult ShortendVarAssignHelper(Token<string> varName, TokenType type)
    {
        ParseResult res = new();

        AdvanceParser(res);
        BaseNode expr = res.Register(Expression()); // this gets the "value" of the variable
        if (res.HasError)
            return res;

        // This converts varName += Expr to varName = varName + Expr
        BaseNode converted = new BinOpNode(
            new VarAccessNode(varName),
            new BaseToken(type, expr.StartPos, expr.StartPos),
            expr
        );
        return res.Success(new VarAssignNode(varName, converted));
    }

    private void SkipNewLines(ParseResult res)
    {
        while (currentToken.IsType(TokenType.NEWLINE))
            AdvanceParser(res);
    }

    private bool IsCurrentTokenKeyword(KeywordType keyword)
    {
        return currentToken is Token<KeywordType> keywordTok && keywordTok.ValueEquals(keyword);
    }

    private bool IsCurrentTokenNotKeyword(KeywordType keyword)
    {
        return !IsCurrentTokenKeyword(keyword);
    }
}
