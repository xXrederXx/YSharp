using Xunit;
using YSharp.Common;
using YSharp.Runtime;
using YSharp.Runtime.Collections.List;
using YSharp.Runtime.Primatives.Number;
using YSharp.Util;

namespace YSharp.Tests;

using RunResult = Result<Value, Error>;

public class FeatureTest
{
    private readonly RunClass _runClass = new();

    [Theory]
    [MemberData(nameof(TestCases))]
    public void ForLoopSum(CliArgs arg)
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
    public void FunctionSimpleReturn(CliArgs arg)
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
    public void ListIndex(CliArgs arg)
    {
        RunResult res = _runClass.Run("TEST", "VAR x = [10, 20, 30];x.Get(1)", arg);

        Assert.True(res.IsSuccess);
        Assert.Equal(20, ExtractResult(res));
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void ListLengthProperty(CliArgs arg)
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
    public void MathSqrtTest(CliArgs arg)
    {
        RunResult res = _runClass.Run("TEST", "MATH.SQRT(9)", arg);

        Assert.True(res.IsSuccess);
        Assert.Equal(3, ExtractResult(res));
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void NestedCalls(CliArgs arg)
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
        if(!res.TryGetValue(out Value val))
            return double.NaN;
        switch (val)
        {
            case VNumber num:
                return num.value;

            case VList list:
                VNumber? firstNum = list.value.FirstOrDefault(v => v is VNumber) as VNumber;
                if (firstNum != null) return firstNum.value;
                throw new InvalidOperationException("No numeric result found in list.");

            default:
                throw new InvalidOperationException($"Unexpected result type: {val.GetType()}");
        }
    }

    public static TheoryData<CliArgs> TestCases =
        new TheoryData<CliArgs>(CliArgs.ArgsNoOptimization, CliArgs.ArgsWithOptimization);
}