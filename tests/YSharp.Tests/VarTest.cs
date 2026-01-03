using Xunit;
using YSharp.Common;
using YSharp.Runtime;
using YSharp.Util;

namespace YSharp.Tests;

using RunResult = Result<Value, Error>;

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
        RunResult res = _runClass.Run("TEST", "VAR x = " + toAssign + ";PRINT(x)");
        Assert.True(res.IsSuccess);
    }

    [Theory]
    [InlineData("5")]
    [InlineData("6.7")]
    [InlineData("\"Hi\"")]
    [InlineData("[1, 2]")]
    public void Assign_Test(string toAssign)
    {
        RunResult res = _runClass.Run("TEST", "VAR x = " + toAssign);
        Assert.True(res.IsSuccess);
    }
}