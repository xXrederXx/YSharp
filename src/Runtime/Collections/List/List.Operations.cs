using YSharp.Common;
using YSharp.Runtime.Primatives.Bool;
using YSharp.Runtime.Primatives.Number;

namespace YSharp.Runtime.Collections.List;


public sealed partial class VList : Value
{
    public override Result<Value, Error> AddedTo(Value other)
    {
        if (other is VList _other)
        {
            value.AddRange(_other.value);
            return Result<Value, Error>.Success(new VList(value));
        }

        return IlligalOperation(other);
    }

    public override Result<Value, Error> GetComparisonEQ(Value other)
    {
        if (other is VList _other) return Result<Value, Error>.Success(new VBool(value == _other.value));

        return IlligalOperation(other);
    }

    public override Result<Value, Error> GetComparisonNE(Value other)
    {
        if (other is VList _other) return Result<Value, Error>.Success(new VBool(value != _other.value));

        return IlligalOperation(other);
    }

    public override Result<Value, Error> MuledTo(Value other)
    {
        if (other is VNumber _other)
        {
            List<Value> startElments = [.. value];
            if (_other.value < 2)
                return Result<Value, Error>.Success(_other.value < 1 ? new VList([]) : new VList(value)); // * 1 = original * 0 or less = []

            for (int i = 2; i <= _other.value; i++) value.AddRange(startElments);

            return Result<Value, Error>.Success(new VList(value));
        }

        return IlligalOperation(other);
    }
}
