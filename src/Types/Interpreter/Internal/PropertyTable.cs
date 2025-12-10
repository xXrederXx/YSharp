using YSharp.Types.Common;

namespace YSharp.Types.Interpreter.Internal;

public class PropertyTable<T>
    where T : Value
{
    private readonly Dictionary<string, Func<T, ValueAndError>> Properties;

    public PropertyTable(ReadOnlySpan<(string, Func<T, ValueAndError>)> properties)
    {
        Properties = [];
        foreach ((string, Func<T, ValueAndError>) prop in properties) Properties[prop.Item1] = prop.Item2;
    }

    public ValueAndError Get(string name, T self) =>
        Properties.TryGetValue(name, out Func<T, ValueAndError>? func)
            ? func(self)
            : (ValueNull.Instance, new VarNotFoundError(Position.Null, name, self.context));
}