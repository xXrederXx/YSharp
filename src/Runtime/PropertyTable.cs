using YSharp.Common;

namespace YSharp.Runtime;

public class PropertyTable<T>
    where T : Value
{
    private readonly Dictionary<string, ValueProperty> _properties;
    public delegate Result<Value, Error> ValueProperty(T self);

    public PropertyTable(ReadOnlySpan<(string, ValueProperty)> properties)
    {
        _properties = [];
        foreach ((string, ValueProperty) prop in properties)
            _properties[prop.Item1] = prop.Item2;
    }

    public Result<Value, Error> Get(string name, T self) =>
        _properties.TryGetValue(name, out ValueProperty? func)
            ? func.Invoke(self)
            : Result<Value, Error>.Fail(new VarNotFoundError(Position.Null, name, self.Context));
}
