using YSharp.Common;
using YSharp.Lexer;

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

    public Result<Value, Error> Get(Token<string> nameToken, T self) =>
        _properties.TryGetValue(nameToken.Value, out ValueProperty? func)
            ? func.Invoke(self)
            : Result<Value, Error>.Fail(new VarNotFoundError(nameToken.StartPos, nameToken.Value, self.Context));
}
