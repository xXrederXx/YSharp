using YSharp.Common;

namespace YSharp.Runtime;

public class Value
{
    public Context? Context { protected set; get; }
    public Position EndPos { protected set; get; }
    public Position StartPos { protected set; get; }

    // arithmetic
    public virtual Result<Value, Error> AddedTo(Value other) =>
        IllegalOperation(other);

    // other
    public virtual Result<Value, Error> AndedTo(Value other) =>
        IllegalOperation(other);

    public virtual Value Copy() => throw new NotImplementedException("No copy method defined");

    public virtual Result<Value, Error> DivedTo(Value other) =>
        IllegalOperation(other);

    // comparison
    public virtual Result<Value, Error> GetComparisonEQ(Value other) =>
        IllegalOperation(other);

    public virtual Result<Value, Error> GetComparisonGT(Value other) =>
        IllegalOperation(other);

    public virtual Result<Value, Error> GetComparisonGTE(Value other) =>
        IllegalOperation(other);

    public virtual Result<Value, Error> GetComparisonLT(Value other) =>
        IllegalOperation(other);

    public virtual Result<Value, Error> GetComparisonLTE(Value other) =>
        IllegalOperation(other);

    public virtual Result<Value, Error> GetComparisonNE(Value other) =>
        IllegalOperation(other);

    public virtual Result<Value, Error> GetFunc(string name, ReadOnlySpan<Value> argNodes) =>
        Result<Value, Error>.Fail(new FuncNotFoundError(Position.Null, name, Context));

    public virtual Result<Value, Error> GetVar(string name) =>
        Result<Value, Error>.Fail(new VarNotFoundError(Position.Null, name, Context));

    public virtual bool IsTrue() => false;

    public virtual Result<Value, Error> MuledTo(Value other) =>
        IllegalOperation(other);

    public virtual Result<Value, Error> Notted() => IllegalOperation();

    public virtual Result<Value, Error> OredTo(Value other) =>
        IllegalOperation(other);

    public virtual Result<Value, Error> PowedTo(Value other) =>
        IllegalOperation(other);

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
       IllegalOperation(other);

    protected Result<Value, Error> IllegalOperation(Value? other = null)
    {
        string details = "Illegal Operation";
        if (other == null)
            details += " on self";
        else
            details += $" between {GetType()} and {other.GetType()}";

        return Result<Value, Error>.Fail(new IllegalOperationError(StartPos, details, Context));
    }
}

public class ValueNull : Value
{
    public static readonly ValueNull Instance = new();

    private ValueNull() { }
}
