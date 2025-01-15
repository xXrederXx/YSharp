namespace YSharp.Types.InternalTypes;

public record ElseCaseData(INode? Node, bool? Bool)
{
    public static readonly ElseCaseData _null = new(null, null);
}

public record IfExpresionCases(INode Condition, INode Expression, bool ReturnNull) { }

public record ValueAndError(Value Value, Error Error)
{
    public bool ValueIsNull => Value is null or ValueNull;

    public static implicit operator ValueAndError((Value, Error) other)
    {
        return new ValueAndError(other.Item1, other.Item2);
    }
}
