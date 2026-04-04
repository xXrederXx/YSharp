using YSharp.Common;

namespace YSharp.Runtime.Primatives.Bool;


public sealed partial class VBool : Value
{
    public override Result<Value, Error> AndedTo(Value other)
    {
        if (other is VBool _other) return Result<Value, Error>.Success(new VBool(value && _other.value));
        return base.AndedTo(other);
    }

    public override Result<Value, Error> GetComparisonEQ(Value other)
    {
        if (other is VBool _other) return Result<Value, Error>.Success(new VBool(value == _other.value));
        return base.GetComparisonEQ(other);
    }

    public override Result<Value, Error> GetComparisonNE(Value other)
    {
        if (other is VBool _other) return Result<Value, Error>.Success(new VBool(value != _other.value));
        return base.GetComparisonNE(other);
    }

    public override Result<Value, Error> Notted()
    {
        VBool? ret = new(!value);
        ret.SetContext(Context);
        return Result<Value, Error>.Success(ret);
    }

    public override Result<Value, Error> OredTo(Value other)
    {
        if (other is VBool _other) return Result<Value, Error>.Success(new VBool(value || _other.value));
        return base.OredTo(other);
    }
}
