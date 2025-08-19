using YSharp.Types.Common;
using YSharp.Types.Interpreter.Collection;
using YSharp.Types.Interpreter.Internal;

namespace YSharp.Types.Interpreter.Primitives;

public sealed partial class VString
{
    private static MethodTable<VString> methodTable;
    private static PropertyTable<VString> propertyTable;

    static VString()
    {
        methodTable = new MethodTable<VString>([("ToNumber", ToNumber), ("ToBool", ToBool), ("ToUpper", ToUpper), ("ToLower", ToLower), ("Split", Split)]);
        propertyTable = new PropertyTable<VString>([("Length", GetLength)]);
    }
    private static ValueAndError Split(VString self, List<Value> args)
    {
        Error err = ValueHelper.CheckArgs(args, 1, [typeof(VString)], self.context); // format argument
        if (err.IsError)
        {
            return (ValueNull.Instance, err);
        }
        if (args[0] is not VString splitStr)
        {
            return (ValueNull.Instance, new InternalInterpreterError("The splitStr is not Vstring even though Chech args said it is"));
        }

        return (new VList(self.value.Split(splitStr.value).Select(x => (Value)new VString(x)).ToList()), ErrorNull.Instance);
    }
    private static ValueAndError ToUpper(VString self, List<Value> args)
    {
        Error err = ValueHelper.CheckArgs(args, 0, [], self.context); // format argument
        if (err.IsError)
        {
            return (ValueNull.Instance, err);
        }

        return (new VString(self.value.ToUpper()), ErrorNull.Instance);
    }
    private static ValueAndError ToLower(VString self, List<Value> args)
    {
        Error err = ValueHelper.CheckArgs(args, 0, [], self.context); // format argument
        if (err.IsError)
        {
            return (ValueNull.Instance, err);
        }

        return (new VString(self.value.ToUpper()), ErrorNull.Instance);
    }
    private static ValueAndError ToNumber(VString self, List<Value> args)
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

    private static ValueAndError ToBool(VString self, List<Value> args)
    {
        Error err = ValueHelper.CheckArgs(args, 0, [], self.context); // no argument
        if (!err.IsError)
        {
            return (ValueNull.Instance, err);
        }

        return (new VBool(self.IsTrue()), ErrorNull.Instance);
    }

    private static ValueAndError GetLength(VString self) =>
        (new VNumber(self.value.Length), ErrorNull.Instance);

    private static VString AddToString(VString self, VString other) =>
        new VString(self.value + other.value);

    private static VString MulToNum(VString self, VNumber other) =>
        new VString(string.Concat(Enumerable.Repeat(self.value, (int)other.value)));
}
