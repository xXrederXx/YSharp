using YSharp.Utils;

namespace YSharp.Types.Common;

public struct Position : IEquatable<Position>
{
    public static readonly Position Null = new();

    // Auto-properties for better memory layout
    public int Index; // 4 bytes -> Up to 2,147,483,647
    public ushort Line; // 2 bytes -> Up to 65,535
    public ushort Column; // 2 bytes -> Up to 65,535
    public readonly byte FileId; // 1 byte -> Up to 255

    // Toatal 9 bytes

    // Constructor for a valid position
    public Position(int index, ushort line, ushort column, string fileName)
    {
        Index = index;
        Line = line;
        Column = column;
        FileId = FileNameRegistry.GetFileId(fileName);
    }

    // Default constructor for a "null" position
    public Position()
    {
        Index = 0;
        Line = 0;
        Column = 0;
        FileId = 0;
    }

    // Advance to the next character
    public void Advance(char currentChar)
    {
        Index++;
        Column++;
        // Adjust for line breaks
        if (currentChar is '\n' or '\r')
        {
            Line++;
            Column = 0;
        }
    }

    // String representation for debugging
    public override readonly string ToString() =>
        $"[Idx: {Index}, Ln: {Line}, Col: {Column}, FID: {FileId}]";

    public override readonly bool Equals(object? obj)
    {
        return obj is Position posObj && Equals(posObj);
    }

    public override readonly int GetHashCode()
    {
        return HashCode.Combine(Index, Line, Column, FileId);
    }

    public readonly bool Equals(Position other)
    {
        return Index == other.Index
            && Line == other.Line
            && Column == other.Column
            && FileId == other.FileId;
    }

    public static bool operator ==(Position left, Position right) => left.Equals(right);

    public static bool operator !=(Position left, Position right) => !left.Equals(right);

    public readonly bool IsNull => FileId == 0;
}
