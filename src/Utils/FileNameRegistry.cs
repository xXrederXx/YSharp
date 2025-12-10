using System.Collections.Concurrent;

namespace YSharp.Utils;

public static class FileNameRegistry
{
    private static readonly string[] _idToName = new string[256];
    private static readonly ConcurrentDictionary<string, byte> _nameToId = new();
    private static int _nextId = 1; // 0 = null

    public static byte GetFileId(string name) => _nameToId.GetOrAdd(name, AllocateId);

    public static string GetFileName(byte id) => _idToName[id] ?? string.Empty;

    private static byte AllocateId(string name)
    {
        // This method is only called when the name does not yet exist.
        int id = Interlocked.Increment(ref _nextId);
        if (id > byte.MaxValue)
            throw new InvalidOperationException("File ID overflow (256 max)");

        byte b = (byte)id;

        // Write to shared array safely – this is a single atomic pointer write
        _idToName[b] = name;

        return b;
    }
}
