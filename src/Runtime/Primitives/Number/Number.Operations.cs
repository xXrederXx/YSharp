using YSharp.Common;
using YSharp.Runtime.Primatives.Bool;

namespace YSharp.Runtime.Primatives.Number;

public sealed partial class VNumber : Value
{
    public override ValueAndError AddedTo(Value other)
    {
        Value? val = other switch
        {
            VNumber numOther => AddToNum(this, numOther),
            _ => null,
        };
        if (val is null)
            return base.AddedTo(other);

        val.SetContext(Context);
        return (val, ErrorNull.Instance);
    }

    public override ValueAndError DivedTo(Value other)
    {
        return other switch
        {
            VNumber numOther => DivToNum(this, numOther),
            _ => base.DivedTo(other),
        };
    }

    public override ValueAndError GetComparisonEQ(Value other)
    {
        if (other is VNumber _other)
            return (new VBool(value == _other.value), ErrorNull.Instance);
        return base.GetComparisonEQ(other);
    }

    public override ValueAndError GetComparisonGT(Value other)
    {
        if (other is VNumber _other)
            return (new VBool(value > _other.value), ErrorNull.Instance);
        return base.GetComparisonGT(other);
    }

    public override ValueAndError GetComparisonGTE(Value other)
    {
        if (other is VNumber _other)
            return (new VBool(value >= _other.value), ErrorNull.Instance);
        return base.GetComparisonGTE(other);
    }

    public override ValueAndError GetComparisonLT(Value other)
    {
        if (other is VNumber _other)
            return (new VBool(value < _other.value), ErrorNull.Instance);
        return base.GetComparisonLT(other);
    }

    public override ValueAndError GetComparisonLTE(Value other)
    {
        if (other is VNumber _other)
            return (new VBool(value <= _other.value), ErrorNull.Instance);
        return base.GetComparisonLTE(other);
    }

    public override ValueAndError GetComparisonNE(Value other)
    {
        if (other is VNumber _other)
            return (new VBool(value != _other.value), ErrorNull.Instance);
        return base.GetComparisonNE(other);
    }

    public override ValueAndError MuledTo(Value other)
    {
        Value? val = other switch
        {
            VNumber numOther => MulToNum(this, numOther),
            _ => null,
        };
        if (val is null)
            return base.MuledTo(other);

        val.SetContext(Context);
        return (val, ErrorNull.Instance);
    }

    public override ValueAndError PowedTo(Value other)
    {
        Value? val = other switch
        {
            VNumber numOther => PowToNum(this, numOther),
            _ => null,
        };
        if (val is null)
            return base.AddedTo(other);

        val.SetContext(Context);
        return (val, ErrorNull.Instance);
    }

    public override ValueAndError SubedTo(Value other)
    {
        Value? val = other switch
        {
            VNumber numOther => SubToNum(this, numOther),
            _ => null,
        };
        if (val is null)
            return base.SubedTo(other);

        val.SetContext(Context);
        return (val, ErrorNull.Instance);
    }
}
