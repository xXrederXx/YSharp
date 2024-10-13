namespace YSharp_2._0;


// Use a struct for Position to reduce heap allocations
public struct Position
{
    public static readonly Position _null = new();
    public int Index { get; private set; }
    public int Line { get; private set; }
    public int Column { get; private set; }
    public string FileName { get; }
    public string FileText { get; }
    public readonly bool IsNull;

    // Constructor
    public Position(int index, int line, int column, string fileName, string fileText)
    {
        Index = index;
        Line = line;
        Column = column;
        FileName = fileName;
        FileText = fileText;

        IsNull = false;
    }
    public Position()
    {
        IsNull = true;

        FileName = "fileName";
        FileText = "fileText";
    }

    // Advance to the next character
    public Position Advance(char currentChar)
    {
        Index++;
        Column++;

        if (currentChar is '\n' or '\r')
        {
            Line++;
            Column = 0;
        }
        return this;
    }

    // String representation for debugging
    public override readonly string ToString()
    {
        return $"Column {Column}, Index {Index}";
    }
}
