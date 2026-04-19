using Xunit;
using YSharp.Common;
using YSharp.Runtime;
using YSharp.Runtime.Collections.List;
using YSharp.Runtime.Primitives.Bool;
using YSharp.Runtime.Primitives.Number;
using YSharp.Runtime.Primitives.String;

namespace YSharp.Tests;

public class ListTest
{
    [Fact]
    void checkToString()
    {
        Assert.False(string.IsNullOrEmpty(GetList([new VNumber(0), new VNumber(1)]).ToString()));
    }

    [Fact]
    void checkAddedTo()
    {
        Result<Value, Error> result = GetList().AddedTo(GetList());
        Assert.True(result.TryGetValue(out Value value));
        VList list = Assert.IsType<VList>(value);
        Assert.Equal(GetList().value.Count * 2, list.value.Count);
    }

    [Fact]
    void checkAddedTo_InvalidType()
    {
        Result<Value, Error> result = GetList().AddedTo(new VNumber(5));

        Assert.True(result.IsFailed);
        Assert.IsType<IllegalOperationError>(result.GetError());
    }

    [Fact]
    void checkComparisonEQ_SameReference()
    {
        VList list = GetList();
        Result<Value, Error> result = list.GetComparisonEQ(list);

        Assert.True(result.TryGetValue(out Value value));
        VBool boolean = Assert.IsType<VBool>(value);
        Assert.True(boolean.value);
    }

    [Fact]
    void checkComparisonEQ_DifferentInstances()
    {
        Result<Value, Error> result = GetList().GetComparisonEQ(GetList());

        Assert.True(result.TryGetValue(out Value value));
        VBool boolean = Assert.IsType<VBool>(value);
        Assert.False(boolean.value);
    }

    [Fact]
    void checkComparisonEQ_InvalidType()
    {
        Result<Value, Error> result = GetList().GetComparisonEQ(new VNumber(1));

        Assert.True(result.IsFailed);
        Assert.IsType<IllegalOperationError>(result.GetError());
    }

    [Fact]
    void checkComparisonNE_SameReference()
    {
        VList list = GetList();
        Result<Value, Error> result = list.GetComparisonNE(list);

        Assert.True(result.TryGetValue(out Value value));
        VBool boolean = Assert.IsType<VBool>(value);
        Assert.False(boolean.value);
    }

    [Fact]
    void checkComparisonNE_DifferentInstances()
    {
        Result<Value, Error> result = GetList().GetComparisonNE(GetList());

        Assert.True(result.TryGetValue(out Value value));
        VBool boolean = Assert.IsType<VBool>(value);
        Assert.True(boolean.value);
    }

    [Fact]
    void checkComparisonNE_InvalidType()
    {
        Result<Value, Error> result = GetList().GetComparisonNE(new VNumber(1));

        Assert.True(result.IsFailed);
        Assert.IsType<IllegalOperationError>(result.GetError());
    }

    [Fact]
    void checkMuledTo_TimesTwo()
    {
        VList list = GetList();
        int originalCount = list.value.Count;

        Result<Value, Error> result = list.MuledTo(new VNumber(2));

        Assert.True(result.TryGetValue(out Value value));
        VList newList = Assert.IsType<VList>(value);
        Assert.Equal(originalCount * 2, newList.value.Count);
    }

    [Fact]
    void checkMuledTo_TimesOne()
    {
        VList list = GetList();
        int originalCount = list.value.Count;

        Result<Value, Error> result = list.MuledTo(new VNumber(1));

        Assert.True(result.TryGetValue(out Value value));
        VList newList = Assert.IsType<VList>(value);
        Assert.Equal(originalCount, newList.value.Count);
    }

    [Fact]
    void checkMuledTo_TimesZero()
    {
        Result<Value, Error> result = GetList().MuledTo(new VNumber(0));

        Assert.True(result.TryGetValue(out Value value));
        VList newList = Assert.IsType<VList>(value);
        Assert.Empty(newList.value);
    }

    [Fact(Skip = "Issue#41")]
    void checkMuledTo_Negative()
    {
        Result<Value, Error> result = GetList().MuledTo(new VNumber(-3));

        Assert.True(result.IsFailed);
        Assert.IsType<IllegalOperationError>(result.GetError());
    }

    [Fact]
    void checkMuledTo_InvalidType()
    {
        Result<Value, Error> result = GetList().MuledTo(GetList());

        Assert.True(result.IsFailed);
        Assert.IsType<IllegalOperationError>(result.GetError());
    }

    [Fact]
    void checkAdd_success()
    {
        VNumber number = new(1);
        VList vList = GetList();
        Result<Value, Error> result = vList.GetFunc("Add", [number]);

        Assert.True(result.IsSuccess);
        Assert.Equal(number, vList.value.Last());
    }

    [Fact]
    void checkGet_whenValidId_success()
    {
        VNumber number = new(1);
        Result<Value, Error> result = GetList([number]).GetFunc("Get", [new VNumber(0)]);

        Assert.True(result.TryGetValue(out Value value));
        VNumber newList = Assert.IsType<VNumber>(value);
        Assert.Equal(1, newList.value);
    }

    [Fact]
    void checkGet_whenNegativValidId_success()
    {
        VNumber number = new(1);
        Result<Value, Error> result = GetList([number]).GetFunc("Get", [new VNumber(-1)]);

        Assert.True(result.TryGetValue(out Value value));
        VNumber newList = Assert.IsType<VNumber>(value);
        Assert.Equal(1, newList.value);
    }

    [Fact]
    void checkGet_whenIdTooBig_error()
    {
        VNumber number = new(1);
        Result<Value, Error> result = GetList([number]).GetFunc("Get", [new VNumber(10)]);

        Assert.True(result.IsFailed);
        Assert.IsType<ArgOutOfRangeError>(result.GetError());
    }

    [Fact]
    void checkGet_whenIdTooSmall_error()
    {
        VNumber number = new(1);
        Result<Value, Error> result = GetList([number]).GetFunc("Get", [new VNumber(-10)]);

        Assert.True(result.IsFailed);
        Assert.IsType<ArgOutOfRangeError>(result.GetError());
    }

    [Fact]
    void checkGet_whenIdInvalidType_error()
    {
        VNumber number = new(1);
        Result<Value, Error> result = GetList([number]).GetFunc("Get", [new VString("0")]);

        Assert.True(result.IsFailed);
        Assert.IsType<WrongFormatError>(result.GetError());
    }

    [Fact]
    void checkGetLength_success()
    {
        Result<Value, Error> result = GetList([new VString("0")]).GetVar("Length");

        Assert.True(result.TryGetValue(out Value value));
        VNumber count = Assert.IsType<VNumber>(value);
        Assert.Equal(1, count.value);
    }

    [Fact]
    void checkIsTrue_whenEmpty_returnFalse()
    {
        Assert.False(GetList().IsTrue());
    }

    [Fact]
    void checkIsTrue_whenNotEmpty_returnTrue()
    {
        Assert.True(GetList([new VString("0")]).IsTrue());
    }

    [Fact]
    void checkIndexOf_whenNoElement_returnNegativ1()
    {
        Result<Value, Error> result = GetList().GetFunc("IndexOf", [new VNumber(7)]);

        Assert.True(result.TryGetValue(out Value value));
        VNumber index = Assert.IsType<VNumber>(value);
        Assert.Equal(-1, index.value);
    }

    [Fact]
    void checkIndexOf_whenInvalidArg_returnError()
    {
        Result<Value, Error> result = GetList()
            .GetFunc("IndexOf", [new VNumber(7), new VNumber(6)]);

        Assert.True(result.IsFailed);
        Assert.IsType<NumArgsError>(result.GetError());
    }

    public static TheoryData<Value> IndexOfData()
    {
        return new TheoryData<Value>([
            new VNumber(7),
            new VString("hi"),
            new VBool(true),
            new VList([]),
        ]);
    }

    [Theory]
    [MemberData(nameof(IndexOfData))]
    void checkIndexOf_whenElements_returnIndex(Value testItem)
    {
        // new Value just for testing that it checks types
        Result<Value, Error> result = GetList([new Value(), testItem])
            .GetFunc("IndexOf", [testItem]);

        Assert.True(result.TryGetValue(out Value value));
        VNumber index = Assert.IsType<VNumber>(value);
        Assert.Equal(1, index.value);
    }

    [Fact]
    void checkRemove_whenValidId()
    {
        VList vList = GetList([new VNumber(0)]);
        Result<Value, Error> result = vList.GetFunc("Remove", [new VNumber(0)]);

        Assert.True(result.IsSuccess);
        Assert.Empty(vList.value);
    }

    [Fact]
    void checkRemove_whenInvalidId()
    {
        VList vList = GetList([new VNumber(0)]);
        Result<Value, Error> result = vList.GetFunc("Remove", [new VNumber(2)]);

        Assert.True(result.IsFailed);
        Assert.IsType<ArgOutOfRangeError>(result.GetError());
    }

    private VList GetList(List<Value>? values = null)
    {
        return (VList)
            new VList(values ?? []).SetPos(Position.Null, Position.Null).SetContext(new Context());
    }
}
