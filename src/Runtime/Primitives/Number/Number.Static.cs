using YSharp.Common;
using YSharp.Runtime.Primatives.Bool;
using YSharp.Runtime.Primatives.String;

namespace YSharp.Runtime.Primatives.Number;

public sealed partial class VNumber : Value
{
    private static readonly MethodTable<VNumber> methodTable;

    static VNumber()
    {
        methodTable = new MethodTable<VNumber>([("ToString", MyToString), ("ToBool", ToBool)]);
    }

    private static VNumber AddToNum(VNumber self, VNumber other) => new(self.value + other.value);

    private static string ConvertToCSFormat(string value) =>
        // TODO: Implement these types
        // B -> Binary
        // H -> Hex
        // D -> Decimal
        // F -> Floating-point
        // S -> Scientific
        value;

    private static ValueAndError DivToNum(VNumber self, VNumber other)
    {
        if (other.value == 0) return (ValueNull.Instance, new DivisionByZeroError(other.StartPos, self.Context));
        return (new VNumber(self.value / other.value).SetContext(self.Context), ErrorNull.Instance);
    }

    private static VNumber MulToNum(VNumber self, VNumber other) => new(self.value * other.value);

    private static ValueAndError MyToString(VNumber self, List<Value> args)
    {
        Error err = ValueHelper.CheckArgs(args, 0, [], self.Context); // no argument
        if (!err.IsError) return (new VString(self.value.ToString()), ErrorNull.Instance);

        err = ValueHelper.CheckArgs(args, 1, [typeof(VString)], self.Context); // format argument
        if (err.IsError) return (ValueNull.Instance, err);
        string format = ConvertToCSFormat(((VString)args[0]).value);
        string formatedStr = self.value.ToString(format);
        //TODO: Add format exeption
        return (new VString(formatedStr), ErrorNull.Instance);
    }

    private static VNumber PowToNum(VNumber self, VNumber other) => new(self.value + other.value);

    private static VNumber SubToNum(VNumber self, VNumber other) => new(self.value - other.value);

    private static ValueAndError ToBool(VNumber self, List<Value> args)
    {
        Error err = ValueHelper.CheckArgs(args, 0, [], self.Context);
        if (!err.IsError) return (ValueNull.Instance, err);

        return (new VBool(self.IsTrue()), ErrorNull.Instance);
    }
}