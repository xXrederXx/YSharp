using YSharp.Common;

namespace YSharp.Runtime;

public class PropertyTable<T>
    where T : Value
{
    private readonly Dictionary<string, Func<T, Result<Value, Error>>> Properties;

    public PropertyTable(ReadOnlySpan<(string, Func<T, Result<Value, Error>>)> properties)
    {
        Properties = [];
        foreach ((string, Func<T, Result<Value, Error>>) prop in properties)
            Properties[prop.Item1] = prop.Item2;
    }

    public Result<Value, Error> Get(string name, T self) =>
        Properties.TryGetValue(name, out Func<T, Result<Value, Error>>? func)
            ? func(self)
            : Result<Value, Error>.Fail(new VarNotFoundError(Position.Null, name, self.Context));
}
