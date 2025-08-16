using YSharp.Types.Common;

namespace YSharp.Types.Interpreter.Primatives;

public sealed class VString(string value) : Value()
{
    public string value { get; set; } = value;

    private static MethodTable<VString> methodTable;
    private static PropertyTable<VString> propertyTable;

    static VString()
    {
        methodTable = new MethodTable<VString>([("ToNumber", ToNumber), ("ToBool", ToBool)]);
        propertyTable = new PropertyTable<VString>([("Length", GetLength)]);
    }

    public static ValueAndError ToNumber(VString self, List<Value> args)
    {
        Error err = ValueHelper.CheckArgs(args, 0, [typeof(VString)], self.context); // format argument
        if (err.IsError)
        {
            return (ValueNull.Instance, err);
        }

        if (double.TryParse(self.value, out double res))
        {
            return (new VNumber(res), ErrorNull.Instance);
        }
        else
        {
            return (
                ValueNull.Instance,
                new WrongFormatError(
                    self.startPos,
                    self.value + " can't be converted to Number",
                    self.context
                )
            );
        }
    }

    public static ValueAndError ToBool(VString self, List<Value> args)
    {
        Error err = ValueHelper.CheckArgs(args, 0, [], self.context); // no argument
        if (!err.IsError)
        {
            return (ValueNull.Instance, err);
        }

        return (new VBool(self.IsTrue()), ErrorNull.Instance);
    }

    public static ValueAndError GetLength(VString self) =>
        (new VNumber(self.value.Length), ErrorNull.Instance);

    public override ValueAndError GetFunc(string name, List<Value> argNodes) =>
        methodTable.Invoke(name, this, argNodes);

    public override ValueAndError GetVar(string name) => propertyTable.Get(name, this);

    public override ValueAndError AddedTo(Value other)
    {
        return other switch
        {
            VString strOther => (new VString(value + strOther.value).SetContext(context), ErrorNull.Instance),
            _ => base.AddedTo(other),
        };
    }

    public override ValueAndError MuledTo(Value other)
    {
        Value? ret = null;
        if (other is VNumber)
        {
            string _value = "";
            int mulTimes = (int)((VNumber)other).value;

            for (int i = 0; i < mulTimes; i++)
            {
                _value += value;
            }
            ret = new VString(_value).SetContext(context);
        }

        if (ret == null)
        {
            return (ValueNull.Instance, IlligalOperation(other));
        }
        return (ret, ErrorNull.Instance);
    }

    public override ValueAndError GetComparisonEQ(Value other)
    {
        if (other is VString _other)
        {
            return (new VBool(value == _other.value), ErrorNull.Instance);
        }
        else
        {
            return (ValueNull.Instance, IlligalOperation(other));
        }
    }

    public override ValueAndError GetComparisonNE(Value other)
    {
        if (other is VString _other)
        {
            return (new VBool(value != _other.value), ErrorNull.Instance);
        }
        else
        {
            return (ValueNull.Instance, IlligalOperation(other));
        }
    }

    public override bool IsTrue()
    {
        return value.Length > 0;
    }

    public override Value Copy()
    {
        VString copy = new(value);
        copy.SetPos(startPos, endPos);
        copy.SetContext(context);
        return copy;
    }

    public override string ToString()
    {
        return $"\"{value}\"";
    }
}
