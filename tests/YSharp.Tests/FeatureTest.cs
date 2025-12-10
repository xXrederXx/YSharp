using Xunit;
using YSharp.Types.Common;
using YSharp.Types.Interpreter;
using YSharp.Types.Interpreter.Primitives;
using YSharp.Types.Interpreter.Collection;
using YSharp.Utils;

namespace YSharp.Tests;

public class FeatureTest
{
    private readonly RunClass _runClass = new();

    [Fact]
    public void FunctionSimpleReturn()
    {
        var (val, err) = _runClass.Run(
            "TEST",
            @"
        FUN A(x)
            RETURN x + 1
        END
        A(4)
    "
        );

        Assert.IsType<ErrorNull>(err);
        Assert.Equal(5, ExtractResult(val));
    }

    [Fact]
    public void NestedCalls()
    {
        var (val, err) = _runClass.Run(
            "TEST",
            @"
        FUN A(x); RETURN x + 1 END
        FUN B(x); RETURN x * 2 END
        B(A(3))
    "
        );

        Assert.IsType<ErrorNull>(err);
        Assert.Equal(8, ExtractResult(val));
    }

    [Fact]
    public void ForLoopSum()
    {
        var (val, err) = _runClass.Run(
            "TEST",
            @"
        VAR s = 0
        FOR i = 1 TO 5 THEN
            VAR s = s + i
        END
        s
    "
        );

        Assert.IsType<ErrorNull>(err);
        Assert.Equal(15, ExtractResult(val));
    }

    [Fact]
    public void ListIndex()
    {
        var (val, err) = _runClass.Run("TEST", "PRINT([10, 20, 30 ].get(1)");

        Assert.IsType<ErrorNull>(err);
        Assert.Equal(20, ExtractResult(val));
    }

    [Fact]
    public void ListLengthProperty()
    {
        var (val, err) = _runClass.Run(
            "TEST",
            @"
        VAR l = [1,2,3,4]
        l.Length
    "
        );

        Assert.IsType<ErrorNull>(err);
        Assert.Equal(4, ExtractResult(val));
    }

    [Fact]
    public void MathSqrtTest()
    {
        var (val, err) = _runClass.Run("TEST", "MATH.SQRT(9)");

        Assert.IsType<ErrorNull>(err);
        Assert.Equal(3, ExtractResult(val));
    }

    private double ExtractResult(Value val)
    {
        switch (val)
        {
            case VNumber num:
                return num.value;

            case VList list:
                var firstNum = list.value.FirstOrDefault(v => v is VNumber) as VNumber;
                if (firstNum != null) return firstNum.value;
                throw new InvalidOperationException("No numeric result found in list.");

            default:
                throw new InvalidOperationException($"Unexpected result type: {val.GetType()}");
        }
    }

}
