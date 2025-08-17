using YSharp.Types.Common;

namespace YSharp.Types.Interpreter.Internal;

public record ValueAndError(Value Value, Error Error)
{
    public bool ValueIsNull => Value is null or ValueNull;

    public static implicit operator ValueAndError((Value, Error) other)
    {
        return new ValueAndError(other.Item1, other.Item2);
    }
}
