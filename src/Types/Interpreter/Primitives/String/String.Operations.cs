using YSharp.Types.Common;
using YSharp.Types.Interpreter.Internal;

namespace YSharp.Types.Interpreter.Primitives;

public sealed partial class VString{
    public override ValueAndError AddedTo(Value other)
    {
        Value? val = other switch
        {
            VString strOther => AddToString(this, strOther),
            _ => null
        };
        if (val is null)
            return base.AddedTo(other);

        val.SetContext(context);
        return (val, ErrorNull.Instance);
    }

    public override ValueAndError GetComparisonEQ(Value other)
    {
        if (other is VString _other) return (new VBool(value == _other.value), ErrorNull.Instance);
        return base.GetComparisonEQ(other);
    }

    public override ValueAndError GetComparisonNE(Value other)
    {
        if (other is VString _other) return (new VBool(value != _other.value), ErrorNull.Instance);
        return base.GetComparisonNE(other);
    }

    public override ValueAndError MuledTo(Value other)
    {
        Value? val = other switch
        {
            VNumber strOther => MulToNum(this, strOther),
            _ => null
        };
        if (val is null)
            return base.MuledTo(other);

        val.SetContext(context);
        return (val, ErrorNull.Instance);
    }
}