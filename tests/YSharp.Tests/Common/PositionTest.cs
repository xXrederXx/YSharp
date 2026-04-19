using Xunit;
using YSharp.Common;

namespace YSharp.Tests;

public class PositionTest
{
    [Fact]
    void checkPosition_whenInitalized_fieldReturnSameValue()
    {
        Position pos = new Position(1, 2, 3, 4);

        Assert.Equal(1, pos.Index);
        Assert.Equal(2, pos.Line);
        Assert.Equal(3, pos.Column);
        Assert.Equal(4, pos.FileId);
    }

    [Fact]
    void checkPosition_whenIndexBelow0_throw()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Position(-1, 2, 3, 4));
    }

    [Fact]
    void checkPosition_whenLineTooBig_throw()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Position(1, ushort.MaxValue, 3, 4));
    }

    [Fact]
    void checkPosition_whenColumnTooBig_throw()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Position(1, 2, ushort.MaxValue, 4));
    }

    [Fact]
    void checkPositionEquality_whenSame_returnTrue()
    {
        Position left = new Position(1, 2, 3, 4);
        Position right = new Position(1, 2, 3, 4);

        Assert.True(left == right);
        Assert.False(left != right);

        Assert.True(left.Equals(right));
        Assert.True(left.Equals((object)right));
    }

    [Fact]
    void checkPositionEquality_whenDifferent_returnFalse()
    {
        Position left = new Position(2, 3, 4, 5);
        Position right = new Position(1, 2, 3, 4);

        Assert.False(left == right);
        Assert.True(left != right);

        Assert.False(left.Equals(right));
        Assert.False(left.Equals((object)right));
    }

    [Fact]
    void checkPositionEquality_whenDifferentType_returnFalse()
    {
        Position left = new Position(2, 3, 4, 5);
        Assert.False(left.Equals((object)"not pos"));
    }
}
