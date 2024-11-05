namespace YSharp_2._0;

public class VDateTime(System.DateTime? dateTime = null) : Value
{
    public System.DateTime dateTime = dateTime ?? System.DateTime.Now;

    public override ValueError GetVar(string name)
    {
        return name switch
        {
            "Microsecond" => (ValueError)(new VNumber(dateTime.Microsecond), ErrorNull.Instance),// Microseconds approximation
            "Millisecond" => (ValueError)(new VNumber(dateTime.Millisecond), ErrorNull.Instance),
            "Second" => (ValueError)(new VNumber(dateTime.Second), ErrorNull.Instance),
            "Minute" => (ValueError)(new VNumber(dateTime.Minute), ErrorNull.Instance),
            "Hour" => (ValueError)(new VNumber(dateTime.Hour), ErrorNull.Instance),
            "DayOfMonth" => (ValueError)(new VNumber(dateTime.Day), ErrorNull.Instance),
            "DayOfWeek" => (ValueError)(new VString(dateTime.DayOfWeek.ToString()), ErrorNull.Instance),
            "DayOfYear" => (ValueError)(new VNumber(dateTime.DayOfYear), ErrorNull.Instance),
            "Month" => (ValueError)(new VNumber(dateTime.Month), ErrorNull.Instance),
            "Year" => (ValueError)(new VNumber(dateTime.Year), ErrorNull.Instance),
            _ => base.GetVar(name),
        };
    }

    public override ValueError AddedTo(Value other)
    {
        if (other is VDateTime otherDateTime)
        {
            System.DateTime newDateTime = dateTime
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

    public override ValueError SubedTo(Value other)
    {
        if (other is VDateTime otherDateTime)
        {
            System.DateTime newDateTime = dateTime
                .AddMicroseconds(-otherDateTime.dateTime.Microsecond) // Microseconds approximation
                .AddMilliseconds(-otherDateTime.dateTime.Millisecond)
                .AddSeconds(-otherDateTime.dateTime.Second)
                .AddMinutes(-otherDateTime.dateTime.Minute)
                .AddHours(-otherDateTime.dateTime.Hour)
                .AddDays(-(otherDateTime.dateTime.Day - 1))
                .AddMonths(-(otherDateTime.dateTime.Month - 1))
                .AddYears(-(otherDateTime.dateTime.Year - 1));

            // Ensure DateTime doesn't go below MinValue
            if (newDateTime < System.DateTime.MinValue)
            {
                newDateTime = System.DateTime.MinValue;
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

