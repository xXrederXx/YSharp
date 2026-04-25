using YSharp.Common;
using YSharp.Lexer;

namespace YSharp.Runtime.Primitives.Datetime;

public sealed partial class VDateTime(DateTime? dateTime = null) : Value
{
    public DateTime dateTime = dateTime ?? DateTime.Now;

    public override VDateTime Copy()
    {
        VDateTime copy = new(dateTime);
        copy.SetPos(StartPos, EndPos);
        copy.SetContext(Context);
        return copy;
    }

    public override Result<Value, Error> GetVar(Token<string> nameToken) => propertyTable.Get(nameToken, this);

    public override string ToString() => dateTime.ToString("MM-dd-yyyyTHH:mm:ss.fffffff");
}
