using YSharp.Common;
using YSharp.Runtime.Primatives.Bool;
using YSharp.Runtime.Primatives.Number;
using YSharp.Runtime.Primatives.String;

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

    private static ValueAndError Add(VList self, List<Value> args)
    {
        self.value.AddRange(args);
        return (ValueNull.Instance, ErrorNull.Instance);
    }

    private static (int, Error) ConvertToUsableIndex(VList self, List<Value> argValues)
    {
        // converts a value to a csharp usable index
        Error err = ValueHelper.CheckArgs(argValues, 1, [typeof(VNumber)], self.context);
        if (err.IsError) return (0, err);

        int index = (int)((VNumber)argValues[0]).value;

        if (index >= self.value.Count)
        {
            return (
                0,
                new ArgOutOfRangeError(
                    argValues[0].startPos,
                    "Index was out of range. Must be less than size of list.",
                    self.context
                )
            );
        }

        if (index < 0) index = self.value.Count + index; // -1 would be last element

        if (index < 0)
        {
            return (
                0,
                new ArgOutOfRangeError(
                    argValues[0].startPos,
                    "Index was out of range. Negative size cant be greater than size of list.",
                    self.context
                )
            );
        }

        return (index, ErrorNull.Instance);
    }

    private static ValueAndError Get(VList self, List<Value> args)
    {
        (int, Error) index = ConvertToUsableIndex(self, args);
        if (index.Item2.IsError) return (ValueNull.Instance, index.Item2);

        return (self.value[index.Item1], ErrorNull.Instance);
    }

    private static ValueAndError GetLength(VList self) =>
        (new VNumber(self.value.Count), ErrorNull.Instance);

    private static ValueAndError IndexOf(VList self, List<Value> args)
    {
        Error err = ValueHelper.IsRightLength(1, args, self.context);
        if (err.IsError) return (ValueNull.Instance, err);

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

        return (new VNumber(index), ErrorNull.Instance);
    }

    private static ValueAndError Remove(VList self, List<Value> args)
    {
        (int, Error) index = ConvertToUsableIndex(self, args);
        if (index.Item2.IsError) return (ValueNull.Instance, index.Item2);

        self.value.RemoveAt(index.Item1);
        return (ValueNull.Instance, ErrorNull.Instance);
    }
}