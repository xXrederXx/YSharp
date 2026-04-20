using Xunit;
using YSharp.Common;
using YSharp.Runtime;
using YSharp.Runtime.Primitives.Number;
using YSharp.Runtime.Utils.Math;

namespace YSharp.Tests;

public class MathTest
{
    public static TheoryData<string, double> GetFunctionsData()
    {
        const double testNum = 0.5;
        TheoryData<string, double> data = new TheoryData<string, double>
        {
            { "ABS", System.Math.Abs(testNum) },
            { "CEIL", System.Math.Ceiling(testNum) },
            { "FLOOR", System.Math.Floor(testNum) },
            { "ROUND", System.Math.Round(testNum) },
            { "SQRT", System.Math.Sqrt(testNum) },
            { "CBRT", System.Math.Cbrt(testNum) },
            { "SIN", System.Math.Sin(testNum) },
            { "COS", System.Math.Cos(testNum) },
            { "TAN", System.Math.Tan(testNum) },
            { "SINH", System.Math.Sinh(testNum) },
            { "COSH", System.Math.Cosh(testNum) },
            { "TANH", System.Math.Tanh(testNum) },
            { "ASIN", System.Math.Asin(testNum) },
            { "ACOS", System.Math.Acos(testNum) },
            { "ATAN", System.Math.Atan(testNum) },
            { "ASINH", System.Math.Asinh(testNum) },
            { "ACOSH", System.Math.Acosh(testNum) },
            { "ATANH", System.Math.Atanh(testNum) },
            { "LOG", System.Math.Log(testNum) },
            { "LOG2", System.Math.Log2(testNum) },
            { "LOG10", System.Math.Log10(testNum) },
        };
        return data;
    }

    [Theory]
    [MemberData(nameof(GetFunctionsData))]
    void checkFunctions(string name, double expected)
    {
        Result<Value, Error> result = new VMath().GetFunc(name, [new VNumber(0.5)]);

        Assert.True(result.TryGetValue(out Value value));
        VNumber number = Assert.IsType<VNumber>(value);
        Assert.Equal(expected, number.value, TestingConstans.DOUBLE_PRECISION);
    }

    [Theory]
    [MemberData(nameof(GetFunctionsData))]
    void checkFunctions_invalidArgs(string name, double _)
    {
        Result<Value, Error> result = new VMath().GetFunc(
            name,
            [new VNumber(0.5), new VNumber(0.5)]
        );

        Assert.True(result.TryGetError(out Error error));
        Assert.IsType<NumArgsError>(error);
    }

    [Theory]
    [InlineData("PI", Math.PI)]
    [InlineData("E", Math.E)]
    [InlineData("TAU", Math.Tau)]
    void checkVariables(string name, double expected)
    {
        Result<Value, Error> result = new VMath().GetVar(name);

        Assert.True(result.TryGetValue(out Value value));
        VNumber number = Assert.IsType<VNumber>(value);
        Assert.Equal(expected, number.value, TestingConstans.DOUBLE_PRECISION);
    }
}
