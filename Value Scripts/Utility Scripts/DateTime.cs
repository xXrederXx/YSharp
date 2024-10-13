namespace YSharp_2._0;

public class DateTime(System.DateTime? dateTime = null) : Value
{
    public System.DateTime dateTime = dateTime ?? System.DateTime.Now;

    public override ValueError GetVar(string name)
    {
        return name switch
        {
            "Microsecond" => (ValueError)(new Number(dateTime.Microsecond), NoError.Instance),// Microseconds approximation
            "Millisecond" => (ValueError)(new Number(dateTime.Millisecond), NoError.Instance),
            "Second" => (ValueError)(new Number(dateTime.Second), NoError.Instance),
            "Minute" => (ValueError)(new Number(dateTime.Minute), NoError.Instance),
            "Hour" => (ValueError)(new Number(dateTime.Hour), NoError.Instance),
            "DayOfMonth" => (ValueError)(new Number(dateTime.Day), NoError.Instance),
            "DayOfWeek" => (ValueError)(new String(dateTime.DayOfWeek.ToString()), NoError.Instance),
            "DayOfYear" => (ValueError)(new Number(dateTime.DayOfYear), NoError.Instance),
            "Month" => (ValueError)(new Number(dateTime.Month), NoError.Instance),
            "Year" => (ValueError)(new Number(dateTime.Year), NoError.Instance),
            _ => base.GetVar(name),
        };
    }

    public override ValueError addedTo(Value other)
    {
        if (other is DateTime otherDateTime)
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

            return (new DateTime(newDateTime), NoError.Instance);
        }

        return (ValueNull.Instance, IlligalOperation(other));
    }

    public override ValueError subedTo(Value other)
    {
        if (other is DateTime otherDateTime)
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

            return (new DateTime(newDateTime), NoError.Instance);
        }

        return (ValueNull.Instance, IlligalOperation(other));
    }

    public override string ToString()
    {
        return dateTime.ToString("MM-dd-yyyyTHH:mm:ss.fffffff");
    }

    public override DateTime copy()
    {
        DateTime copy = new(dateTime);
        copy.SetPos(startPos, endPos);
        copy.SetContext(context);
        return copy;
    }
}

