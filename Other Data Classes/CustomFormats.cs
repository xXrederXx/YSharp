

namespace YSharp_2._0;

public readonly struct ElseCaseData(Node? _node, bool? _bool){ // Node Bool Nullable
    public static readonly ElseCaseData _null = new(null, null);
    public readonly bool? Bool = _bool;
    public readonly Node? Node = _node;
}

public readonly struct IfExpresionCases(Node condition, Node expression, bool returnNull){
    public readonly Node condition = condition;
    public readonly Node expresion = expression;
    public readonly bool returnNull = returnNull;
}

public readonly struct ValueError(Value _value, Error _error){
    public readonly Value value = _value;
    public readonly Error error= _error;
    public readonly bool ValueIsNull => value is null or ValueNull;

    public static implicit operator ValueError((Value, Error) other){
        return new ValueError(other.Item1, other.Item2);
    }
}
