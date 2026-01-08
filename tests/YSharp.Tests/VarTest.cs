using Xunit;
using YSharp.Common;
using YSharp.Runtime;
using YSharp.Util;

namespace YSharp.Tests;

using RunResult = Result<Value, Error>;

public class VarTest{
    private readonly RunClass _runClass = new();

    [Theory]
    [MemberData(nameof(TestCases))]
    public void Assign_And_Get_Test(CliArgs arg, string toAssign)
    {
        RunResult res = _runClass.Run("TEST", "VAR x = " + toAssign + ";PRINT(x)", arg);
        Assert.True(res.IsSuccess);
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void Assign_Test(CliArgs arg, string toAssign)
    {
        RunResult res = _runClass.Run("TEST", "VAR x = " + toAssign, arg);
        Assert.True(res.IsSuccess);
    }

    public static IEnumerable<object[]> TestCases()
    {
        string[] values =
        [
            "5", "6.7", "\"Hi\"", "[1, 2]"
        ];

        foreach (CliArgs mode in new[] { CliArgs.ArgsNoOptimization, CliArgs.ArgsWithOptimization })
        {
            foreach (string x in values)
            {
                yield return [mode, x];
            }
        }
    }
}