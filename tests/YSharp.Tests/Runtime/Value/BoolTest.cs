using Xunit;
using YSharp.Common;
using YSharp.Runtime;
using YSharp.Runtime.Collections.List;
using YSharp.Runtime.Primitives.Bool;
using YSharp.Runtime.Primitives.Number;
using YSharp.Runtime.Primitives.String;

namespace YSharp.Tests;

public class BoolTest
{
    [Fact]
    void checkToString()
    {
        Assert.False(string.IsNullOrEmpty(new VNumber(7).ToString()));
    }

    [Fact]
    void checkCopy_returnEqual()
    {
        VBool left = new VBool(true);
        VBool right = left.Copy();

        Assert.Equal(left.value, right.value);
        Assert.Equal(left.StartPos, right.StartPos);
        Assert.Equal(left.EndPos, right.EndPos);
        Assert.Equal(left.Context, right.Context);
    }

    [Theory]
    [InlineData(false, false, false)]
    [InlineData(true, false, false)]
    [InlineData(false, true, false)]
    [InlineData(true, true, true)]
    void checkAnd_whenValid(bool x, bool y, bool expected)
    {
        Result<Value, Error> result = new VBool(x).AndedTo(new VBool(y));

        Assert.True(result.TryGetValue(out Value value));
        VBool date = Assert.IsType<VBool>(value);
        Assert.Equal(expected, date.value);
    }

    [Fact]
    void checkAnd_whenInvalid()
    {
        Result<Value, Error> result = new VBool(true).AndedTo(new VString("test"));
        Assert.True(result.TryGetError(out Error error));
        Assert.IsType<IllegalOperationError>(error);
    }

    [Theory]
    [InlineData(false, false, false)]
    [InlineData(true, false, true)]
    [InlineData(false, true, true)]
    [InlineData(true, true, true)]
    void checkOr_whenValid(bool x, bool y, bool expected)
    {
        Result<Value, Error> result = new VBool(x).OredTo(new VBool(y));

        Assert.True(result.TryGetValue(out Value value));
        VBool date = Assert.IsType<VBool>(value);
        Assert.Equal(expected, date.value);
    }

    [Fact]
    void checkOr_whenInvalid()
    {
        Result<Value, Error> result = new VBool(true).OredTo(new VString("test"));
        Assert.True(result.TryGetError(out Error error));
        Assert.IsType<IllegalOperationError>(error);
    }

    [Theory]
    [InlineData(false, false, true)]
    [InlineData(true, false, false)]
    [InlineData(false, true, false)]
    [InlineData(true, true, true)]
    void checkEqual_whenValid(bool x, bool y, bool expected)
    {
        Result<Value, Error> result = new VBool(x).GetComparisonEQ(new VBool(y));

        Assert.True(result.TryGetValue(out Value value));
        VBool date = Assert.IsType<VBool>(value);
        Assert.Equal(expected, date.value);
    }

    [Fact]
    void checkEqual_whenInvalid()
    {
        Result<Value, Error> result = new VBool(true).GetComparisonEQ(new VString("test"));
        Assert.True(result.TryGetError(out Error error));
        Assert.IsType<IllegalOperationError>(error);
    }

    [Theory]
    [InlineData(false, false, false)]
    [InlineData(true, false, true)]
    [InlineData(false, true, true)]
    [InlineData(true, true, false)]
    void checkNotEqual_whenValid(bool x, bool y, bool expected)
    {
        Result<Value, Error> result = new VBool(x).GetComparisonNE(new VBool(y));

        Assert.True(result.TryGetValue(out Value value));
        VBool date = Assert.IsType<VBool>(value);
        Assert.Equal(expected, date.value);
    }

    [Fact]
    void checkNotEqual_whenInvalid()
    {
        Result<Value, Error> result = new VBool(true).GetComparisonNE(new VString("test"));
        Assert.True(result.TryGetError(out Error error));
        Assert.IsType<IllegalOperationError>(error);
    }
}
