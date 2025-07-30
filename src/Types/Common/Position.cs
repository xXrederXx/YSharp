using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace YSharp.Types.Common;

[StructLayout(LayoutKind.Explicit, Size = 8)]
public readonly struct Position : IEquatable<Position>
{
    [FieldOffset(0)]
    private readonly ulong _data;

    private const int IndexShift = 32;
    private const int LineShift = 18;
    private const int ColumnShift = 8;
    private const int FileIdShift = 0;

    private const ulong IndexMaskRaw = 0xFFFFFFFFUL; // 32 bits
    private const ulong LineMaskRaw = 0x3FFFUL; // 14 bits
    private const ulong ColumnMaskRaw = 0x3FFUL; // 10 bits
    private const ulong FileIdMaskRaw = 0xFFUL; // 8 bits

    public int Index => (int)((_data >> IndexShift) & IndexMaskRaw);
    public ushort Line => (ushort)((_data >> LineShift) & LineMaskRaw);
    public ushort Column => (ushort)((_data >> ColumnShift) & ColumnMaskRaw);
    public byte FileId => (byte)((_data >> FileIdShift) & FileIdMaskRaw);

    public static readonly Position Null = new();
    public readonly bool IsNull => _data == 0;

    public Position(int index, ushort line, ushort column, byte fileId)
    {
        if (index < 0)
            throw new ArgumentOutOfRangeException(nameof(index), "Index must be non-negative.");
        if (line > LineMaskRaw)
            throw new ArgumentOutOfRangeException(
                nameof(line),
                $"Max value is {LineMaskRaw} (14 bits)"
            );
        if (column > ColumnMaskRaw)
            throw new ArgumentOutOfRangeException(
                nameof(column),
                $"Max value is {ColumnMaskRaw} (10 bits) / {column} > {ColumnMaskRaw}"
            );

        _data =
            ((ulong)(uint)index << IndexShift)
            | ((ulong)line << LineShift)
            | ((ulong)column << ColumnShift)
            | ((ulong)fileId << FileIdShift);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly Position Advance(char currentChar)
    {
        ushort line = Line;
        ushort col = Column;

        if (currentChar is '\n' or '\r')
        {
            line++;
            col = 0;
        }
        else
        {
            col++;
        }

        return new Position(Index + 1, line, col, FileId);
    }

    public override readonly string ToString() =>
        $"[Idx: {Index}, Ln: {Line}, Col: {Column}, FID: {FileId}]";

    public override readonly bool Equals(object? obj) => obj is Position posObj && Equals(posObj);

    public override readonly int GetHashCode() => _data.GetHashCode();

    public readonly bool Equals(Position other) => _data == other._data;

    public static bool operator ==(Position left, Position right) => left.Equals(right);

    public static bool operator !=(Position left, Position right) => !left.Equals(right);
}
