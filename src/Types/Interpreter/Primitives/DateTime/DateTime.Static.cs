using YSharp.Types.Common;

namespace YSharp.Types.Interpreter.Primitives;

public sealed partial class VDateTime
{
    private static PropertyTable<VDateTime> propertyTable;

    static VDateTime()
    {
        propertyTable = new PropertyTable<VDateTime>(
            [
                (
                    "Microsecond",
                    (self) => (new VNumber(self.dateTime.Microsecond), ErrorNull.Instance)
                ),
                (
                    "Millisecond",
                    (self) => (new VNumber(self.dateTime.Millisecond), ErrorNull.Instance)
                ),
                ("Second", (self) => (new VNumber(self.dateTime.Second), ErrorNull.Instance)),
                ("Minute", (self) => (new VNumber(self.dateTime.Minute), ErrorNull.Instance)),
                ("Hour", (self) => (new VNumber(self.dateTime.Hour), ErrorNull.Instance)),
                ("DayOfMonth", (self) => (new VNumber(self.dateTime.Day), ErrorNull.Instance)),
                (
                    "DayOfWeek",
                    (self) => (new VString(self.dateTime.DayOfWeek.ToString()), ErrorNull.Instance)
                ),
                ("DayOfYear", (self) => (new VNumber(self.dateTime.DayOfYear), ErrorNull.Instance)),
                ("Month", (self) => (new VNumber(self.dateTime.Month), ErrorNull.Instance)),
                ("Year", (self) => (new VNumber(self.dateTime.Year), ErrorNull.Instance)),
            ]
        );
    }

    private static VDateTime AddToDate(VDateTime self, VDateTime other) =>
        new VDateTime(new DateTime(self.dateTime.Ticks + other.dateTime.Ticks));

    private static VDateTime SubToDate(VDateTime self, VDateTime other) =>
        new VDateTime(new DateTime(self.dateTime.Ticks - other.dateTime.Ticks));
}
