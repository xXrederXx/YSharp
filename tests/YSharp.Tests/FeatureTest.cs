using Xunit;
using YSharp.Types.Common;
using YSharp.Types.Interpreter;
using YSharp.Types.Interpreter.Primitives;
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
        PRINT(A(4))
    "
        );

        Assert.IsType<ErrorNull>(err);
        Assert.Equal(5, ((VNumber)val).value);
    }

    [Fact]
    public void NestedCalls()
    {
        var (val, err) = _runClass.Run(
            "TEST",
            @"
        FUN A(x); RETURN x + 1 END
        FUN B(x); RETURN x * 2 END
        PRINT(B(A(3)))
    "
        );

        Assert.IsType<ErrorNull>(err);
        Assert.Equal(8, ((VNumber)val).value);
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
        PRINT(s)
    "
        );

        Assert.IsType<ErrorNull>(err);
        Assert.Equal(15, ((VNumber)val).value);
    }

    [Fact]
    public void ListIndex()
    {
        var (val, err) = _runClass.Run("TEST", "PRINT([10, 20, 30][1])");

        Assert.IsType<ErrorNull>(err);
        Assert.Equal(20, ((VNumber)val).value);
    }

    [Fact]
    public void ListLengthProperty()
    {
        var (val, err) = _runClass.Run(
            "TEST",
            @"
        VAR l = [1,2,3,4]
        PRINT(l.Length)
    "
        );

        Assert.IsType<ErrorNull>(err);
        Assert.Equal(4, ((VNumber)val).value);
    }

    [Fact]
    public void MathSqrtTest()
    {
        var (val, err) = _runClass.Run("TEST", "PRINT(MATH.SQRT(9))");

        Assert.IsType<ErrorNull>(err);
        Assert.Equal(3, ((VNumber)val).value);
    }
}
