using YSharp.Common;
using YSharp.Runtime.Primatives.Bool;

namespace YSharp.Runtime.Primatives.Number;

public sealed partial class VNumber : Value
{
    public override Result<Value, Error> AddedTo(Value other)
    {
        Value? val = other switch
        {
            VNumber numOther => AddToNum(this, numOther),
            _ => null,
        };
        if (val is null)
            return base.AddedTo(other);

        val.SetContext(Context);
        return Result<Value, Error>.Success(val);
    }

    public override Result<Value, Error> DivedTo(Value other)
    {
        return other switch
        {
            VNumber numOther => DivToNum(this, numOther),
            _ => base.DivedTo(other),
        };
    }

    public override Result<Value, Error> GetComparisonEQ(Value other)
    {
        if (other is VNumber _other)
            return Result<Value, Error>.Success(new VBool(value == _other.value));
        return base.GetComparisonEQ(other);
    }

    public override Result<Value, Error> GetComparisonGT(Value other)
    {
        if (other is VNumber _other)
            return Result<Value, Error>.Success(new VBool(value > _other.value));
        return base.GetComparisonGT(other);
    }

    public override Result<Value, Error> GetComparisonGTE(Value other)
    {
        if (other is VNumber _other)
            return Result<Value, Error>.Success(new VBool(value >= _other.value));
        return base.GetComparisonGTE(other);
    }

    public override Result<Value, Error> GetComparisonLT(Value other)
    {
        if (other is VNumber _other)
            return Result<Value, Error>.Success(new VBool(value < _other.value));
        return base.GetComparisonLT(other);
    }

    public override Result<Value, Error> GetComparisonLTE(Value other)
    {
        if (other is VNumber _other)
            return Result<Value, Error>.Success(new VBool(value <= _other.value));
        return base.GetComparisonLTE(other);
    }

    public override Result<Value, Error> GetComparisonNE(Value other)
    {
        if (other is VNumber _other)
            return Result<Value, Error>.Success(new VBool(value != _other.value));
        return base.GetComparisonNE(other);
    }

    public override Result<Value, Error> MuledTo(Value other)
    {
        Value? val = other switch
        {
            VNumber numOther => MulToNum(this, numOther),
            _ => null,
        };
        if (val is null)
            return base.MuledTo(other);

        val.SetContext(Context);
        return Result<Value, Error>.Success(val);
    }

    public override Result<Value, Error> PowedTo(Value other)
    {
        Value? val = other switch
        {
            VNumber numOther => PowToNum(this, numOther),
            _ => null,
        };
        if (val is null)
            return base.AddedTo(other);

        val.SetContext(Context);
        return Result<Value, Error>.Success(val);
    }

    public override Result<Value, Error> SubedTo(Value other)
    {
        Value? val = other switch
        {
            VNumber numOther => SubToNum(this, numOther),
            _ => null,
        };
        if (val is null)
            return base.SubedTo(other);

        val.SetContext(Context);
        return Result<Value, Error>.Success(val);
    }
}
