using YSharp.Common;
using YSharp.Runtime.Collections.List;
using YSharp.Runtime.Primatives.Bool;
using YSharp.Runtime.Primatives.Number;

namespace YSharp.Runtime.Primatives.String;


public sealed partial class VString
{
    private static readonly MethodTable<VString> methodTable;
    private static readonly PropertyTable<VString> propertyTable;

    static VString()
    {
        methodTable = new MethodTable<VString>([
            ("ToNumber", ToNumber), ("ToBool", ToBool), ("ToUpper", ToUpper), ("ToLower", ToLower), ("Split", Split)
        ]);
        propertyTable = new PropertyTable<VString>([("Length", GetLength)]);
    }

    private static VString AddToString(VString self, VString other) => new(self.value + other.value);

    private static ValueAndError GetLength(VString self) =>
        (new VNumber(self.value.Length), ErrorNull.Instance);

    private static VString MulToNum(VString self, VNumber other) =>
        new(string.Concat(Enumerable.Repeat(self.value, (int)other.value)));

    private static ValueAndError Split(VString self, List<Value> args)
    {
        Error err = ValueHelper.CheckArgs(args, 1, [typeof(VString)], self.context); // format argument
        if (err.IsError) return (ValueNull.Instance, err);
        if (args[0] is not VString splitStr)
            return (ValueNull.Instance,
                new InternalInterpreterError("The splitStr is not Vstring even though Chech args said it is"));

        return (new VList(self.value.Split(splitStr.value).Select(x => (Value)new VString(x)).ToList()),
            ErrorNull.Instance);
    }

    private static ValueAndError ToBool(VString self, List<Value> args)
    {
        Error err = ValueHelper.CheckArgs(args, 0, [], self.context); // no argument
        if (!err.IsError) return (ValueNull.Instance, err);

        return (new VBool(self.IsTrue()), ErrorNull.Instance);
    }

    private static ValueAndError ToLower(VString self, List<Value> args)
    {
        Error err = ValueHelper.CheckArgs(args, 0, [], self.context); // format argument
        if (err.IsError) return (ValueNull.Instance, err);

        return (new VString(self.value.ToUpper()), ErrorNull.Instance);
    }

    private static ValueAndError ToNumber(VString self, List<Value> args)
    {
        Error err = ValueHelper.CheckArgs(args, 0, [typeof(VString)], self.context); // format argument
        if (err.IsError) return (ValueNull.Instance, err);

        if (double.TryParse(self.value, out double res)) return (new VNumber(res), ErrorNull.Instance);

        return (
            ValueNull.Instance,
            new WrongFormatError(
                self.startPos,
                self.value + " can't be converted to Number",
                self.context
            )
        );
    }

    private static ValueAndError ToUpper(VString self, List<Value> args)
    {
        Error err = ValueHelper.CheckArgs(args, 0, [], self.context); // format argument
        if (err.IsError) return (ValueNull.Instance, err);

        return (new VString(self.value.ToUpper()), ErrorNull.Instance);
    }
}