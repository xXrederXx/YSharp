using YSharp.Common;
using YSharp.Runtime.Primatives.Number;
using YSharp.Runtime.Primatives.String;

namespace YSharp.Runtime.Primatives.Datetime;

public sealed partial class VDateTime
{
    private static readonly PropertyTable<VDateTime> propertyTable;

    static VDateTime()
    {
        propertyTable = new PropertyTable<VDateTime>([
            ("Microsecond", self => Result<Value, Error>.Success(new VNumber(self.dateTime.Microsecond))),
            ("Millisecond", self => Result<Value, Error>.Success(new VNumber(self.dateTime.Millisecond))),
            ("Second", self => Result<Value, Error>.Success(new VNumber(self.dateTime.Second))),
            ("Minute", self => Result<Value, Error>.Success(new VNumber(self.dateTime.Minute))),
            ("Hour", self => Result<Value, Error>.Success(new VNumber(self.dateTime.Hour))),
            ("DayOfMonth", self => Result<Value, Error>.Success(new VNumber(self.dateTime.Day))),
            (
                "DayOfWeek",
                self => Result<Value, Error>.Success(new VString(self.dateTime.DayOfWeek.ToString()))
            ),
            ("DayOfYear", self => Result<Value, Error>.Success(new VNumber(self.dateTime.DayOfYear))),
            ("Month", self => Result<Value, Error>.Success(new VNumber(self.dateTime.Month))),
            ("Year", self => Result<Value, Error>.Success(new VNumber(self.dateTime.Year))),
        ]);
    }

    private static VDateTime AddToDate(VDateTime self, VDateTime other) =>
        new(new DateTime(self.dateTime.Ticks + other.dateTime.Ticks));

    private static VDateTime SubToDate(VDateTime self, VDateTime other) =>
        new(new DateTime(self.dateTime.Ticks - other.dateTime.Ticks));
}
