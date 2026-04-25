using YSharp.Common;

namespace YSharp.Runtime;

public static class ValueHelper
{
    public static Error CheckArgs(
        ReadOnlySpan<Value> argValue,
        int length,
        List<Type> types,
        Context? context
    )
    {
        Error ret;
        ret = IsRightLength(length, argValue, context ?? new Context());
        if (ret.IsError) return ret;
        if (length > 0) ret = IsRightType(types, argValue, context ?? new Context());
        return ret;
    }

    public static Error IsRightLength(int length, ReadOnlySpan<Value> argValue, Context? context)
    {
        if (length == argValue.Length) return ErrorNull.Instance;
        return new NumArgsError(Position.Null, length, argValue.Length, context);
    }

    public static Error IsRightType(List<Type> types, ReadOnlySpan<Value> argValue, Context context)
    {
        if (types.Count != 1 && types.Count != argValue.Length)
            return new InternalInterpreterError(
                "An error occured when trying to check the args passed into a function. But it failed when trying to check the types. The problem was that the args in the internal function didnt match a criteria. ");
        bool oneType = types.Count == 1;
        for (int i = 0; i < argValue.Length; i++)
        {
            Type valType = argValue[i].GetType();

            if (oneType && valType != types[0])
            {
                // one type
                return new WrongFormatError(
                    Position.Null,
                    $"Value should have Type {types[0]} and not {valType}",
                    context
                );
            }

            if (valType != types[i])
            {
                return new WrongFormatError(
                    Position.Null,
                    $"Value should have {types[0]} and not Type {valType}",
                    context
                );
            }
        }

        return ErrorNull.Instance;
    }
}
