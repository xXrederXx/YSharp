using Xunit;
using YSharp.Types.Common;
using YSharp.Types.Interpreter;
using YSharp.Utils;

namespace YSharp.Tests;

public class VarTest
{
    private readonly RunClass _runClass = new();

    [Theory]
    [InlineData("5")]
    [InlineData("6.7")]
    [InlineData("\"Hi\"")]
    [InlineData("[1, 2]")]
    public void Assign_And_Get_Test(string toAssign)
    {
        (Value _, Error err) = _runClass.Run("TEST", "VAR x = " + toAssign + ";PRINT(x)");
        Assert.IsType<ErrorNull>(err);
    }

    [Theory]
    [InlineData("5")]
    [InlineData("6.7")]
    [InlineData("\"Hi\"")]
    [InlineData("[1, 2]")]
    public void Assign_Test(string toAssign)
    {
        (Value _, Error err) = _runClass.Run("TEST", "VAR x = " + toAssign);
        Assert.IsType<ErrorNull>(err);
    }
}