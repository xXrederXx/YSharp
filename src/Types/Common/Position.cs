using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace YSharp.Types.Common;

[StructLayout(LayoutKind.Explicit, Size = 8)]
public readonly struct Position : IEquatable<Position>
{
    [FieldOffset(0)]
    public readonly int Index;

    [FieldOffset(4)]
    public readonly uint packed;

    private const int LineShift = 18;
    private const int ColumnShift = 8;
    private const int FileIdShift = 0;

    private const uint LineMaskRaw = 0x3FFF; // 14 bits
    private const uint ColumnMaskRaw = 0x3FF; // 10 bits
    private const uint FileIdMaskRaw = 0xFF; // 8 bits

    public ushort Line => (ushort)((packed >> LineShift) & LineMaskRaw);
    public ushort Column => (ushort)((packed >> ColumnShift) & ColumnMaskRaw);
    public byte FileId => (byte)((packed >> FileIdShift) & FileIdMaskRaw);

    public static readonly Position Null = new();
    public readonly bool IsNull => packed == 0;

    public Position(int index, ushort line, ushort column, byte fileId)
    {
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

        Index = index;
        packed = ((uint)line << LineShift) | ((uint)column << ColumnShift) | fileId;
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

    public override readonly int GetHashCode() => HashCode.Combine(Index, packed);

    public readonly bool Equals(Position other) => Index == other.Index && packed == other.packed;

    public static bool operator ==(Position left, Position right) => left.Equals(right);

    public static bool operator !=(Position left, Position right) => !left.Equals(right);
}
