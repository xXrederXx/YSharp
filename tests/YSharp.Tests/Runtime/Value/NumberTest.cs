using Xunit;
using YSharp.Common;
using YSharp.Runtime;
using YSharp.Runtime.Collections.List;
using YSharp.Runtime.Primitives.Bool;
using YSharp.Runtime.Primitives.Number;
using YSharp.Runtime.Primitives.String;

namespace YSharp.Tests;

public class NumberTest
{
    [Fact]
    void checkToString()
    {
        Assert.False(string.IsNullOrEmpty(new VNumber(7).ToString()));
    }

    [Fact]
    void checkAdd_whenValid()
    {
        Result<Value, Error> result = new VNumber(5).AddedTo(new VNumber(2));

        Assert.True(result.TryGetValue(out Value value));
        VNumber castValue = Assert.IsType<VNumber>(value);
        Assert.Equal(7, castValue.value, TestingConstans.DOUBLE_PRECISION);
    }

    [Fact]
    void checkAdd_whenInvalid()
    {
        Result<Value, Error> result = new VNumber(5).AddedTo(new VString("test"));

        Assert.True(result.TryGetError(out Error error));
        Assert.IsType<IllegalOperationError>(error);
    }

    [Fact]
    void checkSub_whenValid()
    {
        Result<Value, Error> result = new VNumber(5).SubedTo(new VNumber(2));

        Assert.True(result.TryGetValue(out Value value));
        VNumber castValue = Assert.IsType<VNumber>(value);
        Assert.Equal(3, castValue.value, TestingConstans.DOUBLE_PRECISION);
    }

    [Fact]
    void checkSub_whenInvalid()
    {
        Result<Value, Error> result = new VNumber(5).SubedTo(new VString("test"));

        Assert.True(result.TryGetError(out Error error));
        Assert.IsType<IllegalOperationError>(error);
    }

    [Fact]
    void checkMul_whenValid()
    {
        Result<Value, Error> result = new VNumber(5).MuledTo(new VNumber(2));

        Assert.True(result.TryGetValue(out Value value));
        VNumber castValue = Assert.IsType<VNumber>(value);
        Assert.Equal(10, castValue.value, TestingConstans.DOUBLE_PRECISION);
    }

    [Fact]
    void checkMul_whenInvalid()
    {
        Result<Value, Error> result = new VNumber(5).MuledTo(new VString("test"));

        Assert.True(result.TryGetError(out Error error));
        Assert.IsType<IllegalOperationError>(error);
    }

    [Fact]
    void checkDiv_whenValid()
    {
        Result<Value, Error> result = new VNumber(5).DivedTo(new VNumber(2));

        Assert.True(result.TryGetValue(out Value value));
        VNumber castValue = Assert.IsType<VNumber>(value);
        Assert.Equal(2.5, castValue.value, TestingConstans.DOUBLE_PRECISION);
    }

    [Fact]
    void checkDiv_when0_returnError()
    {
        Result<Value, Error> result = new VNumber(5).DivedTo(new VNumber(0));

        Assert.True(result.TryGetError(out Error error));
        Assert.IsType<DivisionByZeroError>(error);
    }

    [Fact]
    void checkDiv_whenInvalid()
    {
        Result<Value, Error> result = new VNumber(5).DivedTo(new VString("test"));

        Assert.True(result.TryGetError(out Error error));
        Assert.IsType<IllegalOperationError>(error);
    }

    [Fact]
    void checkPow_whenValid()
    {
        Result<Value, Error> result = new VNumber(5).PowedTo(new VNumber(2));

        Assert.True(result.TryGetValue(out Value value));
        VNumber castValue = Assert.IsType<VNumber>(value);
        Assert.Equal(25, castValue.value, TestingConstans.DOUBLE_PRECISION);
    }

    [Fact]
    void checkPow_whenInvalid()
    {
        Result<Value, Error> result = new VNumber(5).MuledTo(new VString("test"));

        Assert.True(result.TryGetError(out Error error));
        Assert.IsType<IllegalOperationError>(error);
    }

    [Theory]
    [InlineData(1, 3, false)]
    [InlineData(2, 2, true)]
    [InlineData(3, 1, false)]
    void checkEqual(double x, double y, bool expected)
    {
        Result<Value, Error> result = new VNumber(x).GetComparisonEQ(new VNumber(y));

        Assert.True(result.TryGetValue(out Value value));
        VBool castValue = Assert.IsType<VBool>(value);
        Assert.Equal(expected, castValue.value);
    }

    [Fact]
    void checkEqual_whenInvalid()
    {
        Result<Value, Error> result = new VNumber(5).GetComparisonEQ(new VString("test"));

        Assert.True(result.TryGetError(out Error error));
        Assert.IsType<IllegalOperationError>(error);
    }

    [Theory]
    [InlineData(1, 3, true)]
    [InlineData(2, 2, false)]
    [InlineData(3, 1, true)]
    void checkNotEqual(double x, double y, bool expected)
    {
        Result<Value, Error> result = new VNumber(x).GetComparisonNE(new VNumber(y));

        Assert.True(result.TryGetValue(out Value value));
        VBool castValue = Assert.IsType<VBool>(value);
        Assert.Equal(expected, castValue.value);
    }

    [Fact]
    void checkNotEqual_whenInvalid()
    {
        Result<Value, Error> result = new VNumber(5).GetComparisonNE(new VString("test"));

        Assert.True(result.TryGetError(out Error error));
        Assert.IsType<IllegalOperationError>(error);
    }

    [Theory]
    [InlineData(1, 3, false)]
    [InlineData(2, 2, false)]
    [InlineData(3, 1, true)]
    void checkGreater(double x, double y, bool expected)
    {
        Result<Value, Error> result = new VNumber(x).GetComparisonGT(new VNumber(y));

        Assert.True(result.TryGetValue(out Value value));
        VBool castValue = Assert.IsType<VBool>(value);
        Assert.Equal(expected, castValue.value);
    }

    [Fact]
    void checkGreater_whenInvalid()
    {
        Result<Value, Error> result = new VNumber(5).GetComparisonGT(new VString("test"));

        Assert.True(result.TryGetError(out Error error));
        Assert.IsType<IllegalOperationError>(error);
    }

    [Theory]
    [InlineData(1, 3, false)]
    [InlineData(2, 2, true)]
    [InlineData(3, 1, true)]
    void checkGreaterOrEqual(double x, double y, bool expected)
    {
        Result<Value, Error> result = new VNumber(x).GetComparisonGTE(new VNumber(y));

        Assert.True(result.TryGetValue(out Value value));
        VBool castValue = Assert.IsType<VBool>(value);
        Assert.Equal(expected, castValue.value);
    }

    [Fact]
    void checkGreaterOrEqual_whenInvalid()
    {
        Result<Value, Error> result = new VNumber(5).GetComparisonGTE(new VString("test"));

        Assert.True(result.TryGetError(out Error error));
        Assert.IsType<IllegalOperationError>(error);
    }

    [Theory]
    [InlineData(1, 3, true)]
    [InlineData(2, 2, false)]
    [InlineData(3, 1, false)]
    void checkLess(double x, double y, bool expected)
    {
        Result<Value, Error> result = new VNumber(x).GetComparisonLT(new VNumber(y));

        Assert.True(result.TryGetValue(out Value value));
        VBool castValue = Assert.IsType<VBool>(value);
        Assert.Equal(expected, castValue.value);
    }

    [Fact]
    void checkLess_whenInvalid()
    {
        Result<Value, Error> result = new VNumber(5).GetComparisonLT(new VString("test"));

        Assert.True(result.TryGetError(out Error error));
        Assert.IsType<IllegalOperationError>(error);
    }

    [Theory]
    [InlineData(1, 3, true)]
    [InlineData(2, 2, true)]
    [InlineData(3, 1, false)]
    void checkLessOrEqual(double x, double y, bool expected)
    {
        Result<Value, Error> result = new VNumber(x).GetComparisonLTE(new VNumber(y));

        Assert.True(result.TryGetValue(out Value value));
        VBool castValue = Assert.IsType<VBool>(value);
        Assert.Equal(expected, castValue.value);
    }

    [Fact]
    void checkLessOrEqual_whenInvalid()
    {
        Result<Value, Error> result = new VNumber(5).GetComparisonLTE(new VString("test"));

        Assert.True(result.TryGetError(out Error error));
        Assert.IsType<IllegalOperationError>(error);
    }

    [Theory]
    [InlineData(0, false)]
    [InlineData(1, true)]
    void checkToBool(double x, bool expected)
    {
        Result<Value, Error> result = new VNumber(x).GetFunc(TestingConstans.MakeToken("ToBool"), []);

        Assert.True(result.TryGetValue(out Value value));
        VBool castValue = Assert.IsType<VBool>(value);
        Assert.Equal(expected, castValue.value);
    }

    [Fact]
    void checkToBool_whenInvalid()
    {
        Result<Value, Error> result = new VNumber(6.7).GetFunc(TestingConstans.MakeToken("ToBool"), [new Value()]);

        Assert.True(result.TryGetError(out Error error));
        Assert.IsType<NumArgsError>(error);
    }

    [Fact]
    void checkToString_whenInvalid()
    {
        Result<Value, Error> result = new VNumber(6.7).GetFunc(TestingConstans.MakeToken("ToString"), [new VNumber(1)]);

        Assert.True(result.TryGetError(out Error error));
        Assert.IsType<WrongFormatError>(error);
    }

    [Fact]
    void checkToString_whenNoArg()
    {
        Result<Value, Error> result = new VNumber(6.7).GetFunc(TestingConstans.MakeToken("ToString"), []);

        Assert.True(result.TryGetValue(out Value value));
        VString castValue = Assert.IsType<VString>(value);
        Assert.Equal("6.7", castValue.value);
    }

    [Theory(Skip = "Issue#19")]
    [InlineData(16, "H", "F")]
    void checkToString_whenValidFormattArg(double number, string format, string expected)
    {
        Result<Value, Error> result = new VNumber(number).GetFunc(
            TestingConstans.MakeToken("ToString"),
            [new VString(format)]
        );

        Assert.True(result.TryGetValue(out Value value));
        VString castValue = Assert.IsType<VString>(value);
        Assert.Equal(expected, castValue.value);
    }
}
