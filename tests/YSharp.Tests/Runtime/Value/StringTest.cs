using Xunit;
using YSharp.Common;
using YSharp.Runtime;
using YSharp.Runtime.Collections.List;
using YSharp.Runtime.Primitives.Bool;
using YSharp.Runtime.Primitives.Number;
using YSharp.Runtime.Primitives.String;

namespace YSharp.Tests;

public class StringTest
{
    [Fact]
    void checkToString()
    {
        Assert.False(string.IsNullOrEmpty(GetString("hi").ToString()));
    }

    [Fact]
    void checkAddedTo()
    {
        Result<Value, Error> result = GetString("hi").AddedTo(GetString(" there"));

        Assert.True(result.TryGetValue(out Value value));
        VString str = Assert.IsType<VString>(value);
        Assert.Equal("hi there", str.value);
    }

    [Fact]
    void checkAddedTo_InvalidType()
    {
        Result<Value, Error> result = GetString("hi").AddedTo(new VNumber(5));

        Assert.True(result.TryGetError(out Error error));
        Assert.IsType<IllegalOperationError>(error);
    }

    [Fact]
    void checkComparisonEQ_SameReference()
    {
        VString str = GetString("hi");
        Result<Value, Error> result = str.GetComparisonEQ(str);

        Assert.True(result.TryGetValue(out Value value));
        VBool boolean = Assert.IsType<VBool>(value);
        Assert.True(boolean.value);
    }

    [Fact]
    void checkComparisonEQ_SameValue()
    {
        Result<Value, Error> result = GetString("hi").GetComparisonEQ(GetString("hi"));

        Assert.True(result.TryGetValue(out Value value));
        VBool boolean = Assert.IsType<VBool>(value);
        Assert.True(boolean.value);
    }

    [Fact]
    void checkComparisonEQ_DifferentValue()
    {
        Result<Value, Error> result = GetString("hi").GetComparisonEQ(GetString("bye"));

        Assert.True(result.TryGetValue(out Value value));
        VBool boolean = Assert.IsType<VBool>(value);
        Assert.False(boolean.value);
    }

    [Fact]
    void checkComparisonEQ_InvalidType()
    {
        Result<Value, Error> result = GetString("hi").GetComparisonEQ(new VNumber(1));

        Assert.True(result.TryGetError(out Error error));
        Assert.IsType<IllegalOperationError>(error);
    }

    [Fact]
    void checkComparisonNE_SameValue()
    {
        Result<Value, Error> result = GetString("hi").GetComparisonNE(GetString("hi"));

        Assert.True(result.TryGetValue(out Value value));
        VBool boolean = Assert.IsType<VBool>(value);
        Assert.False(boolean.value);
    }

    [Fact]
    void checkComparisonNE_DifferentValue()
    {
        Result<Value, Error> result = GetString("hi").GetComparisonNE(GetString("bye"));

        Assert.True(result.TryGetValue(out Value value));
        VBool boolean = Assert.IsType<VBool>(value);
        Assert.True(boolean.value);
    }

    [Fact]
    void checkComparisonNE_InvalidType()
    {
        Result<Value, Error> result = GetString("hi").GetComparisonNE(new VNumber(1));

        Assert.True(result.TryGetError(out Error error));
        Assert.IsType<IllegalOperationError>(error);
    }

    [Fact]
    void checkMuledTo_TimesTwo()
    {
        Result<Value, Error> result = GetString("a").MuledTo(new VNumber(2));

        Assert.True(result.TryGetValue(out Value value));
        VString str = Assert.IsType<VString>(value);
        Assert.Equal("aa", str.value);
    }

    [Fact]
    void checkMuledTo_TimesOne()
    {
        Result<Value, Error> result = GetString("a").MuledTo(new VNumber(1));

        Assert.True(result.TryGetValue(out Value value));
        VString str = Assert.IsType<VString>(value);
        Assert.Equal("a", str.value);
    }

    [Fact]
    void checkMuledTo_TimesZero()
    {
        Result<Value, Error> result = GetString("a").MuledTo(new VNumber(0));

        Assert.True(result.TryGetValue(out Value value));
        VString str = Assert.IsType<VString>(value);
        Assert.Equal(string.Empty, str.value);
    }

    [Fact]
    void checkMuledTo_InvalidType()
    {
        Result<Value, Error> result = GetString("a").MuledTo(GetString("b"));

        Assert.True(result.TryGetError(out Error error));
        Assert.IsType<IllegalOperationError>(error);
    }

    [Fact]
    void checkIsTrue_whenEmpty_returnFalse()
    {
        Assert.False(GetString("").IsTrue());
    }

    [Fact]
    void checkIsTrue_whenNotEmpty_returnTrue()
    {
        Assert.True(GetString("hi").IsTrue());
    }

    [Fact]
    void checkSplit_basic()
    {
        Result<Value, Error> result = GetString("a,b,c").GetFunc(TestingConstans.MakeToken("Split"), [new VString(",")]);

        Assert.True(result.TryGetValue(out Value value));
        VList list = Assert.IsType<VList>(value);
        Assert.Equal(3, list.value.Count);
    }

    [Fact]
    void checkSplit_invalidArgs()
    {
        Result<Value, Error> result = GetString("a,b").GetFunc(TestingConstans.MakeToken("Split"), []);

        Assert.True(result.TryGetError(out Error error));
        Assert.IsType<NumArgsError>(error);
    }

    [Fact]
    void checkToBool_true()
    {
        Result<Value, Error> result = GetString("hi").GetFunc(TestingConstans.MakeToken("ToBool"), []);

        Assert.True(result.TryGetValue(out Value value));
        VBool boolean = Assert.IsType<VBool>(value);
        Assert.True(boolean.value);
    }

    [Fact]
    void checkToBool_false()
    {
        Result<Value, Error> result = GetString("").GetFunc(TestingConstans.MakeToken("ToBool"), []);

        Assert.True(result.TryGetValue(out Value value));
        VBool boolean = Assert.IsType<VBool>(value);
        Assert.False(boolean.value);
    }

    [Fact]
    void checkToLower_success()
    {
        Result<Value, Error> result = GetString("HI").GetFunc(TestingConstans.MakeToken("ToLower"), []);

        Assert.True(result.TryGetValue(out Value value));
        VString str = Assert.IsType<VString>(value);
        Assert.Equal("hi", str.value);
    }

    [Fact]
    void checkToUpper_success()
    {
        Result<Value, Error> result = GetString("hi").GetFunc(TestingConstans.MakeToken("ToUpper"), []);

        Assert.True(result.TryGetValue(out Value value));
        VString str = Assert.IsType<VString>(value);
        Assert.Equal("HI", str.value);
    }

    [Fact]
    void checkToLower_invalid()
    {
        Result<Value, Error> result = GetString("HI").GetFunc(TestingConstans.MakeToken("ToLower"), [new Value()]);

        Assert.True(result.TryGetError(out Error error));
        Assert.IsType<NumArgsError>(error);
    }

    [Fact]
    void checkToUpper_invalid()
    {
        Result<Value, Error> result = GetString("hi").GetFunc(TestingConstans.MakeToken("ToUpper"), [new Value()]);

        Assert.True(result.TryGetError(out Error error));
        Assert.IsType<NumArgsError>(error);
    }

    [Fact]
    void checkToBool_invalid()
    {
        Result<Value, Error> result = GetString("hi").GetFunc(TestingConstans.MakeToken("ToBool"), [new Value()]);

        Assert.True(result.TryGetError(out Error error));
        Assert.IsType<NumArgsError>(error);
    }

    [Fact]
    void checkToNumber_invalidArgs()
    {
        Result<Value, Error> result = GetString("hi").GetFunc(TestingConstans.MakeToken("ToNumber"), [new Value()]);

        Assert.True(result.TryGetError(out Error error));
        Assert.IsType<NumArgsError>(error);
    }

    [Fact]
    void checkToNumber_valid()
    {
        Result<Value, Error> result = GetString("123").GetFunc(TestingConstans.MakeToken("ToNumber"), []);

        Assert.True(result.TryGetValue(out Value value));
        VNumber num = Assert.IsType<VNumber>(value);
        Assert.Equal(123, num.value);
    }

    [Fact]
    void checkToNumber_invalidContent()
    {
        Result<Value, Error> result = GetString("abc").GetFunc(TestingConstans.MakeToken("ToNumber"), []);

        Assert.True(result.TryGetError(out Error error));
        Assert.IsType<WrongFormatError>(error);
    }

    [Fact]
    void checkLength_valid()
    {
        Result<Value, Error> result = GetString("123").GetVar(TestingConstans.MakeToken("Length"));

        Assert.True(result.TryGetValue(out Value value));
        VNumber num = Assert.IsType<VNumber>(value);
        Assert.Equal(3, num.value);
    }

    private VString GetString(string value)
    {
        return (VString)
            new VString(value).SetPos(Position.Null, Position.Null).SetContext(new Context());
    }
}
