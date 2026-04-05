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
            ("ABS", (self, args) => GetMathFunc(self, System.Math.Abs, args)),
            ("CEIL", (self, args) => GetMathFunc(self, System.Math.Ceiling, args)),
            ("FLOOR", (self, args) => GetMathFunc(self, System.Math.Floor, args)),
            ("ROUND", (self, args) => GetMathFunc(self, System.Math.Round, args)),
            ("SQRT", (self, args) => GetMathFunc(self, System.Math.Sqrt, args)),
            ("CBRT", (self, args) => GetMathFunc(self, System.Math.Cbrt, args)),
            ("SIN", (self, args) => GetMathFunc(self, System.Math.Sin, args)),
            ("COS", (self, args) => GetMathFunc(self, System.Math.Cos, args)),
            ("TAN", (self, args) => GetMathFunc(self, System.Math.Tan, args)),
            ("SINH", (self, args) => GetMathFunc(self, System.Math.Sinh, args)),
            ("COSH", (self, args) => GetMathFunc(self, System.Math.Cosh, args)),
            ("TANH", (self, args) => GetMathFunc(self, System.Math.Tanh, args)),
            ("ASIN", (self, args) => GetMathFunc(self, System.Math.Asin, args)),
            ("ACOS", (self, args) => GetMathFunc(self, System.Math.Acos, args)),
            ("ATAN", (self, args) => GetMathFunc(self, System.Math.Atan, args)),
            ("ASINH", (self, args) => GetMathFunc(self, System.Math.Asinh, args)),
            ("ACOSH", (self, args) => GetMathFunc(self, System.Math.Acosh, args)),
            ("ATANH", (self, args) => GetMathFunc(self, System.Math.Atanh, args)),
            ("LOG", (self, args) => GetMathFunc(self, System.Math.Log, args)),
            ("LOG2", (self, args) => GetMathFunc(self, System.Math.Log2, args)),
            ("LOG10", (self, args) => GetMathFunc(self, System.Math.Log10, args)),
        ]);
        propertyTable = new PropertyTable<VMath>([
            ("PI", self => Result<Value, Error>.Success(new VNumber(System.Math.PI))),
            ("E", self => Result<Value, Error>.Success(new VNumber(System.Math.E))),
            ("TAU", self => Result<Value, Error>.Success(new VNumber(System.Math.Tau))),
        ]);
    }

    private static Result<Value, Error> GetMathFunc(VMath self, Func<double, double> func, List<Value> argNodes)
    {
        Error err = ValueHelper.CheckArgs(
            argNodes,
            1,
            [typeof(VNumber)],
            self.Context ?? new Context()
        );
        if (err.IsError)
            return Result<Value, Error>.Fail(err);

        return Result<Value, Error>.Success(new VNumber(func(((VNumber)argNodes[0]).value)));
    }
}
