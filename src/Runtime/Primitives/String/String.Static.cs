using YSharp.Common;
using YSharp.Runtime.Collections.List;
using YSharp.Runtime.Primitives.Bool;
using YSharp.Runtime.Primitives.Number;

namespace YSharp.Runtime.Primitives.String;


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

    private static Result<Value, Error> GetLength(VString self) =>
        Result<Value, Error>.Success(new VNumber(self.value.Length));

    private static VString MulToNum(VString self, VNumber other) =>
        new(string.Concat(Enumerable.Repeat(self.value, (int)other.value)));

    private static Result<Value, Error> Split(VString self, List<Value> args)
    {
        Error err = ValueHelper.CheckArgs(args, 1, [typeof(VString)], self.Context); // format argument
        if (err.IsError) return Result<Value, Error>.Fail(err);
        if (args[0] is not VString splitStr)
            return Result<Value, Error>.Fail(
                new InternalInterpreterError("The splitStr is not Vstring even though Chech args said it is"));

        return Result<Value, Error>.Success(new VList(self.value.Split(splitStr.value).Select(x => (Value)new VString(x)).ToList()));
    }

    private static Result<Value, Error> ToBool(VString self, List<Value> args)
    {
        Error err = ValueHelper.CheckArgs(args, 0, [], self.Context); // no argument
        if (err.IsError) return Result<Value, Error>.Fail(err);

        return Result<Value, Error>.Success(new VBool(self.IsTrue()));
    }

    private static Result<Value, Error> ToLower(VString self, List<Value> args)
    {
        Error err = ValueHelper.CheckArgs(args, 0, [], self.Context); // format argument
        if (err.IsError) return Result<Value, Error>.Fail(err);

        return Result<Value, Error>.Success(new VString(self.value.ToLower()));
    }

    private static Result<Value, Error> ToNumber(VString self, List<Value> args)
    {
        Error err = ValueHelper.CheckArgs(args, 0, [typeof(VString)], self.Context); // format argument
        if (err.IsError) return Result<Value, Error>.Fail(err);

        if (double.TryParse(self.value, out double res)) return Result<Value, Error>.Success(new VNumber(res));

        return Result<Value, Error>.Fail(
            new WrongFormatError(
                self.StartPos,
                self.value + " can't be converted to Number",
                self.Context
            )
        );
    }

    private static Result<Value, Error> ToUpper(VString self, List<Value> args)
    {
        Error err = ValueHelper.CheckArgs(args, 0, [], self.Context); // format argument
        if (err.IsError) return Result<Value, Error>.Fail(err);

        return Result<Value, Error>.Success(new VString(self.value.ToUpper()));
    }
}
