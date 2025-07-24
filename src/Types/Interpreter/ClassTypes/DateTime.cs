using YSharp.Types.Common;

namespace YSharp.Types.Interpreter.ClassTypes;

public class VDateTime(DateTime? dateTime = null) : Value
{
    public DateTime dateTime = dateTime ?? DateTime.Now;

    public override ValueAndError GetVar(string name)
    {
        return name switch
        {
            "Microsecond" => (ValueAndError)(new VNumber(dateTime.Microsecond), ErrorNull.Instance), // Microseconds approximation
            "Millisecond" => (ValueAndError)(new VNumber(dateTime.Millisecond), ErrorNull.Instance),
            "Second" => (ValueAndError)(new VNumber(dateTime.Second), ErrorNull.Instance),
            "Minute" => (ValueAndError)(new VNumber(dateTime.Minute), ErrorNull.Instance),
            "Hour" => (ValueAndError)(new VNumber(dateTime.Hour), ErrorNull.Instance),
            "DayOfMonth" => (ValueAndError)(new VNumber(dateTime.Day), ErrorNull.Instance),
            "DayOfWeek" => (ValueAndError)
                (new VString(dateTime.DayOfWeek.ToString()), ErrorNull.Instance),
            "DayOfYear" => (ValueAndError)(new VNumber(dateTime.DayOfYear), ErrorNull.Instance),
            "Month" => (ValueAndError)(new VNumber(dateTime.Month), ErrorNull.Instance),
            "Year" => (ValueAndError)(new VNumber(dateTime.Year), ErrorNull.Instance),
            _ => base.GetVar(name),
        };
    }

    public override ValueAndError AddedTo(Value other)
    {
        if (other is VDateTime otherDateTime)
        {
            DateTime newDateTime = dateTime
                .AddMicroseconds(otherDateTime.dateTime.Microsecond) // Microseconds approximation
                .AddMilliseconds(otherDateTime.dateTime.Millisecond)
                .AddSeconds(otherDateTime.dateTime.Second)
                .AddMinutes(otherDateTime.dateTime.Minute)
                .AddHours(otherDateTime.dateTime.Hour)
                .AddDays(otherDateTime.dateTime.Day - 1)
                .AddMonths(otherDateTime.dateTime.Month - 1)
                .AddYears(otherDateTime.dateTime.Year - 1); // -1 as we add the years later

            return (new VDateTime(newDateTime), ErrorNull.Instance);
        }

        return (ValueNull.Instance, IlligalOperation(other));
    }

    public override ValueAndError SubedTo(Value other)
    {
        if (other is VDateTime otherDateTime)
        {
            DateTime newDateTime = dateTime
                .AddMicroseconds(-otherDateTime.dateTime.Microsecond) // Microseconds approximation
                .AddMilliseconds(-otherDateTime.dateTime.Millisecond)
                .AddSeconds(-otherDateTime.dateTime.Second)
                .AddMinutes(-otherDateTime.dateTime.Minute)
                .AddHours(-otherDateTime.dateTime.Hour)
                .AddDays(-(otherDateTime.dateTime.Day - 1))
                .AddMonths(-(otherDateTime.dateTime.Month - 1))
                .AddYears(-(otherDateTime.dateTime.Year - 1));

            // Ensure DateTime doesn't go below MinValue
            if (newDateTime < DateTime.MinValue)
            {
                newDateTime = DateTime.MinValue;
            }

            return (new VDateTime(newDateTime), ErrorNull.Instance);
        }

        return (ValueNull.Instance, IlligalOperation(other));
    }

    public override string ToString()
    {
        return dateTime.ToString("MM-dd-yyyyTHH:mm:ss.fffffff");
    }

    public override VDateTime Copy()
    {
        VDateTime copy = new(dateTime);
        copy.SetPos(startPos, endPos);
        copy.SetContext(context);
        return copy;
    }
}
