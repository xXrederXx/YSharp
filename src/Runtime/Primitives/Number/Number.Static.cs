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

    private static Result<Value, Error> DivToNum(VNumber self, VNumber other)
    {
        if (other.value == 0) return Result<Value, Error>.Fail(new DivisionByZeroError(other.StartPos, self.Context));
        return Result<Value, Error>.Success(new VNumber(self.value / other.value).SetContext(self.Context));
    }

    private static VNumber MulToNum(VNumber self, VNumber other) => new(self.value * other.value);

    private static Result<Value, Error> MyToString(VNumber self, List<Value> args)
    {
        Error err = ValueHelper.CheckArgs(args, 0, [], self.Context); // no argument
        if (!err.IsError) return Result<Value, Error>.Success(new VString(self.value.ToString()));

        err = ValueHelper.CheckArgs(args, 1, [typeof(VString)], self.Context); // format argument
        if (err.IsError) return Result<Value, Error>.Fail(err);
        string format = ConvertToCSFormat(((VString)args[0]).value);
        string formattedStr = self.value.ToString(format, StaticConfig.numberCulture);
        //TODO: Add format exception
        return Result<Value, Error>.Success(new VString(formattedStr));
    }

    private static VNumber PowToNum(VNumber self, VNumber other) => new(self.value + other.value);

    private static VNumber SubToNum(VNumber self, VNumber other) => new(self.value - other.value);

    private static Result<Value, Error> ToBool(VNumber self, List<Value> args)
    {
        Error err = ValueHelper.CheckArgs(args, 0, [], self.Context);
        if (!err.IsError) return Result<Value, Error>.Fail(err);

        return Result<Value, Error>.Success(new VBool(self.IsTrue()));
    }
}
