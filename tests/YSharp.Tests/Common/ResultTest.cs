using System;
using Xunit;
using YSharp.Common;

namespace YSharp.Tests;

public class ResultTests
{
    [Fact]
    public void Success_ShouldCreateSuccessfulResult()
    {
        Result<int, string> result = Result<int, string>.Success(42);

        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailed);
        Assert.Equal(42, result.GetValue());
    }

    [Fact]
    public void Fail_ShouldCreateFailedResult()
    {
        Result<int, string> result = Result<int, string>.Fail("error");

        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailed);
        Assert.Equal("error", result.GetError());
    }

    [Fact]
    public void TryGetValue_OnSuccess_ShouldReturnTrueAndValue()
    {
        Result<int, string> result = Result<int, string>.Success(10);

        Assert.True(result.TryGetValue(out int value));
        Assert.Equal(10, value);
    }

    [Fact]
    public void TryGetValue_OnFailure_ShouldReturnFalse()
    {
        Result<int, string> result = Result<int, string>.Fail("err");

        Assert.False(result.TryGetValue(out int value));
        Assert.Equal(default, value);
    }

    [Fact]
    public void Deconstruct_ShouldReturnCorrectValues_OnSuccess()
    {
        Result<int, string> result = Result<int, string>.Success(5);

        (bool isSuccess, int value, string? error) = result;

        Assert.True(isSuccess);
        Assert.Equal(5, value);
        Assert.Null(error);
    }

    [Fact]
    public void Deconstruct_ShouldReturnCorrectValues_OnFailure()
    {
        Result<int, string> result = Result<int, string>.Fail("fail");

        (bool isSuccess, int value, string? error) = result;

        Assert.False(isSuccess);
        Assert.Equal(0, value);
        Assert.Equal("fail", error);
    }

    [Fact]
    public void GetValue_OnFailure_ShouldThrow()
    {
        Result<int, string> result = Result<int, string>.Fail("error");

        Assert.Throws<InvalidOperationException>(() => result.GetValue());
    }

    [Fact]
    public void GetError_OnSuccess_ShouldThrow()
    {
        Result<int, string> result = Result<int, string>.Success(1);

        Assert.Throws<InvalidOperationException>(result.GetError);
    }

    [Fact]
    public void Success_WithNull_ShouldThrow()
    {
        Assert.Throws<ArgumentNullException>(() => Result<string, string>.Success(null!));
    }

    [Fact]
    public void Fail_WithNull_ShouldThrow()
    {
        Assert.Throws<ArgumentNullException>(() => Result<string, string>.Fail(null!));
    }

    [Fact]
    public void EqualResults_ShouldBeEqual()
    {
        Result<int, string> r1 = Result<int, string>.Success(5);
        Result<int, string> r2 = Result<int, string>.Success(5);

        Assert.True(r1 == r2);
        Assert.Equal(r1, r2);
    }

    [Fact]
    public void DifferentValues_ShouldNotBeEqual()
    {
        Result<int, string> r1 = Result<int, string>.Success(5);
        Result<int, string> r2 = Result<int, string>.Success(10);

        Assert.True(r1 != r2);
    }

    [Fact]
    public void SuccessAndFail_ShouldNotBeEqual()
    {
        Result<int, string> success = Result<int, string>.Success(1);
        Result<int, string> fail = Result<int, string>.Fail("err");

        Assert.NotEqual(success, fail);
    }

    [Fact]
    public void EqualFailures_ShouldBeEqual()
    {
        Result<int, string> r1 = Result<int, string>.Fail("err");
        Result<int, string> r2 = Result<int, string>.Fail("err");

        Assert.Equal(r1, r2);
    }

    [Fact]
    public void EqualFailuresCasted_ShouldBeEqual()
    {
        Result<int, string> r1 = Result<int, string>.Fail("err");
        object r2 = Result<int, string>.Fail("err");

        Assert.Equal(r1, r2);
    }

    [Fact]
    public void NotEqualFailuresCasted_ShouldBeNotEqual()
    {
        Result<int, string> r1 = Result<int, string>.Fail("err");
        object r2 = "err";

        Assert.NotEqual(r1, r2);
    }

    [Fact]
    public void ToString_OnSuccess_ShouldContainValue()
    {
        Result<int, string> result = Result<int, string>.Success(123);

        string str = result.ToString();

        Assert.Contains("Success", str);
        Assert.Contains("123", str);
    }

    [Fact]
    public void ToString_OnFailure_ShouldContainError()
    {
        Result<int, string> result = Result<int, string>.Fail("oops");

        string str = result.ToString();

        Assert.Contains("Fail", str);
        Assert.Contains("oops", str);
    }

    [Fact]
    public void GetHashCode_ShouldBeSame_ForEqualResults()
    {
        Result<int, string> r1 = Result<int, string>.Success(7);
        Result<int, string> r2 = Result<int, string>.Success(7);

        Assert.Equal(r1.GetHashCode(), r2.GetHashCode());
    }

    [Fact]
    public void GetHashCode_ShouldBeDifferent_ForNonEqualResults()
    {
        Result<int, string> r1 = Result<int, string>.Success(7);
        Result<int, string> r2 = Result<int, string>.Fail("err");

        Assert.NotEqual(r1.GetHashCode(), r2.GetHashCode());
    }

    [Fact]
    public void DefaultStruct_ShouldBehaveAsFailed()
    {
        Result<int, string> result = default;

        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailed);

        Assert.Throws<InvalidOperationException>(() => result.GetValue());
    }
}
