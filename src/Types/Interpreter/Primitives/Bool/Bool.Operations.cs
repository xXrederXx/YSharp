using YSharp.Types.Common;

namespace YSharp.Types.Interpreter.Primitives;

public sealed partial class VBool : Value
{
    public override ValueAndError GetComparisonEQ(Value other)
    {
        if (other is VBool _other)
        {
            return (new VBool(value == _other.value), ErrorNull.Instance);
        }
        return base.GetComparisonEQ(other);
    }

    public override ValueAndError GetComparisonNE(Value other)
    {
        if (other is VBool _other)
        {
            return (new VBool(value != _other.value), ErrorNull.Instance);
        }
        return base.GetComparisonNE(other);
    }

    public override ValueAndError AndedTo(Value other)
    {
        if (other is VBool _other)
        {
            return (new VBool(value && _other.value), ErrorNull.Instance);
        }
        return base.AndedTo(other);
    }

    public override ValueAndError OredTo(Value other)
    {
        if (other is VBool _other)
        {
            return (new VBool(value || _other.value), ErrorNull.Instance);
        }
        return base.OredTo(other);
    }

    public override ValueAndError Notted()
    {
        VBool? ret = new(!value);
        ret.SetContext(context);
        return (ret, ErrorNull.Instance);
    }
}
