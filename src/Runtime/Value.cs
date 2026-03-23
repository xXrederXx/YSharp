using YSharp.Common;

namespace YSharp.Runtime;

public class Value
{
    public Context? Context { protected set; get; }
    public Position EndPos { protected set; get; }
    public Position StartPos { protected set; get; }

    // arethmetic
    public virtual Result<Value, Error> AddedTo(Value other) =>
        IlligalOperation(other);

    // other
    public virtual Result<Value, Error> AndedTo(Value other) =>
        IlligalOperation(other);

    public virtual Value Copy() => throw new NotImplementedException("No copy method defined");

    public virtual Result<Value, Error> DivedTo(Value other) =>
        IlligalOperation(other);

    // comparison
    public virtual Result<Value, Error> GetComparisonEQ(Value other) =>
        IlligalOperation(other);

    public virtual Result<Value, Error> GetComparisonGT(Value other) =>
        IlligalOperation(other);

    public virtual Result<Value, Error> GetComparisonGTE(Value other) =>
        IlligalOperation(other);

    public virtual Result<Value, Error> GetComparisonLT(Value other) =>
        IlligalOperation(other);

    public virtual Result<Value, Error> GetComparisonLTE(Value other) =>
        IlligalOperation(other);

    public virtual Result<Value, Error> GetComparisonNE(Value other) =>
        IlligalOperation(other);

    public virtual Result<Value, Error> GetFunc(string name, List<Value> argNodes) =>
        Result<Value, Error>.Fail(new FuncNotFoundError(Position.Null, name, Context));

    public virtual Result<Value, Error> GetVar(string name) =>
        Result<Value, Error>.Fail( new VarNotFoundError(Position.Null, name, Context));

    public virtual bool IsTrue() => false;

    public virtual Result<Value, Error> MuledTo(Value other) =>
        IlligalOperation(other);

    public virtual Result<Value, Error> Notted() => IlligalOperation();

    public virtual Result<Value, Error> OredTo(Value other) =>
        IlligalOperation(other);

    public virtual Result<Value, Error> PowedTo(Value other) =>
        IlligalOperation(other);

    public Value SetContext(Context? context)
    {
        this.Context = context;
        return this;
    }

    public Value SetPos(in Position startPos, in Position endPos)
    {
        // set the positions
        this.StartPos = startPos;
        this.EndPos = endPos;
        return this;
    }

    public virtual Result<Value, Error> SubedTo(Value other) =>
       IlligalOperation(other);

    protected Result<Value, Error> IlligalOperation(Value? other = null)
    {
        string details = "Illigal Operation";
        if (other == null)
            details += " on self";
        else
            details += $" between {GetType()} and {other.GetType()}";

        return Result<Value, Error>.Fail( new IlligalOperationError(StartPos, details, Context));
    }
}

public class ValueNull : Value
{
    public static readonly ValueNull Instance = new();

    private ValueNull() { }
}
