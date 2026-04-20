using System;
using Xunit;
using YSharp.Common;

namespace YSharp.Tests;

public class ResultTests
{
    [Fact]
    public void checkResult_whenSuccess_thenCreatesSuccessfulResult()
    {
        Result<int, string> result = Result<int, string>.Success(42);

        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailed);
        Assert.Equal(42, result.GetValue());
    }

    [Fact]
    public void checkResult_whenFail_thenCreatesFailedResult()
    {
        Result<int, string> result = Result<int, string>.Fail("error");

        Assert.False(result.IsSuccess);
        Assert.True(result.TryGetError(out string error));
        Assert.Equal("error", result.GetError());
    }

    [Fact]
    public void checkResult_whenTryGetValueOnSuccess_thenReturnsTrueAndValue()
    {
        Result<int, string> result = Result<int, string>.Success(10);

        Assert.True(result.TryGetValue(out int value));
        Assert.Equal(10, value);
    }

    [Fact]
    public void checkResult_whenTryGetValueOnFailure_thenReturnsFalse()
    {
        Result<int, string> result = Result<int, string>.Fail("err");

        Assert.False(result.TryGetValue(out int value));
        Assert.Equal(default, value);
    }

    [Fact]
    public void checkResult_whenDeconstructOnSuccess_thenReturnsCorrectValues()
    {
        Result<int, string> result = Result<int, string>.Success(5);

        (bool isSuccess, int value, string? error) = result;

        Assert.True(isSuccess);
        Assert.Equal(5, value);
        Assert.Null(error);
    }

    [Fact]
    public void checkResult_whenDeconstructOnFailure_thenReturnsCorrectValues()
    {
        Result<int, string> result = Result<int, string>.Fail("fail");

        (bool isSuccess, int value, string? error) = result;

        Assert.False(isSuccess);
        Assert.Equal(0, value);
        Assert.Equal("fail", error);
    }

    [Fact]
    public void checkResult_whenGetValueOnFailure_thenThrows()
    {
        Result<int, string> result = Result<int, string>.Fail("error");

        Assert.Throws<InvalidOperationException>(() => result.GetValue());
    }

    [Fact]
    public void checkResult_whenGetErrorOnSuccess_thenThrows()
    {
        Result<int, string> result = Result<int, string>.Success(1);

        Assert.Throws<InvalidOperationException>(result.GetError);
    }

    [Fact]
    public void checkResult_whenSuccessWithNull_thenThrows()
    {
        Assert.Throws<ArgumentNullException>(() => Result<string, string>.Success(null!));
    }

    [Fact]
    public void checkResult_whenFailWithNull_thenThrows()
    {
        Assert.Throws<ArgumentNullException>(() => Result<string, string>.Fail(null!));
    }

    [Fact]
    public void checkResult_whenEqualResults_thenTrue()
    {
        Result<int, string> r1 = Result<int, string>.Success(5);
        Result<int, string> r2 = Result<int, string>.Success(5);

        Assert.True(r1 == r2);
        Assert.Equal(r1, r2);
    }

    [Fact]
    public void checkResult_whenDifferentValues_thenNotEqual()
    {
        Result<int, string> r1 = Result<int, string>.Success(5);
        Result<int, string> r2 = Result<int, string>.Success(10);

        Assert.True(r1 != r2);
    }

    [Fact]
    public void checkResult_whenSuccessAndFail_thenNotEqual()
    {
        Result<int, string> success = Result<int, string>.Success(1);
        Result<int, string> fail = Result<int, string>.Fail("err");

        Assert.NotEqual(success, fail);
    }

    [Fact]
    public void checkResult_whenEqualFailures_thenEqual()
    {
        Result<int, string> r1 = Result<int, string>.Fail("err");
        Result<int, string> r2 = Result<int, string>.Fail("err");

        Assert.Equal(r1, r2);
    }

    [Fact]
    public void checkResult_whenEqualFailuresCasted_thenEqual()
    {
        Result<int, string> r1 = Result<int, string>.Fail("err");
        object r2 = Result<int, string>.Fail("err");

        Assert.Equal(r1, r2);
    }

    [Fact]
    public void checkResult_whenNotEqualFailuresCasted_thenNotEqual()
    {
        Result<int, string> r1 = Result<int, string>.Fail("err");
        object r2 = "err";

        Assert.NotEqual(r1, r2);
    }

    [Fact]
    public void checkResult_whenToStringOnSuccess_thenContainsValue()
    {
        Result<int, string> result = Result<int, string>.Success(123);

        string str = result.ToString();

        Assert.Contains("Success", str);
        Assert.Contains("123", str);
    }

    [Fact]
    public void checkResult_whenToStringOnFailure_thenContainsError()
    {
        Result<int, string> result = Result<int, string>.Fail("oops");

        string str = result.ToString();

        Assert.Contains("Fail", str);
        Assert.Contains("oops", str);
    }

    [Fact]
    public void checkResult_whenGetHashCodeForEqualResults_thenSame()
    {
        Result<int, string> r1 = Result<int, string>.Success(7);
        Result<int, string> r2 = Result<int, string>.Success(7);

        Assert.Equal(r1.GetHashCode(), r2.GetHashCode());
    }

    [Fact]
    public void checkResult_whenGetHashCodeForNonEqualResults_thenDifferent()
    {
        Result<int, string> r1 = Result<int, string>.Success(7);
        Result<int, string> r2 = Result<int, string>.Fail("err");

        Assert.NotEqual(r1.GetHashCode(), r2.GetHashCode());
    }

    [Fact]
    public void checkResult_whenDefaultStruct_thenBehavesAsFailed()
    {
        Result<int, string> result = default;

        Assert.False(result.IsSuccess);
        Assert.True(result.TryGetError(out string error));

        Assert.Throws<InvalidOperationException>(() => result.GetValue());
    }
}
