using Xunit;
using YSharp.Common;
using YSharp.Runtime;
using YSharp.Runtime.Collections.List;
using YSharp.Runtime.Primatives.Number;
using YSharp.Util;

namespace YSharp.Tests;

using RunResult = Result<Value, Error>;
public class ScriptTest
{
    private readonly RunClass _runClass = new();

    [Theory]
    [MemberData(nameof(TestCases))]
    public void SumFactorials(CliArgs arg)
    {
        RunResult res = _runClass.Run(
            "TEST",
            @"
FUN sumFactorials(n)
    VAR total = 0
    FOR i = 1 TO n + 1 THEN
        VAR fact = 1
        FOR j = 1 TO i + 1 THEN
            VAR fact *= j
        END
        VAR total += fact
    END
    RETURN total
END

sumFactorials(5)

    ", arg
        );

        Assert.True(res.IsSuccess);
        Assert.Equal(153, ExtractResult(res));
    } 

    [Theory(Skip = "No Modulo")]
    [MemberData(nameof(TestCases))]
    public void CollatzSteps(CliArgs arg)
    {
        RunResult res = _runClass.Run(
            "TEST",
            @"
FUN collatzSteps(x)
    VAR steps = 0
    WHILE x != 1 THEN
        IF x % 2 == 0 THEN
            VAR x /= 2
        ELSE
            VAR x = x * 3 + 1
        END
        VAR steps += 1
    END
    RETURN steps
END

collatzSteps(13)

        ", arg
        );

        Assert.True(res.IsSuccess);
        Assert.Equal(9, ExtractResult(res));
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void FibSum(CliArgs arg)
    {
        RunResult res = _runClass.Run(
            "TEST",
            @"
FUN fibSum(n)
    VAR a = 0
    VAR b = 1
    VAR total = a + b
    FOR i = 2 TO n THEN
        VAR c = a + b
        VAR total += c
        VAR a = b
        VAR b = c
    END
    RETURN total
END

fibSum(10)
    ", arg
        );

        Assert.True(res.IsSuccess);
        Assert.Equal(88, ExtractResult(res));
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void Power(CliArgs arg)
    {
        RunResult res = _runClass.Run(
            "TEST",
            @"
FUN power(base, exp)
    VAR result = 1
    FOR i = 1 TO exp + 1 THEN
        VAR result *= base
    END
    RETURN result
END

power(3, 4)
    ", arg
        );

        Assert.True(res.IsSuccess);
        Assert.Equal(81, ExtractResult(res));
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void SumList(CliArgs arg)
    {
        RunResult res = _runClass.Run(
            "TEST",
            @"
FUN sumList(lst)
    VAR total = 0
    FOR i = 0 TO lst.Length THEN
        VAR total += lst.Get(i)
    END
    RETURN total
END

sumList([1, 2, 3, 4, 5])
    ", arg
        );

        Assert.True(res.IsSuccess);
        Assert.Equal(15, ExtractResult(res));
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void Fib(CliArgs arg)
    {
        RunResult res = _runClass.Run(
            "TEST",
            @"
FUN fib(n)
    VAR a = 0
    VAR b = 1
    FOR i = 2 TO n + 1 THEN
        VAR temp = b
        VAR b = a + b
        VAR a = temp
    END
    RETURN b
END

fib(10)
    ", arg
        );

        Assert.True(res.IsSuccess);
        Assert.Equal(55, ExtractResult(res));
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void Factorial(CliArgs arg)
    {
        RunResult res = _runClass.Run(
            "TEST",
            @"
FUN factorial(n)
    VAR result = 1
    FOR i = 1 TO n + 1 THEN
        VAR result *= i
    END
    RETURN result
END

factorial(5)
    ", arg
        );

        Assert.True(res.IsSuccess);
        Assert.Equal(120, ExtractResult(res));
    }

    [Theory]
    [MemberData(nameof(TestCases))]
    public void Add(CliArgs arg)
    {
        RunResult res = _runClass.Run(
            "TEST",
            @"
FUN add(a, b)
    RETURN a + b
END

add(5, 7)

    ", arg
        );

        Assert.True(res.IsSuccess);
        Assert.Equal(12, ExtractResult(res));
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
                if (firstNum != null)
                    return firstNum.value;
                throw new InvalidOperationException("No numeric result found in list.");

            default:
                throw new InvalidOperationException($"Unexpected result type: {val.GetType()}");
        }
    }
    
    
    public static TheoryData<CliArgs> TestCases =
        new TheoryData<CliArgs>(CliArgs.ArgsNoOptimization, CliArgs.ArgsWithOptimization);
}
