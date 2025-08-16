using YSharp.Types.Common;

namespace YSharp.Types.Interpreter.Primitives;

public sealed partial class VDateTime
{
    public override ValueAndError AddedTo(Value other)
    {
        Value? val = other switch
        {
            VDateTime dateOther => AddToDate(this, dateOther),
            _ => null,
        };
        if (val is null)
            return base.AddedTo(other);

        val.SetContext(context);
        return (val, ErrorNull.Instance);
    }

    public override ValueAndError SubedTo(Value other)
    {
        Value? val = other switch
        {
            VDateTime dateOther => SubToDate(this, dateOther),
            _ => null,
        };
        if (val is null)
            return base.AddedTo(other);

        val.SetContext(context);
        return (val, ErrorNull.Instance);
    }
}
