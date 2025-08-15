using System.Runtime.CompilerServices;
using YSharp.Types.AST;
using YSharp.Types.Common;
using YSharp.Types.Lexer;

namespace YSharp.Core;

public partial class Parser
{
    public INode GetBodyNode(ParseResult res)
    {
        if (currentToken.IsType(TokenType.NEWLINE))
        {
            AdvanceParser(res);
            return res.Register(Statements());
        }
        else
        {
            return res.Register(Statement());
        }
    }

    public bool HasErrorButEnd(ParseResult res)
    {
        if (res.HasError && res.Error is not EndKeywordError)
        {
            return true;
        }
        res.ResetError();

        if (currentToken.IsNotMatchingKeyword(KeywordType.END))
        {
            res.Failure(new ExpectedKeywordError(currentToken.StartPos, "END"));
            return true;
        }
        AdvanceParser(res);
        return false;
    }

    public static bool TryCastToken<T>(
        IToken token,
        out Token<T> result,
        out InternalParserError error,
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
        error = new InternalParserError(
            $"Casting the token ({token.GetType()}) to a Token<{typeof(T)}> failed in {membername} / Token: {token}"
        );

        return false;
    }

    private List<INode> MakeArgs(ParseResult res)
    {
        if (currentToken.IsNotType(TokenType.LPAREN))
        {
            res.Failure(new ExpectedTokenError(currentToken.StartPos, "'('"));
            return [];
        }

        AdvanceParser(res);
        if (currentToken.IsType(TokenType.RPAREN))
        {
            AdvanceParser(res);
            return []; // empty node just for the parseresult.succses
        }

        // get argument
        List<INode> argNodes = [];
        argNodes.Add(res.Register(Expression()));
        if (res.HasError)
        {
            return [];
        }

        // get the rest of the arguments which are seperated by commas
        while (currentToken.IsType(TokenType.COMMA))
        {
            AdvanceParser(res);

            argNodes.Add(res.Register(Expression()));
            if (res.HasError)
            {
                return [];
            }
        }

        if (currentToken.IsNotType(TokenType.RPAREN))
        {
            res.Failure(new ExpectedTokenError(currentToken.StartPos, "')' or ','"));
            return [];
        }

        AdvanceParser(res);

        return argNodes; // empty node just for the parseresult.succses
    }

    private void SkipNewLines(ParseResult res)
    {
        while (currentToken.IsType(TokenType.NEWLINE))
        {
            AdvanceParser(res);
        }
    }

    private ParseResult ShortendVarAssignHelper(Token<string> varName, TokenType type)
    {
        ParseResult res = new();

        AdvanceParser(res);
        INode expr = res.Register(Expression()); // this gets the "value" of the variable
        if (res.HasError)
        {
            return res;
        }

        // This converts varName += Expr to varName = varName + Expr
        INode converted = new BinOpNode(
            new VarAccessNode(varName),
            new TokenNoValue(type, expr.StartPos, expr.StartPos),
            expr
        );
        return res.Success(new VarAssignNode(varName, converted));
    }
}
