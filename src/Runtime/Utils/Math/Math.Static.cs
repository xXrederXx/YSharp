using YSharp.Common;
using YSharp.Runtime.Primatives.Number;

namespace YSharp.Runtime.Utils.Math;

public sealed partial class VMath : Value
{
    private static readonly MethodTable<VMath> methodTable;
    private static readonly PropertyTable<VMath> propertyTable;

    static VMath()
    {
        methodTable = new MethodTable<VMath>([
            ("ABS", (self, args) => GetMathFunc(self, "ABS", args)),
            ("CEIL", (self, args) => GetMathFunc(self, "CEIL", args)),
            ("FLOOR", (self, args) => GetMathFunc(self, "FLOOR", args)),
            ("ROUND", (self, args) => GetMathFunc(self, "ROUND", args)),
            ("SQRT", (self, args) => GetMathFunc(self, "SQRT", args)),
            ("CBRT", (self, args) => GetMathFunc(self, "CBRT", args)),
            ("SIN", (self, args) => GetMathFunc(self, "SIN", args)),
            ("COS", (self, args) => GetMathFunc(self, "COS", args)),
            ("TAN", (self, args) => GetMathFunc(self, "TAN", args)),
            ("SINH", (self, args) => GetMathFunc(self, "SINH", args)),
            ("COSH", (self, args) => GetMathFunc(self, "COSH", args)),
            ("TANH", (self, args) => GetMathFunc(self, "TANH", args)),
            ("ASIN", (self, args) => GetMathFunc(self, "ASIN", args)),
            ("ACOS", (self, args) => GetMathFunc(self, "ACOS", args)),
            ("ATAN", (self, args) => GetMathFunc(self, "ATAN", args)),
            ("ASINH", (self, args) => GetMathFunc(self, "ASINH", args)),
            ("ACOSH", (self, args) => GetMathFunc(self, "ACOSH", args)),
            ("ATANH", (self, args) => GetMathFunc(self, "ATANH", args)),
            ("LOG", (self, args) => GetMathFunc(self, "LOG", args)),
            ("LOG2", (self, args) => GetMathFunc(self, "LOG2", args)),
            ("LOG10", (self, args) => GetMathFunc(self, "LOG10", args)),
        ]);
        propertyTable = new PropertyTable<VMath>([
            ("PI", self => Result<Value, Error>.Success(new VNumber(System.Math.PI))),
            ("E", self => Result<Value, Error>.Success(new VNumber(System.Math.E))),
            ("TAU", self => Result<Value, Error>.Success(new VNumber(System.Math.Tau))),
        ]);
    }

    private static Result<Value, Error> GetMathFunc(VMath self, string name, List<Value> argNodes)
    {
        Error err = ValueHelper.CheckArgs(
            argNodes,
            1,
            [typeof(VNumber)],
            self.Context ?? new Context()
        );
        if (err.IsError)
            return Result<Value, Error>.Fail(err);

        return name switch
        {
            "ABS" =>
                Result<Value, Error>.Success(new VNumber(System.Math.Abs(((VNumber)argNodes[0]).value))),
            "CEIL" =>
                Result<Value, Error>.Success(new VNumber(System.Math.Ceiling(((VNumber)argNodes[0]).value))),
            "FLOOR" =>
                Result<Value, Error>.Success(new VNumber(System.Math.Floor(((VNumber)argNodes[0]).value))),
            "ROUND" =>
                Result<Value, Error>.Success(new VNumber(System.Math.Round(((VNumber)argNodes[0]).value))),
            "SQRT" =>
                Result<Value, Error>.Success(new VNumber(System.Math.Sqrt(((VNumber)argNodes[0]).value))),
            "CBRT" =>
                Result<Value, Error>.Success(new VNumber(System.Math.Cbrt(((VNumber)argNodes[0]).value))),
            "SIN" =>
                Result<Value, Error>.Success(new VNumber(System.Math.Sin(((VNumber)argNodes[0]).value))),
            "COS" =>
                Result<Value, Error>.Success(new VNumber(System.Math.Cos(((VNumber)argNodes[0]).value))),
            "TAN" =>
                Result<Value, Error>.Success(new VNumber(System.Math.Tan(((VNumber)argNodes[0]).value))),
            "SINH" =>
                Result<Value, Error>.Success(new VNumber(System.Math.Sinh(((VNumber)argNodes[0]).value))),
            "COSH" =>
                Result<Value, Error>.Success(new VNumber(System.Math.Cosh(((VNumber)argNodes[0]).value))),
            "TANH" =>
                Result<Value, Error>.Success(new VNumber(System.Math.Tanh(((VNumber)argNodes[0]).value))),
            "ASIN" =>
                Result<Value, Error>.Success(new VNumber(System.Math.Asin(((VNumber)argNodes[0]).value))),
            "ACOS" =>
                Result<Value, Error>.Success(new VNumber(System.Math.Acos(((VNumber)argNodes[0]).value))),
            "ATAN" =>
                Result<Value, Error>.Success(new VNumber(System.Math.Atan(((VNumber)argNodes[0]).value))),
            "ASINH" =>
                Result<Value, Error>.Success(new VNumber(System.Math.Asinh(((VNumber)argNodes[0]).value))),
            "ACOSH" =>
                Result<Value, Error>.Success(new VNumber(System.Math.Acosh(((VNumber)argNodes[0]).value))),
            "ATANH" =>
                Result<Value, Error>.Success(new VNumber(System.Math.Atanh(((VNumber)argNodes[0]).value))),
            "LOG" =>
                Result<Value, Error>.Success(new VNumber(System.Math.Log(((VNumber)argNodes[0]).value))),
            "LOG2" =>
                Result<Value, Error>.Success(new VNumber(System.Math.Log2(((VNumber)argNodes[0]).value))),
            "LOG10" =>
                Result<Value, Error>.Success(new VNumber(System.Math.Log10(((VNumber)argNodes[0]).value))),
            _ => throw new NotImplementedException(
                $"You called this with {name} but didnt implement it"
            ),
        };
    }
}
