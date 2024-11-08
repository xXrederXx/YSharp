
using System.Diagnostics;

namespace YSharp_2._0;

// this keeps the result of the parser
public class ParseResult
{
    public Error Error { get; private set; } = ErrorNull.Instance;
    public INode Node { get; private set; } = NodeNull.Instance;
    public (List<IfExpresionCases>, ElseCaseData) NodeTupleNN { get; private set; }
    private int _advanceCount = 0;
    public int ToReverseCount { get; private set; } = 0;
    
    // Check if an error exists
    public bool HasError => Error.IsError;

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
        if (result.HasError)
        {
            Error = result.Error;
        }
        return result.Node;
    }

    // Register tuple result and accumulate advances
    public (List<IfExpresionCases>, ElseCaseData) RegisterTuple(ParseResult result)
    {
        _advanceCount += result._advanceCount;
        if (result.HasError)
        {
            Error = result.Error;
        }
        return result.NodeTupleNN;
    }

    // Register an advancement count increment
    public void Register(int advance)
    {
        _advanceCount += advance;
    }

    // Return successful result with Node
    public ParseResult Success(INode node)
    {
        Node = node;
        return this;
    }

    // Return successful result with tuple
    public ParseResult Success((List<IfExpresionCases>, ElseCaseData) nodeTuple)
    {
        NodeTupleNN = nodeTuple;
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
    private readonly List<IToken> tokens;
    private int tokIndex = -1;
    private IToken currentToken;

    // initalizer
    public Parser(List<IToken> tokens)
    {
        this.tokens = tokens;
        currentToken = new Token<TokenNoValueType>(TokenType.NULL);
        Advance();
    }
    // goes to the next token
    private int Advance()
    {
        tokIndex++;
        UpdateCurrentTok();
        return 0; // tutorial sayd wrap in advance so it needs to return something
    }

    private IToken Reverse(int amount = 1)
    {
        tokIndex -= amount;
        UpdateCurrentTok();
        return currentToken;
    }

    private void UpdateCurrentTok()
    {
        if (tokIndex >= 0 && tokIndex < tokens.Count)
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
    private ParseResult IfExprCases(string keyword)
    {
        ParseResult res = new();
        List<IfExpresionCases> cases = [];// (condition, expression, should return null)
        ElseCaseData elseCase = ElseCaseData._null;

        if (!currentToken.Matches(TokenType.KEYWORD, keyword))
        {
            return res.Failure(new ExpectedKeywordError(currentToken.StartPos, $"Expected {keyword}"));
        }

        res.Register(Advance());
        INode condition = res.Register(Statement());
        if (res.HasError)
        {
            return res;
        }

        if (!currentToken.Matches(TokenType.KEYWORD, "THEN"))
        {
            return res.Failure(new ExpectedKeywordError(currentToken.StartPos, $"Expected THEN"));
        }

        res.Register(Advance());
        if (currentToken.Type == TokenType.NEWLINE)
        {
            res.Register(Advance());

            INode _statements = res.Register(Statements());
            if (res.HasError)
            {
                return res;
            }
            cases.Add(new(condition, _statements, true));

            if (currentToken.Matches(TokenType.KEYWORD, "END"))
            {
                res.Register(Advance());
            }
            else
            {
                (List<IfExpresionCases>, ElseCaseData) allCases = res.RegisterTuple(IfExprBorC());
                if (res.HasError)
                {
                    return res;
                }
                elseCase = allCases.Item2;
                cases.AddRange(allCases.Item1);
            }
        }
        else
        {
            INode expr = res.Register(Statement());
            if (res.HasError)
            {
                return res;
            }
            cases.Add(new(condition, expr, false));
            (List<IfExpresionCases>, ElseCaseData) allCases = res.RegisterTuple(IfExprBorC());
            if (res.HasError)
            {
                return res;
            }
            elseCase = allCases.Item2;
            cases.AddRange(allCases.Item1);
        }
        return res.Success((cases, elseCase));
    }
    private ParseResult IfExprB()
    {
        return IfExprCases("ELIF");
    }
    private ParseResult IfExprC()
    { // Tuple
        ParseResult res = new();
        ElseCaseData elseCase = ElseCaseData._null;

        if (currentToken.Matches(TokenType.KEYWORD, "ELSE"))
        {
            res.Register(Advance());

            if (currentToken.Type == TokenType.NEWLINE)
            {
                res.Register(Advance());
                INode _statements = res.Register(Statements());
                if (res.HasError)
                {
                    return res;
                }
                elseCase = new(_statements, true);

                if (currentToken.Matches(TokenType.KEYWORD, "END"))
                {
                    res.Register(Advance());
                }
                else
                {
                    return res.Failure(new ExpectedKeywordError(currentToken.StartPos, "Expected END"));
                }
            }
            else
            {
                INode expr = res.Register(Statement());
                if (res.HasError)
                {
                    return res;
                }
                elseCase = new(expr, false);
            }
        }
        return res.Success((new List<IfExpresionCases>(), elseCase));
    }
    private ParseResult IfExprBorC()
    {
        ParseResult res = new();
        List<IfExpresionCases> cases = [];
        ElseCaseData elseCase;
        if (currentToken.Matches(TokenType.KEYWORD, "ELIF"))
        {
            (List<IfExpresionCases>, ElseCaseData) allCases = res.RegisterTuple(IfExprB());
            if (res.HasError)
            {
                return res;
            }
            cases = allCases.Item1;
            elseCase = allCases.Item2;
        }
        else
        {
            elseCase = res.RegisterTuple(IfExprC()).Item2;
            if (res.HasError)
            {
                return res;
            }
        }
        return res.Success((cases, elseCase));
    }
    private ParseResult IfExpr()
    {
        ParseResult res = new();
        (List<IfExpresionCases>, ElseCaseData) allCases = res.RegisterTuple(IfExprCases("IF"));
        if (res.HasError)
        {
            return res;
        }
        return res.Success(new IfNode(allCases.Item1, allCases.Item2));
    }
    private ParseResult ForExpr()
    {
        ParseResult res = new();

        if (!currentToken.Matches(TokenType.KEYWORD, "FOR"))
        {
            return res.Failure(new ExpectedKeywordError(currentToken.StartPos, "Expected FOR"));
        }

        res.Register(Advance());

        if (currentToken.Type != TokenType.IDENTIFIER)
        {
            return res.Failure(new ExpectedTokenError(currentToken.StartPos, "Expected identifier"));
        }

        Token<string> varName = (Token<string>)currentToken;
        res.Register(Advance());

        if (currentToken.Type != TokenType.EQ)
        {
            return res.Failure(new ExpectedTokenError(currentToken.StartPos, "Expected '='"));
        }

        res.Register(Advance());

        INode startValue = res.Register(Expression());
        if (res.HasError)
        {
            return res;
        }

        if (!currentToken.Matches(TokenType.KEYWORD, "TO"))
        {
            return res.Failure(new ExpectedKeywordError(currentToken.StartPos, "Expected TO"));
        }

        res.Register(Advance());
        INode endValue = res.Register(Expression());
        if (res.HasError)
        {
            return res;
        }
        INode? stepValue = null;
        if (currentToken.Matches(TokenType.KEYWORD, "STEP"))
        {
            res.Register(Advance());

            stepValue = res.Register(Expression());
            if (res.HasError)
            {
                return res;
            }
        }

        if (!currentToken.Matches(TokenType.KEYWORD, "THEN"))
        {
            return res.Failure(new ExpectedKeywordError(currentToken.StartPos, "Expected THEN"));
        }

        res.Register(Advance());

        if (currentToken.Type == TokenType.NEWLINE)
        {
            res.Register(Advance());
            INode _body = res.Register(Statements());
            if (res.HasError)
            {
                return res;
            }

            if (!currentToken.Matches(TokenType.KEYWORD, "END"))
            {
                return res.Failure(new ExpectedKeywordError(currentToken.StartPos, "Expected END"));
            }
            res.Register(Advance());

            return res.Success(new ForNode(varName, startValue, endValue, stepValue, _body, false));
        }

        INode body = res.Register(Statement());
        if (res.HasError)
        {
            return res;
        }

        return res.Success(new ForNode(varName, startValue, endValue, stepValue, body, false));
    }
    private ParseResult WhileExpr()
    {
        ParseResult res = new();

        if (!currentToken.Matches(TokenType.KEYWORD, "WHILE"))
        {
            return res.Failure(new ExpectedKeywordError(currentToken.StartPos, "Expected WHILE"));
        }
        res.Register(Advance());

        INode condition = res.Register(Expression());
        if (res.HasError)
        {
            return res;
        }

        if (!currentToken.Matches(TokenType.KEYWORD, "THEN"))
        {
            return res.Failure(new ExpectedKeywordError(currentToken.StartPos, "Expected THEN"));
        }

        res.Register(Advance());

        if (currentToken.Type == TokenType.NEWLINE)
        {
            res.Register(Advance());
            INode _body = res.Register(Statements());
            if (res.HasError)
            {
                return res;
            }

            if (!currentToken.Matches(TokenType.KEYWORD, "END"))
            {
                return res.Failure(new ExpectedKeywordError(currentToken.StartPos, "Expected END"));
            }
            res.Register(Advance());

            return res.Success(new WhileNode(condition, _body, false));
        }

        INode body = res.Register(Statement());
        if (res.HasError)
        {
            return res;
        }

        return res.Success(new WhileNode(condition, body, true));
    }
    private ParseResult ListExpr()
    {
        ParseResult res = new();
        List<INode> elementNodes = [];
        Position StartPos = currentToken.StartPos;

        if (currentToken.Type != TokenType.LSQUARE)
        {
            return res.Failure(new ExpectedTokenError(StartPos, "Expected '['"));
        }

        res.Register(Advance());

        if (currentToken.Type == TokenType.RSQUARE)
        {
            res.Register(Advance());
        }
        else
        {
            elementNodes.Add(res.Register(Expression()));
            if (res.HasError)
            {
                return res.Failure(new InvalidSyntaxError(currentToken.StartPos, "expected 'VAR', 'IF', 'WHILE', 'FUN', int, float, identifier, '+', '-', ']'"));
            }

            while (currentToken.Type == TokenType.COMMA)
            {
                res.Register(Advance());

                elementNodes.Add(res.Register(Expression()));
                if (res.HasError)
                {
                    return res;
                }
            }

            if (currentToken.Type != TokenType.RSQUARE)
            {
                return res.Failure(new ExpectedTokenError(currentToken.StartPos, "expected ',' or ']'"));
            }

            res.Register(Advance());
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

        res.Register(Advance());
        if (currentToken.Type != TokenType.DOT) // normal identifier
        {
            return res.Success(new VarAccessNode((Token<string>)tok));
        }

        // there is a dot notaited identifier
        res.Register(Advance());
        if (currentToken.Type != TokenType.IDENTIFIER)
        {
            return res.Failure(new ExpectedTokenError(currentToken.StartPos, "Expected IDENTIFIER"));
        }

        Token<string> varName = (Token<string>)currentToken;

        res.Register(Advance());
        if (currentToken.Type != TokenType.LPAREN) // it is a variable
        {
            return res.Success(new DotVarAccessNode(varName, new VarAccessNode((Token<string>)tok)));
        }

        // it is a function
        (ParseResult, List<INode>) args = MakeArgs();
        res.Register(args.Item1);
        if (res.HasError)
        {
            return res;
        }
        return res.Success(new DotCallNode(varName, args.Item2, new VarAccessNode((Token<string>)tok)));
    }
    private ParseResult FuncDef()
    {
        ParseResult res = new();
        if (!currentToken.Matches(TokenType.KEYWORD, "FUN"))
        {
            return res.Failure(new ExpectedKeywordError(currentToken.StartPos, "expected FUN"));
        }

        res.Register(Advance());

        Token<string> varNameTok;
        if (currentToken.Type == TokenType.IDENTIFIER)
        {
            varNameTok = (Token<string>)currentToken;
            res.Register(Advance());
            if (currentToken.Type != TokenType.LPAREN)
            {
                return res.Failure(new ExpectedTokenError(currentToken.StartPos, "expected '('"));
            }
        }
        else
        {
            varNameTok = new Token<string>(TokenType.NULL);
            if (currentToken.Type != TokenType.LPAREN)
            {
                return res.Failure(new ExpectedTokenError(currentToken.StartPos, "expected identifier or '('"));
            }
        }

        res.Register(Advance());
        List<IToken> argNameTok = [];

        if (currentToken.Type == TokenType.IDENTIFIER)
        { // has args
            argNameTok.Add(currentToken);
            res.Register(Advance());

            while (currentToken.Type == TokenType.COMMA)
            {
                res.Register(Advance());

                if (currentToken.Type != TokenType.IDENTIFIER)
                {
                    return res.Failure(new ExpectedTokenError(currentToken.StartPos, "expected identifier"));
                }
                argNameTok.Add(currentToken);
                res.Register(Advance());
            }

            if (currentToken.Type != TokenType.RPAREN)
            {
                return res.Failure(new ExpectedTokenError(currentToken.StartPos, "expected ',' or ')'"));
            }
        }
        else
        {
            if (currentToken.Type != TokenType.RPAREN)
            {
                return res.Failure(new ExpectedTokenError(currentToken.StartPos, "expected identifier or ')'"));
            }
        }

        res.Register(Advance());

        if (currentToken.Type == TokenType.ARROW)
        {
            res.Register(Advance());

            INode nodeToReturn = res.Register(Expression());
            if (res.HasError)
            {
                return res;
            }
            return res.Success(new FuncDefNode(varNameTok, argNameTok, nodeToReturn, true));
        }

        if (currentToken.Type != TokenType.NEWLINE)
        {
            return res.Failure(new ExpectedTokenError(currentToken.StartPos, "expected '->' or newline"));
        }

        res.Register(Advance());
        INode body = res.Register(Statements());
        if (res.HasError)
        {
            return res;
        }
        if (!currentToken.Matches(TokenType.KEYWORD, "END"))
        {
            return res.Failure(new ExpectedKeywordError(currentToken.StartPos, "expected END"));
        }
        res.Register(Advance());

        return res.Success(new FuncDefNode(varNameTok, argNameTok, body, false));
    }
    private (ParseResult, List<INode>) MakeArgs()
    {
        ParseResult res = new();
        List<INode> argNodes = [];

        if (currentToken.Type != TokenType.LPAREN)
        {
            return (res.Failure(new ExpectedTokenError(currentToken.StartPos, "expected a '('")), []);
        }

        res.Register(Advance());
        if (currentToken.Type == TokenType.RPAREN)
        {
            res.Register(Advance());
            return (res.Success(NodeNull.Instance), argNodes); // empty node just for the parseresult.succses
        }

        // get argument
        argNodes.Add(res.Register(Expression()));
        if (res.HasError)
        {
            return (res.Failure(new InvalidSyntaxError(currentToken.StartPos, "expected 'VAR', 'IF', 'WHILE', 'FUN', int, float, identifier, '+', '-', '(', ')', '['")), []);
        }

        // get the rest of the arguments which are seperated by commas
        while (currentToken.Type == TokenType.COMMA)
        {
            res.Register(Advance());

            argNodes.Add(res.Register(Expression()));
            if (res.HasError)
            {
                return (res, []);
            }
        }

        if (currentToken.Type != TokenType.RPAREN)
        {
            return (res.Failure(new ExpectedTokenError(currentToken.StartPos, "expected a ')' or a ','")), []);
        }

        res.Register(Advance());

        return (res.Success(NodeNull.Instance), argNodes); // empty node just for the parseresult.succses
    }
    private ParseResult ShortendVarAssignHelper(Token<string> varName, TokenType type){
        ParseResult res = new();

        res.Register(Advance());
        INode expr = res.Register(Expression()); // this gets the "value" of the variable
        if (res.HasError)
        {
            return res;
        }

        // This converts varName += Expr to varName = varName + Expr 
        INode converted = new BinOpNode(new VarAccessNode(varName), new Token<TokenNoValueType>(type, expr.StartPos, expr.StartPos), expr);
        return res.Success(new VarAssignNode(varName, converted));
    }
    private ParseResult VarAssignExpr()
    {
        ParseResult res = new();

        if (!currentToken.Matches(TokenType.KEYWORD, "VAR"))
        {
            return res.Failure(new ExpectedKeywordError(currentToken.StartPos, "expected VAR"));
        }

        res.Register(Advance());
        if (currentToken.Type != TokenType.IDENTIFIER)
        {
            return res.Failure(new InvalidSyntaxError(currentToken.StartPos, "Expected Identifier"));
        }

        Token<string> varName = (Token<string>)currentToken;
        INode expr;

        res.Register(Advance());

        // look if it is a shorthand written way else use null
        TokenType? EqType = currentToken.Type switch
        {
            TokenType.PLEQ => (TokenType?)TokenType.PLUS,
            TokenType.MIEQ => (TokenType?)TokenType.MINUS,
            TokenType.MUEQ => (TokenType?)TokenType.MUL,
            TokenType.DIEQ => (TokenType?)TokenType.DIV,
            _ => null,
        };
        if(EqType is not null){
            INode converted = res.Register(ShortendVarAssignHelper(varName, (TokenType)EqType));
            if (res.HasError)
            {
                return res;
            }
            return res.Success(new VarAssignNode(varName, converted));
        }

        // look if it is increment or decrement
        if(currentToken.Type == TokenType.PP){
            return res.Success(
                new VarAssignNode(
                    varName,
                    new BinOpNode(
                        new VarAccessNode(varName),
                        new Token<TokenNoValueType>(TokenType.PLUS),
                        new NumberNode(new Token<double>(TokenType.INT, 1, Position.Null, Position.Null))
                    )
                )
            );
        }
        if(currentToken.Type == TokenType.MM){
            return res.Success(
                new VarAssignNode(
                    varName,
                    new BinOpNode(
                        new VarAccessNode(varName),
                        new Token<TokenNoValueType>(TokenType.MINUS),
                        new NumberNode(new Token<double>(TokenType.INT, 1, Position.Null, Position.Null))
                    )
                )
            );
        }

        // normal variable assign
        if (currentToken.Type != TokenType.EQ)
        {
            return res.Failure(new InvalidSyntaxError(currentToken.StartPos, "Expected ="));
        }

        res.Register(Advance());
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

        if (!currentToken.Matches(TokenType.KEYWORD, "TRY"))
        {
            return res.Failure(new ExpectedKeywordError(currentToken.StartPos, $"Expected TRY"));
        }
        res.Register(Advance());

        INode tryBlock = res.Register(Statements());
        INode catchBlock = NodeNull.Instance;
        Token<string> varName = new Token<string>(TokenType.NULL);
        if (res.HasError)
        {
            return res;
        }

        if (!currentToken.Matches(TokenType.KEYWORD, "END"))
        {
            res.Failure(new ExpectedKeywordError(currentToken.StartPos, "Expected END"));
        }
        res.Register(Advance());

        while(currentToken.Type == TokenType.NEWLINE){
            res.Register(Advance());
        }

        if (currentToken.Matches(TokenType.KEYWORD, "CATCH"))
        {
            res.Register(Advance());

            if(currentToken.Type == TokenType.IDENTIFIER){
                varName = (Token<string>)currentToken;
                res.Register(Advance());
            }

            catchBlock = res.Register(Statements());
            if (res.HasError)
            {
                return res;
            }

            if (!currentToken.Matches(TokenType.KEYWORD, "END"))
            {
                res.Failure(new ExpectedKeywordError(currentToken.StartPos, "Expected END"));
            }
            res.Register(Advance());
        }

        return res.Success(new TryCatchNode(tryBlock, catchBlock, varName));
    }

    // Important Methods Start at bottom to top
    private ParseResult Atom()
    {
        ParseResult res = new();

        IToken tok = currentToken;

        // check tyoes
        if (tok.Type is TokenType.INT or TokenType.FLOAT)
        {
            res.Register(Advance());
            return res.Success(new NumberNode((Token<double>)tok));
        }
        else if (tok.Type == TokenType.STRING)
        {
            res.Register(Advance());
            return res.Success(new StringNode((Token<string>)tok));
        }

        // check identifier
        else if (tok.Type == TokenType.IDENTIFIER)
        {
            INode identifierExpr = res.Register(IdentifierExpr());
            if (res.HasError)
            {
                return res;
            }
            return res.Success(identifierExpr);
        }

        // check other symbols
        else if (tok.Type == TokenType.LPAREN)
        {
            res.Register(Advance());
            INode expr = res.Register(Expression());
            if (res.HasError)
            {
                return res;
            }

            if (currentToken.Type == TokenType.RPAREN)
            {
                res.Register(Advance());
                return res.Success(expr);
            }
            return res.Failure(new InvalidSyntaxError(currentToken.StartPos, "Expected ')'"));
        }
        else if (tok.Type == TokenType.LSQUARE)
        {
            INode listExpression = res.Register(ListExpr());
            if (res.HasError)
            {
                return res;
            }
            return res.Success(listExpression);
        }

        // check keywords
        else if (tok.Matches(TokenType.KEYWORD, "IF"))
        {
            INode ifExpression = res.Register(IfExpr());
            if (res.HasError)
            {
                return res;
            }
            return res.Success(ifExpression);
        }
        else if (tok.Matches(TokenType.KEYWORD, "FOR"))
        {
            INode forExpression = res.Register(ForExpr());
            if (res.HasError)
            {
                return res;
            }
            return res.Success(forExpression);
        }
        else if (tok.Matches(TokenType.KEYWORD, "WHILE"))
        {
            INode whileExpression = res.Register(WhileExpr());
            if (res.HasError)
            {
                return res;
            }
            return res.Success(whileExpression);
        }
        else if (tok.Matches(TokenType.KEYWORD, "FUN"))
        {
            INode funcExpression = res.Register(FuncDef());
            if (res.HasError)
            {
                return res;
            }
            return res.Success(funcExpression);
        }
        else if (tok.Matches(TokenType.KEYWORD, "TRY"))
        {
            INode tryCatch = res.Register(TryCatchExpr());
            if (res.HasError) {
                return res;
            }
            return res.Success(tryCatch);
        }
        return res.Failure(new InvalidSyntaxError(tok.StartPos, "expected int, float, identifier, IF, FOR, WHILE, FUN, '(' or '['"));
    }
    private ParseResult Call()
    {
        ParseResult res = new();

        INode atom = res.Register(Atom());
        if (res.HasError)
        {
            return res;
        }

        if (currentToken.Type == TokenType.LPAREN)
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

        INode? left = res.Register(Call());
        if (res.HasError)
        {
            return res;
        }

        while (currentToken.Type == TokenType.POW)
        {
            Token<TokenNoValueType> opTok = (Token<TokenNoValueType>)currentToken;
            res.Register(Advance());

            INode? right = res.Register(Factor());
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
        if (tok.Type is TokenType.PLUS or TokenType.MINUS)
        {
            res.Register(Advance());
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

        INode? left = res.Register(Factor());
        if (res.HasError)
        {
            return res;
        }

        // point before line
        while (currentToken.Type is TokenType.MUL or TokenType.DIV)
        {
            Token<TokenNoValueType> opTok = (Token<TokenNoValueType>)currentToken;

            res.Register(Advance());
            INode? right = res.Register(Factor());
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

        INode? left = res.Register(Term());
        if (res.HasError)
        {
            return res;
        }

        while (currentToken.Type is TokenType.PLUS or TokenType.MINUS)
        { // punkt vor strich
            Token<TokenNoValueType> opTok = (Token<TokenNoValueType>)currentToken;

            res.Register(Advance());
            INode? right = res.Register(Term());
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

        if (currentToken.Matches(TokenType.KEYWORD, "NOT"))
        {
            IToken opTok = currentToken;

            res.Register(Advance());
            INode node = res.Register(CompExpr());
            if (res.HasError)
            {
                return res;
            }
            return res.Success(new UnaryOpNode(opTok, node));
        }

        // Binary Operation
        INode? left = res.Register(ArithExpr());
        if (res.HasError)
        {
            return res.Failure(new InvalidSyntaxError(left.StartPos, "expected 'NOT', int, float, identifier, '+', '-', '(', '['"));
        }

        // This checks for the comparison operators
        while (currentToken.Type is TokenType.EE or TokenType.NE or TokenType.GT or TokenType.LT or TokenType.GTE or TokenType.LTE)
        {
            Token<TokenNoValueType> opTok = (Token<TokenNoValueType>)currentToken;

            res.Register(Advance());
            INode? right = res.Register(ArithExpr());
            if (res.HasError)
            {
                return res.Failure(new InvalidSyntaxError(right.StartPos, "expected 'NOT', int, float, identifier, '+', '-', '(', '['"));
            }

            left = new BinOpNode(left, opTok, right);
        }
        return res.Success(left);
    }
    private ParseResult Expression()
    {
        ParseResult res = new();
        // A variable assignement
        if (currentToken.Matches(TokenType.KEYWORD, "VAR"))
        {
            INode node = res.Register(VarAssignExpr());
            if (res.HasError)
            {
                return res;
            }

            return res.Success(node);
        }
        // Binary Operation
        INode? left = res.Register(CompExpr());
        if (res.HasError)
        {
            return res.Failure(new InvalidSyntaxError(left.StartPos, "expected 'VAR', 'IF', 'WHILE', 'FUN', int, float, identifier, '+', '-', '(', '['"));
        }

        while (currentToken.Matches(TokenType.KEYWORD, "AND") || currentToken.Matches(TokenType.KEYWORD, "OR"))
        {
            IToken opTok = currentToken;

            res.Register(Advance());
            INode? right = res.Register(CompExpr());
            if (res.HasError)
            {
                return res.Failure(new InvalidSyntaxError(right.StartPos, "expected 'VAR', int, float, identifier, '+', '-', '(', '['"));
            }

            left = new BinOpNode(left, opTok, right);
        }
        return res.Success(left);
    }
    private ParseResult Statement()
    {
        ParseResult res = new();
        if (currentToken.StartPos.IsNull)
        {
            return res.Failure(new InternalError("start pos is null"));
        }
        Position posStart = currentToken.StartPos;

        if (currentToken.Matches(TokenType.KEYWORD, "RETURN"))
        {
            res.Register(Advance());
            INode? _expr = res.TryRegister(Expression());
            if (_expr == null)
            {
                Reverse(res.ToReverseCount);
            }
            return res.Success(new ReturnNode(_expr, posStart, currentToken.StartPos));
        }
        if (currentToken.Matches(TokenType.KEYWORD, "CONTINUE"))
        {
            res.Register(Advance());
            return res.Success(new ContinueNode(posStart, currentToken.StartPos));
        }
        if (currentToken.Matches(TokenType.KEYWORD, "BREAK"))
        {
            res.Register(Advance());
            return res.Success(new BreakNode(posStart, currentToken.StartPos));
        }

        INode expr = res.Register(Expression());

        if (res.HasError)
        {
            return res.Failure(new InvalidSyntaxError(currentToken.StartPos, "expected 'RETURN', 'CONTINUE', 'BREAK', 'VAR', 'IF', 'WHILE', 'FUN', int, float, identifier, '+', '-', '(', '['"));
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

        while (currentToken.Type == TokenType.NEWLINE) // skip all new lines
        {
            res.Register(Advance());
        }

        INode? currentStatement = res.Register(Statement());
        if (res.HasError)
        {
            return res;
        }

        AllStatements.Add(currentStatement);

        while (true) // repeat until no more lines are available
        {
            bool moreStatements = false;
            while (currentToken.Type == TokenType.NEWLINE) // skip all new Lines
            {
                res.Register(Advance());
                moreStatements = true;
            }
            if (!moreStatements) // if no new line was detected break out of the loop
            {
                break;
            }

            currentStatement = res.TryRegister(Statement());
            if (res.HasError)
            {
                return res;
            }
            if (currentStatement == null)
            {
                Reverse(res.ToReverseCount);
                break;
            }
            AllStatements.Add(currentStatement);
        }
        if (currentToken.EndPos.IsNull)
        {
            return res.Success(new ListNode(AllStatements, StartPos, StartPos));
        }
        return res.Success(new ListNode(AllStatements, StartPos, currentToken.EndPos));
    }
}

