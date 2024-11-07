

namespace YSharp_2._0;

public record ElseCaseData(INode? Node, bool? Bool){
    public static ElseCaseData _null = new(null, null);
}
public record IfExpresionCases(INode condition, INode expression, bool returnNull){}

public record ValueError(Value _value, Error _error){
    public Value value = _value;
    public Error error= _error;
    public bool ValueIsNull => value is null or ValueNull;

    public static implicit operator ValueError((Value, Error) other){
        return new ValueError(other.Item1, other.Item2);
    }
}
