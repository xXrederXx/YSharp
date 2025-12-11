using Xunit;
using YSharp.Types.Common;
using YSharp.Types.Interpreter;
using YSharp.Types.Interpreter.Collection;
using YSharp.Types.Interpreter.Primitives;
using YSharp.Utils;

namespace YSharp.Tests;

public class ScriptTest
{
    private readonly RunClass _runClass = new();

    [Fact]
    public void SumFactorials()
    {
        (Value val, Error err) = _runClass.Run(
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

    "
        );

        Assert.IsType<ErrorNull>(err);
        Assert.Equal(153, ExtractResult(val));
    } 

    [Fact(Skip = "No modulo")]
    public void CollatzSteps()
    {
        (Value val, Error err) = _runClass.Run(
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

        "
        );

        Assert.IsType<ErrorNull>(err);
        Assert.Equal(9, ExtractResult(val));
    }

    [Fact]
    public void FibSum()
    {
        (Value val, Error err) = _runClass.Run(
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
    "
        );

        Assert.IsType<ErrorNull>(err);
        Assert.Equal(88, ExtractResult(val));
    }

    [Fact]
    public void Power()
    {
        (Value val, Error err) = _runClass.Run(
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
    "
        );

        Assert.IsType<ErrorNull>(err);
        Assert.Equal(81, ExtractResult(val));
    }

    [Fact]
    public void SumList()
    {
        (Value val, Error err) = _runClass.Run(
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
    "
        );

        Assert.IsType<ErrorNull>(err);
        Assert.Equal(15, ExtractResult(val));
    }

    [Fact]
    public void Fib()
    {
        (Value val, Error err) = _runClass.Run(
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
    "
        );

        Assert.IsType<ErrorNull>(err);
        Assert.Equal(55, ExtractResult(val));
    }

    [Fact]
    public void Factorial()
    {
        (Value val, Error err) = _runClass.Run(
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
    "
        );

        Assert.IsType<ErrorNull>(err);
        Assert.Equal(120, ExtractResult(val));
    }

    [Fact]
    public void Add()
    {
        (Value val, Error err) = _runClass.Run(
            "TEST",
            @"
FUN add(a, b)
    RETURN a + b
END

add(5, 7)

    "
        );

        Assert.IsType<ErrorNull>(err);
        Assert.Equal(12, ExtractResult(val));
    }

    private double ExtractResult(Value val)
    {
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
}
