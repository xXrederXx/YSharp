namespace YSharp.Types.InternalTypes;

public struct Position
{
    public static readonly Position Null = new();

    // Auto-properties for better memory layout
    public int Index { get; private set; }
    public int Line { get; private set; }
    public int Column { get; private set; }
    public readonly string FileName { get; }

    // Constructor for a valid position
    public Position(int index, int line, int column, string fileName)
    {
        Index = index;
        Line = line;
        Column = column;
        FileName = fileName ?? throw new ArgumentNullException(nameof(fileName)); // Avoid null strings
    }

    // Default constructor for a "null" position
    public Position()
    {
        Index = 0;
        Line = 0;
        Column = 0;
        FileName = string.Empty; // Use empty string instead of allocating "fileName"
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
    public override readonly string ToString()
    {
        return $"Column {Column}, Index {Index}";
    }

    // Check if the position is "null"
    public readonly bool IsNull => string.IsNullOrEmpty(FileName);
}
