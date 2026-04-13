using YSharp.Common;
using YSharp.Runtime.Primitives.Bool;
using YSharp.Runtime.Primitives.Number;

namespace YSharp.Runtime.Primitives.String;

public sealed partial class VString
{
    public override Result<Value, Error> AddedTo(Value other)
    {
        Value? val = other switch
        {
            VString strOther => AddToString(this, strOther),
            _ => null,
        };
        if (val is null)
            return base.AddedTo(other);

        val.SetContext(Context);
        return Result<Value, Error>.Success(val);
    }

    public override Result<Value, Error> GetComparisonEQ(Value other)
    {
        if (other is VString _other)
            return Result<Value, Error>.Success(new VBool(value == _other.value));
        return base.GetComparisonEQ(other);
    }

    public override Result<Value, Error> GetComparisonNE(Value other)
    {
        if (other is VString _other)
            return Result<Value, Error>.Success(new VBool(value != _other.value));
        return base.GetComparisonNE(other);
    }

    public override Result<Value, Error> MuledTo(Value other)
    {
        Value? val = other switch
        {
            VNumber strOther => MulToNum(this, strOther),
            _ => null,
        };
        if (val is null)
            return base.MuledTo(other);

        val.SetContext(Context);
        return Result<Value, Error>.Success(val);
    }
}
