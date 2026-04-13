using YSharp.Common;

namespace YSharp.Runtime.Primitives.Datetime;

public sealed partial class VDateTime
{
    public override Result<Value, Error> AddedTo(Value other)
    {
        Value? val = other switch
        {
            VDateTime dateOther => AddToDate(this, dateOther),
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
            VDateTime dateOther => SubToDate(this, dateOther),
            _ => null,
        };
        if (val is null)
            return base.AddedTo(other);

        val.SetContext(Context);
        return Result<Value, Error>.Success(val);
    }
}
