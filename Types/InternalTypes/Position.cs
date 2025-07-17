using YSharp.Utility;

namespace YSharp.Types.InternalTypes;

public struct Position
{
    public static readonly Position Null = new();

    // Auto-properties for better memory layout
    public int Index;
    public int Line;
    public int Column;
    public readonly byte FileId;

    // Constructor for a valid position
    public Position(int index, int line, int column, string fileName)
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
    public override string ToString() =>
        $"[Idx: {Index}, Ln: {Line}, Col: {Column}, FID: {FileId}]";


    public readonly bool IsNull => FileId == 0;
}
