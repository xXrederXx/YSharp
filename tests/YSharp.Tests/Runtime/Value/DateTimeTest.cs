using Xunit;
using YSharp.Common;
using YSharp.Runtime;
using YSharp.Runtime.Primitives.Datetime;
using YSharp.Runtime.Primitives.Number;
using YSharp.Runtime.Primitives.String;

namespace YSharp.Tests;

public class DateTimeTest
{
    const int testYear = 2025;
    const int testMonth = 4;
    const int testDay = 18;
    const int testHour = 13;
    const int testMinute = 31;
    const int testSecond = 21;
    const int testMillisecond = 670;
    const int testMicrosecond = 24;
    readonly DateTime testDateTime = new DateTime(
        testYear,
        testMonth,
        testDay,
        testHour,
        testMinute,
        testSecond,
        testMillisecond,
        testMicrosecond
    );

    [Theory]
    [InlineData("Year", testYear)]
    [InlineData("Month", testMonth)]
    [InlineData("DayOfMonth", testDay)]
    [InlineData("DayOfYear", 108)]
    [InlineData("Hour", testHour)]
    [InlineData("Minute", testMinute)]
    [InlineData("Second", testSecond)]
    [InlineData("Millisecond", testMillisecond)]
    [InlineData("Microsecond", testMicrosecond)]
    void checkNumberPropertyAccessors(string name, double expected)
    {
        VDateTime vDateTime = new VDateTime(testDateTime);

        Result<Value, Error> result = vDateTime.GetVar(TestingConstans.MakeToken(name));
        Assert.True(result.TryGetValue(out Value value));
        VNumber number = Assert.IsType<VNumber>(value);
        Assert.Equal(expected, number.value, TestingConstans.DOUBLE_PRECISION);
    }

    [Fact]
    void checkDayOfWeekPropertyAccessors()
    {
        VDateTime vDateTime = new VDateTime(testDateTime);

        Result<Value, Error> result = vDateTime.GetVar(TestingConstans.MakeToken("DayOfWeek"));
        Assert.True(result.TryGetValue(out Value value));
        VString dayString = Assert.IsType<VString>(value);
        Assert.Equal("Friday", dayString.value);
    }

    [Fact]
    void checkAdding_whenValid()
    {
        Result<Value, Error> result = new VDateTime(testDateTime).AddedTo(
            new VDateTime(testDateTime)
        );
        Assert.True(result.TryGetValue(out Value value));
        VDateTime date = Assert.IsType<VDateTime>(value);
        Assert.Equal(new DateTime(testDateTime.Ticks * 2), date.dateTime, TestingConstans.TIME_PRECISION);
    }

    [Fact]
    void checkAdding_whenInalid()
    {
        Result<Value, Error> result = new VDateTime(testDateTime).AddedTo(new VString("test"));
        Assert.True(result.TryGetError(out Error error));
        Assert.IsType<IllegalOperationError>(error);
    }

    [Fact]
    void checkSubstracting_whenValid()
    {
        Result<Value, Error> result = new VDateTime(testDateTime).SubedTo(
            new VDateTime(testDateTime)
        );
        Assert.True(result.TryGetValue(out Value value));
        VDateTime date = Assert.IsType<VDateTime>(value);
        Assert.Equal(new DateTime(0), date.dateTime, TestingConstans.TIME_PRECISION);
    }

    [Fact]
    void checkSubstracting_whenInalid()
    {
        Result<Value, Error> result = new VDateTime(testDateTime).SubedTo(new VString("test"));
        Assert.True(result.TryGetError(out Error error));
        Assert.IsType<IllegalOperationError>(error);
    }

    [Fact]
    void checkToString_returnNonBlank()
    {
        Assert.False(string.IsNullOrEmpty(new VDateTime(testDateTime).ToString()));
    }

    [Fact]
    void checkCopy_returnEqual()
    {
        VDateTime left = new VDateTime(testDateTime);
        VDateTime right = left.Copy();

        Assert.Equal(left.dateTime, right.dateTime, TestingConstans.TIME_PRECISION);
        Assert.Equal(left.StartPos, right.StartPos);
        Assert.Equal(left.EndPos, right.EndPos);
        Assert.Equal(left.Context, right.Context);
    }

    [Fact]
    void checDefaultCtor_initalizesDateToNow()
    {
        VDateTime left = new VDateTime();

        Assert.Equal(DateTime.Now, left.dateTime, TestingConstans.TIME_PRECISION);
    }
}
