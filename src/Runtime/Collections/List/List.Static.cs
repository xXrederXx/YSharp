using YSharp.Common;
using YSharp.Runtime.Primitives.Bool;
using YSharp.Runtime.Primitives.Number;
using YSharp.Runtime.Primitives.String;

namespace YSharp.Runtime.Collections.List;


public sealed partial class VList : Value
{
    private static readonly MethodTable<VList> methodTable;
    private static readonly PropertyTable<VList> propertyTable;

    static VList()
    {
        methodTable = new MethodTable<VList>([("Add", Add), ("Get", Get), ("Remove", Remove), ("IndexOf", IndexOf)]);
        propertyTable = new PropertyTable<VList>([("Length", GetLength)]);
    }

    private static Result<Value, Error> Add(VList self, ReadOnlySpan<Value> args)
    {
        self.value.AddRange(args);
        return Result<Value, Error>.Success(ValueNull.Instance);
    }

    private static Result<int, Error> ConvertToUsableIndex(VList self, ReadOnlySpan<Value> argValues)
    {
        // converts a value to a csharp usable index
        Error err = ValueHelper.CheckArgs(argValues, 1, [typeof(VNumber)], self.Context);
        if (err.IsError) return Result<int, Error>.Fail(err);

        int index = (int)((VNumber)argValues[0]).value;

        if (index >= self.value.Count)
        {
            return Result<int, Error>.Fail(

                new ArgOutOfRangeError(
                    argValues[0].StartPos,
                    "Index was out of range. Must be less than size of list.",
                    self.Context
                )
            );
        }

        if (index < 0) index = self.value.Count + index; // -1 would be last element

        if (index < 0)
        {
            return Result<int, Error>.Fail(

                new ArgOutOfRangeError(
                    argValues[0].StartPos,
                    "Index was out of range. Negative size cant be greater than size of list.",
                    self.Context
                )
            );
        }

        return Result<int, Error>.Success(index);
    }

    private static Result<Value, Error> Get(VList self, ReadOnlySpan<Value> args)
    {
        Result<int, Error> index = ConvertToUsableIndex(self, args);
        if (index.IsFailed) return Result<Value, Error>.Fail(index.GetError());

        return Result<Value, Error>.Success(self.value[index.GetValue()]);
    }

    private static Result<Value, Error> GetLength(VList self) =>
        Result<Value, Error>.Success(new VNumber(self.value.Count));

    private static Result<Value, Error> IndexOf(VList self, ReadOnlySpan<Value> args)
    {
        Error err = ValueHelper.IsRightLength(1, args, self.Context);
        if (err.IsError) return Result<Value, Error>.Fail(err);

        int index = args[0] switch
        {
            VNumber num => self.value.FindIndex(v => v is VNumber n && n.value == num.value),
            VString str => self.value.FindIndex(v => v is VString s && s.value == str.value),
            VBool b => self.value.FindIndex(v => v is VBool boolVal && boolVal.value == b.value),
            VList list => self.value.FindIndex(v =>
                v is VList l && l.value.SequenceEqual(list.value)
            ),
            _ => -1
        };

        return Result<Value, Error>.Success(new VNumber(index));
    }

    private static Result<Value, Error> Remove(VList self, ReadOnlySpan<Value> args)
    {
        Result<int, Error> index = ConvertToUsableIndex(self, args);
        if (index.IsFailed) return Result<Value, Error>.Fail(index.GetError());

        self.value.RemoveAt(index.GetValue());
        return Result<Value, Error>.Success(ValueNull.Instance);
    }
}
