using System.Collections.Immutable;
using YSharp.Types;
using YSharp.Types.InternalTypes;

namespace YSharp.Internal;

// this keeps the result of the parser
public class ParseResult
{
    public Error Error { get; private set; } = ErrorNull.Instance;
    public INode Node { get; private set; } = NodeNull.Instance;
    private int _advanceCount = 0;
    public int ToReverseCount { get; private set; } = 0;

    // Check if an error exists
    public bool HasError => Error.IsError;

    public void ResetError()
    {
        Error = ErrorNull.Instance;
    }

    // Try to register the result; if error is present, mark reversal
    public INode? TryRegister(ParseResult result)
    {
        if (result.HasError)
        {
            ToReverseCount = result._advanceCount;
            return null;
        }
        return Register(result);
    }

    // Register result and accumulate advances
    public INode Register(ParseResult result)
    {
        _advanceCount += result._advanceCount;
        Error = result.Error;
        return result.Node;
    }

    public bool SafeRegrister(ParseResult result, out INode node)
    {
        node = Register(result);
        return result.HasError;
    }

    // Register an advancement count increment
    public void Advance()
    {
        _advanceCount++;
    }

    // Return successful result with Node
    public ParseResult Success(INode node)
    {
        Node = node;
        return this;
    }

    // Return failure result and update error if not already set
    public ParseResult Failure(Error error)
    {
        if (!HasError || _advanceCount == 0)
        {
            Error = error;
        }
        //* For testing -> Console.WriteLine(error.ToString() + Node.ToString());
        return this;
    }

    public override string ToString()
    {
        return Node.ToString() ?? "null";
    }
}

// the parser which is used to make the abstract syntax tree
public class Parser
{
    private readonly ImmutableArray<IToken> tokens;
    public int tokIndex = -1;
    public IToken currentToken;

    // initalizer
    public Parser(List<IToken> tokens)
    {
        this.tokens = tokens.ToImmutableArray();
        currentToken = new Token<TokenNoValueType>(TokenType.NULL);
        AdvanceParser();
    }

    // goes to the next token
    private void AdvanceParser()
    {
        tokIndex++;
        UpdateCurrentTok();
    }

    private void AdvanceParser(ParseResult res)
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

    // main function which parses all tokens
    public ParseResult Parse()
    {
        return Statements();
    }

    // helper methods

    private ParseResult IfExpr()
    {
        ParseResult res = new();
        List<SubIfNode> cases = [];
        INode elseCase = NodeNull.Instance;

        if (currentToken.IsNotMatching(TokenType.KEYWORD, KeywordType.IF))
        {
            return res.Failure(new ExpectedKeywordError(currentToken.StartPos, $"Expected IF"));
        }

        while (
            currentToken.IsMatching(TokenType.KEYWORD, KeywordType.IF)
            || currentToken.IsMatching(TokenType.KEYWORD, KeywordType.ELIF)
        )
        {
            res.Advance();
            AdvanceParser();

            INode caseCondition = res.Register(Statement());
            if (res.HasError)
            {
                return res;
            }

            if (currentToken.IsNotMatching(TokenType.KEYWORD, KeywordType.THEN))
            {
                return res.Failure(
                    new ExpectedKeywordError(currentToken.StartPos, $"Expected THEN")
                );
            }

            res.Advance();
            AdvanceParser();

            if (currentToken.IsNotType(TokenType.NEWLINE))
            {
                return res.Failure(
                    new ExpectedCharError(currentToken.StartPos, "Newline expected")
                );
            }

            res.Advance();
            AdvanceParser();

            INode caseBodyNode = res.Register(Statements());
            if (res.HasError && res.Error is not EndKeywordError)
            {
                return res;
            }
            res.ResetError();

            cases.Add(new SubIfNode(caseCondition, caseBodyNode));

            if (currentToken.IsMatching(TokenType.KEYWORD, KeywordType.END))
            {
                res.Advance();
                AdvanceParser();
                return res.Success(new IfNode(cases, elseCase));
            }
        }

        if (currentToken.IsNotMatching(TokenType.KEYWORD, KeywordType.ELSE))
        {
            return res.Failure(
                new ExpectedKeywordError(currentToken.StartPos, "Expected END or ELSE")
            );
        }

        res.Advance();
        AdvanceParser();

        if (currentToken.IsNotType(TokenType.NEWLINE))
        {
            return res.Failure(new ExpectedCharError(currentToken.StartPos, "Newline expected"));
        }

        res.Advance();
        AdvanceParser();

        elseCase = res.Register(Statements());
        if (res.HasError && res.Error is not EndKeywordError)
        {
            return res;
        }
        res.ResetError();
        if (currentToken.IsNotMatching(TokenType.KEYWORD, KeywordType.END))
        {
            return res.Failure(new ExpectedKeywordError(currentToken.StartPos, "Expected END"));
        }

        res.Advance();
        AdvanceParser();

        return res.Success(new IfNode(cases, elseCase));
    }

    private ParseResult ForExpr()
    {
        ParseResult res = new();

        if (currentToken.IsNotMatching(TokenType.KEYWORD, KeywordType.FOR))
        {
            return res.Failure(new ExpectedKeywordError(currentToken.StartPos, "Expected FOR"));
        }

        res.Advance();
        AdvanceParser();

        if (currentToken.IsNotType(TokenType.IDENTIFIER))
        {
            return res.Failure(
                new ExpectedTokenError(currentToken.StartPos, "Expected identifier")
            );
        }

        Token<string> varName = (Token<string>)currentToken;
        res.Advance();
        AdvanceParser();

        if (currentToken.IsNotType(TokenType.EQ))
        {
            return res.Failure(new ExpectedTokenError(currentToken.StartPos, "Expected '='"));
        }

        res.Advance();
        AdvanceParser();

        INode startValue = res.Register(Expression());
        if (res.HasError)
        {
            return res;
        }

        if (currentToken.IsNotMatching(TokenType.KEYWORD, KeywordType.TO))
        {
            return res.Failure(new ExpectedKeywordError(currentToken.StartPos, "Expected TO"));
        }

        res.Advance();
        AdvanceParser();
        INode endValue = res.Register(Expression());
        if (res.HasError)
        {
            return res;
        }
        INode? stepValue = null;
        if (currentToken.IsMatching(TokenType.KEYWORD, KeywordType.STEP))
        {
            res.Advance();
            AdvanceParser();

            stepValue = res.Register(Expression());
            if (res.HasError)
            {
                return res;
            }
        }

        if (currentToken.IsNotMatching(TokenType.KEYWORD, KeywordType.THEN))
        {
            return res.Failure(new ExpectedKeywordError(currentToken.StartPos, "Expected THEN"));
        }

        res.Advance();
        AdvanceParser();

        if (currentToken.IsType(TokenType.NEWLINE))
        {
            res.Advance();
            AdvanceParser();
            INode _body = res.Register(Statements());
            if (res.HasError && res.Error is not EndKeywordError)
            {
                return res;
            }
            res.ResetError();
            if (currentToken.IsNotMatching(TokenType.KEYWORD, KeywordType.END))
            {
                return res.Failure(new ExpectedKeywordError(currentToken.StartPos, "Expected END"));
            }
            res.Advance();
            AdvanceParser();

            return res.Success(new ForNode(varName, startValue, endValue, stepValue, _body, false));
        }

        INode body = res.Register(Statement());
        if (res.HasError && res.Error is not EndKeywordError)
        {
            return res;
        }
        res.ResetError();
        if (currentToken.IsNotMatching(TokenType.KEYWORD, KeywordType.END))
        {
            return res.Failure(new ExpectedKeywordError(currentToken.StartPos, "Expected END"));
        }
        res.Advance();
        AdvanceParser();
        return res.Success(new ForNode(varName, startValue, endValue, stepValue, body, false));
    }

    private ParseResult WhileExpr()
    {
        ParseResult res = new();

        if (currentToken.IsNotMatching(TokenType.KEYWORD, KeywordType.WHILE))
        {
            return res.Failure(new ExpectedKeywordError(currentToken.StartPos, "Expected WHILE"));
        }
        res.Advance();
        AdvanceParser();

        INode condition = res.Register(Expression());
        if (res.HasError)
        {
            return res;
        }

        if (currentToken.IsNotMatching(TokenType.KEYWORD, KeywordType.THEN))
        {
            return res.Failure(new ExpectedKeywordError(currentToken.StartPos, "Expected THEN"));
        }

        res.Advance();
        AdvanceParser();

        if (currentToken.IsType(TokenType.NEWLINE))
        {
            res.Advance();
            AdvanceParser();
            INode _body = res.Register(Statements());
            if (res.HasError && res.Error is not EndKeywordError)
            {
                return res;
            }
            res.ResetError();
            if (currentToken.IsNotMatching(TokenType.KEYWORD, KeywordType.END))
            {
                return res.Failure(new ExpectedKeywordError(currentToken.StartPos, "Expected END"));
            }
            res.Advance();
            AdvanceParser();

            return res.Success(new WhileNode(condition, _body, false));
        }

        INode body = res.Register(Statement());
        if (res.HasError && res.Error is not EndKeywordError)
        {
            return res;
        }
        res.ResetError();
        if (currentToken.IsNotMatching(TokenType.KEYWORD, KeywordType.END))
        {
            return res.Failure(new ExpectedKeywordError(currentToken.StartPos, "Expected END"));
        }
        res.Advance();
        AdvanceParser();

        return res.Success(new WhileNode(condition, body, true));
    }

    private ParseResult ListExpr()
    {
        ParseResult res = new();
        List<INode> elementNodes = [];
        Position StartPos = currentToken.StartPos;

        if (currentToken.IsNotType(TokenType.LSQUARE))
        {
            return res.Failure(new ExpectedTokenError(StartPos, "Expected '['"));
        }

        res.Advance();
        AdvanceParser();

        if (currentToken.IsType(TokenType.RSQUARE))
        {
            res.Advance();
            AdvanceParser();
        }
        else
        {
            elementNodes.Add(res.Register(Expression()));
            if (res.HasError)
            {
                return res.Failure(
                    new InvalidSyntaxError(
                        currentToken.StartPos,
                        "expected 'VAR', 'IF', 'WHILE', 'FUN', int, float, identifier, '+', '-', ']'"
                    )
                );
            }

            while (currentToken.IsType(TokenType.COMMA))
            {
                res.Advance();
                AdvanceParser();

                elementNodes.Add(res.Register(Expression()));
                if (res.HasError)
                {
                    return res;
                }
            }

            if (currentToken.IsNotType(TokenType.RSQUARE))
            {
                return res.Failure(
                    new ExpectedTokenError(currentToken.StartPos, "expected ',' or ']'")
                );
            }

            res.Advance();
            AdvanceParser();
        }
        if (!currentToken.EndPos.IsNull)
        {
            return res.Success(new ListNode(elementNodes, StartPos, currentToken.EndPos));
        }
        return res.Success(new ListNode(elementNodes, StartPos, StartPos));
    }

    private ParseResult IdentifierExpr()
    {
        ParseResult res = new();
        IToken tok = currentToken;

        res.Advance();
        AdvanceParser();
        if (currentToken.IsNotType(TokenType.DOT)) // normal identifier
        {
            return res.Success(new VarAccessNode((Token<string>)tok));
        }

        // there is a dot notaited identifier
        res.Advance();
        AdvanceParser();
        if (currentToken.IsNotType(TokenType.IDENTIFIER))
        {
            return res.Failure(
                new ExpectedTokenError(currentToken.StartPos, "Expected IDENTIFIER")
            );
        }

        Token<string> varName = (Token<string>)currentToken;

        res.Advance();
        AdvanceParser();
        if (currentToken.IsNotType(TokenType.LPAREN)) // it is a variable
        {
            return res.Success(
                new DotVarAccessNode(varName, new VarAccessNode((Token<string>)tok))
            );
        }

        // it is a function
        (ParseResult, List<INode>) args = MakeArgs();
        res.Register(args.Item1);
        if (res.HasError)
        {
            return res;
        }
        return res.Success(
            new DotCallNode(varName, args.Item2, new VarAccessNode((Token<string>)tok))
        );
    }

    private ParseResult FuncDef()
    {
        ParseResult res = new();
        if (currentToken.IsNotMatching(TokenType.KEYWORD, KeywordType.FUN))
        {
            return res.Failure(new ExpectedKeywordError(currentToken.StartPos, "expected FUN"));
        }

        res.Advance();
        AdvanceParser();

        Token<string> varNameTok;
        if (currentToken.IsType(TokenType.IDENTIFIER))
        {
            varNameTok = (Token<string>)currentToken;
            res.Advance();
            AdvanceParser();
            if (currentToken.IsNotType(TokenType.LPAREN))
            {
                return res.Failure(new ExpectedTokenError(currentToken.StartPos, "expected '('"));
            }
        }
        else
        {
            varNameTok = new Token<string>(TokenType.NULL);
            if (currentToken.IsNotType(TokenType.LPAREN))
            {
                return res.Failure(
                    new ExpectedTokenError(currentToken.StartPos, "expected identifier or '('")
                );
            }
        }

        res.Advance();
        AdvanceParser();
        List<IToken> argNameTok = [];

        if (currentToken.IsType(TokenType.IDENTIFIER))
        { // has args
            argNameTok.Add(currentToken);
            res.Advance();
            AdvanceParser();

            while (currentToken.IsType(TokenType.COMMA))
            {
                res.Advance();
                AdvanceParser();

                if (currentToken.IsNotType(TokenType.IDENTIFIER))
                {
                    return res.Failure(
                        new ExpectedTokenError(currentToken.StartPos, "expected identifier")
                    );
                }
                argNameTok.Add(currentToken);
                res.Advance();
                AdvanceParser();
            }

            if (currentToken.IsNotType(TokenType.RPAREN))
            {
                return res.Failure(
                    new ExpectedTokenError(currentToken.StartPos, "expected ',' or ')'")
                );
            }
        }
        else
        {
            if (currentToken.IsNotType(TokenType.RPAREN))
            {
                return res.Failure(
                    new ExpectedTokenError(currentToken.StartPos, "expected identifier or ')'")
                );
            }
        }

        res.Advance();
        AdvanceParser();

        if (currentToken.IsType(TokenType.ARROW))
        {
            res.Advance();
            AdvanceParser();

            INode nodeToReturn = res.Register(Expression());
            if (res.HasError)
            {
                return res;
            }
            return res.Success(new FuncDefNode(varNameTok, argNameTok, nodeToReturn, true));
        }

        if (currentToken.IsNotType(TokenType.NEWLINE))
        {
            return res.Failure(
                new ExpectedTokenError(currentToken.StartPos, "expected '->' or newline")
            );
        }

        res.Advance();
        AdvanceParser();
        INode body = res.Register(Statements());
        if (res.HasError && res.Error is not EndKeywordError)
        {
            return res;
        }
        res.ResetError();
        if (currentToken.IsNotMatching(TokenType.KEYWORD, KeywordType.END))
        {
            return res.Failure(new ExpectedKeywordError(currentToken.StartPos, "expected END"));
        }
        res.Advance();
        AdvanceParser();
        return res.Success(new FuncDefNode(varNameTok, argNameTok, body, false));
    }

    private (ParseResult, List<INode>) MakeArgs()
    {
        ParseResult res = new();
        List<INode> argNodes = [];

        if (currentToken.IsNotType(TokenType.LPAREN))
        {
            return (
                res.Failure(new ExpectedTokenError(currentToken.StartPos, "expected a '('")),
                []
            );
        }

        res.Advance();
        AdvanceParser();
        if (currentToken.IsType(TokenType.RPAREN))
        {
            res.Advance();
            AdvanceParser();
            return (res.Success(NodeNull.Instance), argNodes); // empty node just for the parseresult.succses
        }

        // get argument
        argNodes.Add(res.Register(Expression()));
        if (res.HasError)
        {
            return (
                res.Failure(
                    new InvalidSyntaxError(
                        currentToken.StartPos,
                        "expected 'VAR', 'IF', 'WHILE', 'FUN', int, float, identifier, '+', '-', '(', ')', '['"
                    )
                ),
                []
            );
        }

        // get the rest of the arguments which are seperated by commas
        while (currentToken.IsType(TokenType.COMMA))
        {
            res.Advance();
            AdvanceParser();

            argNodes.Add(res.Register(Expression()));
            if (res.HasError)
            {
                return (res, []);
            }
        }

        if (currentToken.IsNotType(TokenType.RPAREN))
        {
            return (
                res.Failure(
                    new ExpectedTokenError(currentToken.StartPos, "expected a ')' or a ','")
                ),
                []
            );
        }

        res.Advance();
        AdvanceParser();

        return (res.Success(NodeNull.Instance), argNodes); // empty node just for the parseresult.succses
    }

    private ParseResult ShortendVarAssignHelper(Token<string> varName, TokenType type)
    {
        ParseResult res = new();

        res.Advance();
        AdvanceParser();
        INode expr = res.Register(Expression()); // this gets the "value" of the variable
        if (res.HasError)
        {
            return res;
        }

        // This converts varName += Expr to varName = varName + Expr
        INode converted = new BinOpNode(
            new VarAccessNode(varName),
            new Token<TokenNoValueType>(type, expr.StartPos, expr.StartPos),
            expr
        );
        return res.Success(new VarAssignNode(varName, converted));
    }

    private ParseResult VarAssignExpr()
    {
        ParseResult res = new();

        if (currentToken.IsNotMatching(TokenType.KEYWORD, KeywordType.VAR))
        {
            return res.Failure(new ExpectedKeywordError(currentToken.StartPos, "expected VAR"));
        }

        res.Advance();
        AdvanceParser();
        if (currentToken.IsNotType(TokenType.IDENTIFIER))
        {
            return res.Failure(
                new InvalidSyntaxError(currentToken.StartPos, "Expected Identifier")
            );
        }

        Token<string> varName = (Token<string>)currentToken;
        INode expr;

        res.Advance();
        AdvanceParser();

        // look if it is a shorthand written way else use null
        TokenType? EqType = currentToken.Type switch
        {
            TokenType.PLEQ => (TokenType?)TokenType.PLUS,
            TokenType.MIEQ => (TokenType?)TokenType.MINUS,
            TokenType.MUEQ => (TokenType?)TokenType.MUL,
            TokenType.DIEQ => (TokenType?)TokenType.DIV,
            _ => null,
        };
        if (EqType is not null)
        {
            INode converted = res.Register(ShortendVarAssignHelper(varName, (TokenType)EqType));
            if (res.HasError)
            {
                return res;
            }
            return res.Success(new VarAssignNode(varName, converted));
        }

        // look if it is increment or decrement
        if (currentToken.IsType(TokenType.PP))
        {
            return res.Success(
                new VarAssignNode(
                    varName,
                    new BinOpNode(
                        new VarAccessNode(varName),
                        new Token<TokenNoValueType>(TokenType.PLUS),
                        new NumberNode(
                            new Token<double>(TokenType.INT, 1, Position.Null, Position.Null)
                        )
                    )
                )
            );
        }
        if (currentToken.IsType(TokenType.MM))
        {
            return res.Success(
                new VarAssignNode(
                    varName,
                    new BinOpNode(
                        new VarAccessNode(varName),
                        new Token<TokenNoValueType>(TokenType.MINUS),
                        new NumberNode(
                            new Token<double>(TokenType.INT, 1, Position.Null, Position.Null)
                        )
                    )
                )
            );
        }

        // normal variable assign
        if (currentToken.IsNotType(TokenType.EQ))
        {
            return res.Failure(new InvalidSyntaxError(currentToken.StartPos, "Expected ="));
        }

        res.Advance();
        AdvanceParser();
        expr = res.Register(Expression()); // this gets the "value" of the variable
        if (res.HasError)
        {
            return res;
        }

        return res.Success(new VarAssignNode(varName, expr));
    }

    private ParseResult TryCatchExpr()
    {
        ParseResult res = new();

        if (currentToken.IsNotMatching(TokenType.KEYWORD, KeywordType.TRY))
        {
            return res.Failure(new ExpectedKeywordError(currentToken.StartPos, $"Expected TRY"));
        }
        res.Advance();
        AdvanceParser();

        INode tryBlock = res.Register(Statements());
        INode catchBlock = NodeNull.Instance;
        Token<string> varName = new Token<string>(TokenType.NULL);
        if (res.HasError && res.Error is not EndKeywordError)
        {
            return res;
        }
        res.ResetError();

        if (currentToken.IsNotMatching(TokenType.KEYWORD, KeywordType.END))
        {
            return res.Failure(new ExpectedKeywordError(currentToken.StartPos, "Expected END"));
        }
        res.Advance();
        AdvanceParser();

        while (currentToken.IsType(TokenType.NEWLINE))
        {
            res.Advance();
            AdvanceParser();
        }

        if (currentToken.IsMatching(TokenType.KEYWORD, KeywordType.CATCH))
        {
            res.Advance();
            AdvanceParser();

            if (currentToken.IsType(TokenType.IDENTIFIER))
            {
                varName = (Token<string>)currentToken;
                res.Advance();
                AdvanceParser();
            }

            catchBlock = res.Register(Statements());
            if (res.HasError && res.Error is not EndKeywordError)
            {
                return res;
            }
            res.ResetError();

            if (currentToken.IsNotMatching(TokenType.KEYWORD, KeywordType.END))
            {
                return res.Failure(new ExpectedKeywordError(currentToken.StartPos, "Expected END"));
            }
            res.Advance();
            AdvanceParser();
        }

        return res.Success(new TryCatchNode(tryBlock, catchBlock, varName));
    }

    private ParseResult ImportExpr()
    {
        ParseResult res = new();
        Position startPos = currentToken.StartPos;
        if (currentToken.IsNotMatching(TokenType.KEYWORD, KeywordType.IMPORT))
        {
            return res.Failure(new ExpectedKeywordError(currentToken.StartPos, "Expected IMPORT"));
        }

        res.Advance();
        AdvanceParser();

        if (currentToken.IsNotType(TokenType.STRING))
        {
            return res.Failure(
                new InvalidSyntaxError(
                    currentToken.StartPos,
                    "Expected a string (Path to dll) after IMPORT"
                )
            );
        }

        Token<string> token = (Token<string>)currentToken;
        return res.Success(new ImportNode(token, startPos, currentToken.EndPos));
    }

    // Important Methods Start at bottom to top
    private ParseResult Atom()
    {
        ParseResult res = new();

        IToken tok = currentToken;

        // check tyoes
        if (tok.IsType(TokenType.INT, TokenType.FLOAT))
        {
            res.Advance();
            AdvanceParser();
            return res.Success(new NumberNode((Token<double>)tok));
        }
        else if (tok.IsType(TokenType.STRING))
        {
            res.Advance();
            AdvanceParser();
            return res.Success(new StringNode((Token<string>)tok));
        }
        // check identifier
        else if (tok.IsType(TokenType.IDENTIFIER))
        {
            INode identifierExpr = res.Register(IdentifierExpr());
            if (res.HasError)
            {
                return res;
            }
            return res.Success(identifierExpr);
        }
        // check other symbols
        else if (tok.IsType(TokenType.LPAREN))
        {
            res.Advance();
            AdvanceParser();
            INode expr = res.Register(Expression());
            if (res.HasError)
            {
                return res;
            }

            if (currentToken.IsType(TokenType.RPAREN))
            {
                res.Advance();
                AdvanceParser();
                return res.Success(expr);
            }
            return res.Failure(new InvalidSyntaxError(currentToken.StartPos, "Expected ')'"));
        }
        else if (tok.IsType(TokenType.LSQUARE))
        {
            INode listExpression = res.Register(ListExpr());
            if (res.HasError)
            {
                return res;
            }
            return res.Success(listExpression);
        }
        // check keywords
        else if (tok.IsMatching(TokenType.KEYWORD, KeywordType.IF))
        {
            INode ifExpression = res.Register(IfExpr());
            if (res.HasError)
            {
                return res;
            }
            return res.Success(ifExpression);
        }
        else if (tok.IsMatching(TokenType.KEYWORD, KeywordType.FOR))
        {
            INode forExpression = res.Register(ForExpr());
            if (res.HasError)
            {
                return res;
            }
            return res.Success(forExpression);
        }
        else if (tok.IsMatching(TokenType.KEYWORD, KeywordType.WHILE))
        {
            INode whileExpression = res.Register(WhileExpr());
            if (res.HasError)
            {
                return res;
            }
            return res.Success(whileExpression);
        }
        else if (tok.IsMatching(TokenType.KEYWORD, KeywordType.FUN))
        {
            INode funcExpression = res.Register(FuncDef());
            if (res.HasError)
            {
                return res;
            }
            return res.Success(funcExpression);
        }
        else if (tok.IsMatching(TokenType.KEYWORD, KeywordType.TRY))
        {
            INode tryCatch = res.Register(TryCatchExpr());
            if (res.HasError)
            {
                return res;
            }
            return res.Success(tryCatch);
        }
        else if (tok.IsMatching(TokenType.KEYWORD, KeywordType.IMPORT))
        {
            INode importNode = res.Register(ImportExpr());
            if (res.HasError)
            {
                return res;
            }
            return res.Success(importNode);
        }
        else if (tok.IsMatching(TokenType.KEYWORD, KeywordType.END))
        {
            return res.Failure(new EndKeywordError(currentToken.StartPos));
        }
        return res.Failure(
            new InvalidSyntaxError(
                tok.StartPos,
                $"expected int, float, identifier, IF, FOR, WHILE, FUN, '(' or '[' current token is of type {tok.Type}"
            )
        );
    }

    private ParseResult Call()
    {
        ParseResult res = new();

        INode atom = res.Register(Atom());
        if (res.HasError)
        {
            return res;
        }

        if (currentToken.IsType(TokenType.LPAREN))
        {
            (ParseResult, List<INode>) args = MakeArgs();
            res.Register(args.Item1);
            if (res.HasError)
            {
                return res;
            }
            return res.Success(new CallNode(atom, args.Item2));
        }
        return res.Success(atom);
    }

    private ParseResult Power()
    {
        ParseResult res = new();

        INode left = res.Register(Call());
        if (res.HasError)
        {
            return res;
        }

        while (currentToken.IsType(TokenType.POW))
        {
            Token<TokenNoValueType> opTok = (Token<TokenNoValueType>)currentToken;
            res.Advance();
            AdvanceParser();

            INode right = res.Register(Factor());
            if (res.HasError)
            {
                return res;
            }

            left = new BinOpNode(left, opTok, right);
        }
        return res.Success(left);
    }

    private ParseResult Factor()
    {
        ParseResult res = new();

        IToken tok = currentToken;

        // if there is a plus or a minus it could be +5 or -5
        if (tok.IsType(TokenType.PLUS, TokenType.MINUS))
        {
            res.Advance();
            AdvanceParser();
            INode factor = res.Register(Factor());
            if (res.HasError)
            {
                return res;
            }
            return res.Success(new UnaryOpNode((Token<TokenNoValueType>)tok, factor));
        }

        return Power();
    }

    private ParseResult Term()
    { // will return BinOpNode
        ParseResult res = new();

        INode left = res.Register(Factor());
        if (res.HasError)
        {
            return res;
        }

        // point before line
        while (currentToken.IsType(TokenType.MUL, TokenType.DIV))
        {
            Token<TokenNoValueType> opTok = (Token<TokenNoValueType>)currentToken;

            res.Advance();
            AdvanceParser();
            INode right = res.Register(Factor());
            if (res.HasError)
            {
                return res;
            }

            left = new BinOpNode(left, opTok, right);
        }
        return res.Success(left);
    }

    private ParseResult ArithExpr()
    {
        ParseResult res = new();

        INode left = res.Register(Term());
        if (res.HasError)
        {
            return res;
        }

        while (currentToken.IsType(TokenType.PLUS, TokenType.MINUS))
        { // punkt vor strich
            Token<TokenNoValueType> opTok = (Token<TokenNoValueType>)currentToken;

            res.Advance();
            AdvanceParser();
            INode right = res.Register(Term());
            if (res.HasError)
            {
                return res;
            }

            left = new BinOpNode(left, opTok, right);
        }
        return res.Success(left);
    }

    private ParseResult CompExpr()
    {
        ParseResult res = new();

        if (currentToken.IsMatching(TokenType.KEYWORD, KeywordType.NOT))
        {
            IToken opTok = currentToken;

            res.Advance();
            AdvanceParser();
            INode node = res.Register(CompExpr());
            if (res.HasError)
            {
                return res;
            }
            return res.Success(new UnaryOpNode(opTok, node));
        }

        // Binary Operation
        INode left = res.Register(ArithExpr());
        if (res.HasError)
        {
            return res;
        }

        // This checks for the comparison operators
        while (
            currentToken.IsType(
                TokenType.EE,
                TokenType.NE,
                TokenType.GT,
                TokenType.LT,
                TokenType.GTE,
                TokenType.LTE
            )
        )
        {
            Token<TokenNoValueType> opTok = (Token<TokenNoValueType>)currentToken;

            res.Advance();
            AdvanceParser();
            INode right = res.Register(ArithExpr());
            if (res.HasError)
            {
                return res;
            }

            left = new BinOpNode(left, opTok, right);
        }
        return res.Success(left);
    }

    private ParseResult Expression()
    {
        ParseResult res = new();
        // A variable assignement
        if (currentToken.IsMatching(TokenType.KEYWORD, KeywordType.VAR))
        {
            INode node = res.Register(VarAssignExpr());
            if (res.HasError)
            {
                return res;
            }

            return res.Success(node);
        }
        // Binary Operation
        INode left = res.Register(CompExpr());
        if (res.HasError)
        {
            return res;
        }

        while (
            currentToken.IsMatching(TokenType.KEYWORD, KeywordType.AND)
            || currentToken.IsMatching(TokenType.KEYWORD, KeywordType.OR)
        )
        {
            IToken opTok = currentToken;

            res.Advance();
            AdvanceParser();
            INode right = res.Register(CompExpr());
            if (res.HasError)
            {
                return res;
            }

            left = new BinOpNode(left, opTok, right);
        }
        return res.Success(left);
    }

    private ParseResult Statement()
    {
        ParseResult res = new();
        Position startPos = currentToken.StartPos;

        if (currentToken.IsMatching(TokenType.KEYWORD, KeywordType.RETURN))
        {
            AdvanceParser(res);
            INode? _expr = res.TryRegister(Expression());
            if (_expr == null)
            {
                Reverse(res.ToReverseCount);
            }
            return res.Success(new ReturnNode(_expr, startPos, currentToken.StartPos));
        }
        if (currentToken.IsMatching(TokenType.KEYWORD, KeywordType.CONTINUE))
        {
            AdvanceParser(res);
            return res.Success(new ContinueNode(startPos, currentToken.StartPos));
        }
        if (currentToken.IsMatching(TokenType.KEYWORD, KeywordType.BREAK))
        {
            AdvanceParser(res);
            return res.Success(new BreakNode(startPos, currentToken.StartPos));
        }
        if (currentToken.IsMatching(TokenType.KEYWORD, KeywordType.END))
        {
            return res.Failure(new EndKeywordError(currentToken.StartPos));
        }

        INode expr = res.Register(Expression());

        if (res.HasError)
        {
            return res;
        }

        return res.Success(expr);
    }

    private ParseResult Statements()
    {
        ParseResult res = new();
        if (currentToken.StartPos.IsNull)
        {
            return res.Failure(new InternalError("start pos is null"));
        }

        Position StartPos = currentToken.StartPos;
        List<INode> AllStatements = [];
        INode nextStatement;

        while (currentToken.IsNotType(TokenType.EOF)) // repeat until no more lines are available
        {
            while (currentToken.IsType(TokenType.NEWLINE)) // skip all new Lines
            {
                AdvanceParser(res);
            }
            if (currentToken.IsType(TokenType.EOF))
            {
                break;
            }

            nextStatement = res.Register(Statement());
            if (res.HasError)
            {
                return res;
            }

            AllStatements.Add(nextStatement);
        }
        if (currentToken.EndPos.IsNull)
        {
            return res.Success(new ListNode(AllStatements, StartPos, StartPos));
        }
        return res.Success(new ListNode(AllStatements, StartPos, currentToken.EndPos));
    }
}
