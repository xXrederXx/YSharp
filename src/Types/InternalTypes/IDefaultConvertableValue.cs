namespace YSharp.Types.InternalTypes;

public interface IDefaultConvertableValue{}

public interface IDefaultConvertableValue<T> : IDefaultConvertableValue
{
    T value { get; set; }
}
