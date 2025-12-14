using YSharp.Common;

namespace YSharp.Runtime;

public record ValueAndError(Value Value, Error Error)
{
    public bool ValueIsNull => Value is null or ValueNull;

    public static implicit operator ValueAndError((Value, Error) other) =>
        new(other.Item1, other.Item2);
}
