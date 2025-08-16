using YSharp.Types.Common;

namespace YSharp.Types.Interpreter;

public record ValueAndError(Value Value, Error Error)
{
    public bool ValueIsNull => Value is null or ValueNull;

    public static implicit operator ValueAndError((Value, Error) other)
    {
        return new ValueAndError(other.Item1, other.Item2);
    }
}

public class MethodTable<T>
    where T : Value
{
    private readonly Dictionary<string, Func<T, List<Value>, ValueAndError>> Methods;

    public MethodTable(ReadOnlySpan<(string, Func<T, List<Value>, ValueAndError>)> methods)
    {
        Methods = [];
        foreach ((string, Func<T, List<Value>, ValueAndError>) method in methods)
        {
            Methods[method.Item1] = method.Item2;
        }
    }

    public ValueAndError Invoke(string name, T self, List<Value> args) =>
        Methods.TryGetValue(name, out var func)
            ? func(self, args)
            : (ValueNull.Instance, new FuncNotFoundError(Position.Null, name, self.context));
}

public class PropertyTable<T>
    where T : Value
{
    private readonly Dictionary<string, Func<T, ValueAndError>> Properties;

    public PropertyTable(ReadOnlySpan<(string, Func<T, ValueAndError>)> properties)
    {
        Properties = [];
        foreach ((string, Func<T, ValueAndError>) prop in properties)
        {
            Properties[prop.Item1] = prop.Item2;
        }
    }

    public ValueAndError Get(string name, T self) =>
        Properties.TryGetValue(name, out var func)
            ? func(self)
            : (ValueNull.Instance, new VarNotFoundError(Position.Null, name, self.context));
}
