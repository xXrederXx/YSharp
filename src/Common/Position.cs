using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace YSharp.Common;

/// <summary>
///     This struct stores information about Index, Line, Column and FileId.
///     It does this by storing it into a single 64 bit ulong.
///     It is already optimized for memory
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 8)]
public readonly struct Position : IEquatable<Position>
{
    /// <summary>
    ///     Returns the saved Column as an Int with a max value of 2^10
    /// </summary>
    public ushort Column => (ushort)((_data >> ColumnShift) & ColumnMaskRaw);

    /// <summary>
    ///     Returns the saved File Id as an Int with a max value of 2^8
    /// </summary>
    public byte FileId => (byte)((_data >> FileIdShift) & FileIdMaskRaw);

    /// <summary>
    ///     Returns the saved Index as an Int with a max value of 2^32
    /// </summary>
    public int Index => (int)((_data >> IndexShift) & IndexMaskRaw);

    public readonly bool IsNull => _data == 0;

    /// <summary>
    ///     Returns the saved Line as an ushort with a max value of 2^14
    /// </summary>
    public ushort Line => (ushort)((_data >> LineShift) & LineMaskRaw);

    public static readonly Position Null = new();

    [FieldOffset(0)]
    private readonly ulong _data;

    /// <summary>
    ///     Constructor for a new Position
    /// </summary>
    /// <param name="index">The current index (Cant be negative)</param>
    /// <param name="line">The current Line (Max 2^14)</param>
    /// <param name="column">The current Column (Max 2^10)</param>
    /// <param name="fileId">The current File Id (Max 2^8)</param>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Throws if a param is out of its range
    /// </exception>
    public Position(int index, ushort line, ushort column, byte fileId)
    {
        if (index < 0)
            throw new ArgumentOutOfRangeException(nameof(index), "Index must be non-negative.");
        if (line > LineMaskRaw)
        {
            throw new ArgumentOutOfRangeException(
                nameof(line),
                $"Max value is {LineMaskRaw} (14 bits)"
            );
        }

        if (column > ColumnMaskRaw)
        {
            throw new ArgumentOutOfRangeException(
                nameof(column),
                $"Max value is {ColumnMaskRaw} (10 bits) / {column} > {ColumnMaskRaw}"
            );
        }

        _data =
            ((ulong)(uint)index << IndexShift)
            | ((ulong)line << LineShift)
            | ((ulong)column << ColumnShift)
            | ((ulong)fileId << FileIdShift);
    }

    /// <summary>
    /// This is only used inside position to create new instances based on this one. Be carefull
    /// </summary>
    /// <param name="data">The fully populated data (No checking done!)</param>
    private Position(ulong data)
    {
        _data = data;
    }

    public static bool operator ==(Position left, Position right) => left.Equals(right);

    public static bool operator !=(Position left, Position right) => !left.Equals(right);

    /// <summary>
    ///     Advances the current position taking newlines into a count and returns a new instance
    /// </summary>
    /// <param name="currentChar">The current character</param>
    /// <returns>New Position</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly Position Advance(char currentChar)
    {
        ulong data = _data;

        // Always increment index (top 32 bits)
        data += 1UL << IndexShift;

        if (currentChar is '\n' or '\r')
        {
            // increment line
            data += 1UL << LineShift;

            // reset column (set to 0)
            data &= ~(ColumnMaskRaw << ColumnShift);
        }
        else
        {
            // increment column
            data += 1UL << ColumnShift;
        }

        return new Position(data);
    }

    public override readonly bool Equals(object? obj) => obj is Position posObj && Equals(posObj);

    public readonly bool Equals(Position other) => _data == other._data;

    public override readonly int GetHashCode() => _data.GetHashCode();

    public override readonly string ToString() =>
        $"[Idx: {Index}, Ln: {Line}, Col: {Column}, FID: {FileId}]";

    private const int IndexShift = 32;
    private const int LineShift = 18;
    private const int ColumnShift = 8;
    private const int FileIdShift = 0;

    private const ulong IndexMaskRaw = 0xFFFFFFFFUL; // 32 bits
    private const ulong LineMaskRaw = 0x3FFFUL; // 14 bits
    private const ulong ColumnMaskRaw = 0x3FFUL; // 10 bits
    private const ulong FileIdMaskRaw = 0xFFUL; // 8 bits
}
