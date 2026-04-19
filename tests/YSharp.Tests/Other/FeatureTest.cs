using Xunit;
using YSharp.Common;
using YSharp.Runtime;
using YSharp.Runtime.Collections.List;
using YSharp.Runtime.Primitives.Number;
using YSharp.Util;

namespace YSharp.Tests;

using RunResult = Result<Value, Error>;

public class FeatureTest
{
    private readonly RunClass _runClass = new();

    [Theory]
    [MemberData(nameof(TestCases))]
    public void checkForLoopSum_whenLoopingFrom1To5_thenSumIs10(CliArgs arg)
    {
        RunResult res = _runClass.Run(
            "TEST",
            @"
        FUN x()
            VAR s = 0
            FOR i = 1 TO 5 THEN
                VAR s = s + i
            END
            RETURN s
        END
        x()

    ", arg
        );

        Assert.True(res.IsSuccess);
        Assert.Equal(10, ExtractResult(res));
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void checkFunctionSimpleReturn_whenCalledWith4_thenReturns5(CliArgs arg)
    {
        RunResult res = _runClass.Run(
            "TEST",
            @"
        FUN A(x)
            RETURN x + 1
        END
        A(4)
    ", arg
        );

        Assert.True(res.IsSuccess);
        Assert.Equal(5, ExtractResult(res));
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void checkListIndex_whenAccessingIndex1_thenReturns20(CliArgs arg)
    {
        RunResult res = _runClass.Run("TEST", "VAR x = [10, 20, 30];x.Get(1)", arg);

        Assert.True(res.IsSuccess);
        Assert.Equal(20, ExtractResult(res));
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void checkListLengthProperty_whenListHas4Elements_thenReturns4(CliArgs arg)
    {
        RunResult res = _runClass.Run(
            "TEST",
            @"
        VAR l = [1,2,3,4]
        l.Length
    ", arg
        );

        Assert.True(res.IsSuccess);
        Assert.Equal(4, ExtractResult(res));
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void checkMathSqrt_whenInput9_thenReturns3(CliArgs arg)
    {
        RunResult res = _runClass.Run("TEST", "MATH.SQRT(9)", arg);

        Assert.True(res.IsSuccess);
        Assert.Equal(3, ExtractResult(res));
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void checkNestedCalls_whenCallingBWithA3_thenReturns8(CliArgs arg)
    {
        RunResult res = _runClass.Run(
            "TEST",
            @"
        FUN A(x); RETURN x + 1 END
        FUN B(x); RETURN x * 2 END
        B(A(3))
    ", arg
        );

        Assert.True(res.IsSuccess);
        Assert.Equal(8, ExtractResult(res));
    }

    private double ExtractResult(RunResult res)
    {
        if (!res.TryGetValue(out Value val))
            return double.NaN;
        switch (val)
        {
            case VNumber num:
                return num.value;

            case VList list:
                if (list.value.LastOrDefault(v => v is VNumber) is VNumber firstNum) return firstNum.value;
                throw new InvalidOperationException("No numeric result found in list.");

            default:
                throw new InvalidOperationException($"Unexpected result type: {val.GetType()}");
        }
    }

    public static TheoryData<CliArgs> TestCases =
        new TheoryData<CliArgs>(CliArgs.ArgsNoOptimization, CliArgs.ArgsWithOptimization);
}
