namespace YSharp.Utils;

public static class FileNameRegistry{
    private static readonly string[] _idToName = new string[256];
    private static readonly Dictionary<string, byte> _nameToId = new();
    private static byte _nextId = 1; // 0 = "null"

    public static byte GetFileId(string name)
    {
        if (_nameToId.TryGetValue(name, out byte id)) return id;
        if (_nextId == 0) throw new InvalidOperationException("File ID overflow (256 max)");

        id = _nextId++;
        _nameToId[name] = id;
        _idToName[id] = name;
        return id;
    }

    public static string GetFileName(byte id) => _idToName[id] ?? string.Empty;
}