using YSharp.Types.Common;
using YSharp.Types.Interpreter.Internal;
using YSharp.Types.Interpreter.Primitives;

namespace YSharp.Types.Interpreter.Utils;

public sealed partial class VMath : Value
{
    private static readonly MethodTable<VMath> methodTable;
    private static readonly PropertyTable<VMath> propertyTable;

    static VMath()
    {
        methodTable = new MethodTable<VMath>(
            [
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
                ("LOG10", (self, args) => GetMathFunc(self, "LOG10", args))
            ]
        );
        propertyTable = new PropertyTable<VMath>(
            [
                ("PI", self => (new VNumber(Math.PI), ErrorNull.Instance)),
                ("E", self => (new VNumber(Math.E), ErrorNull.Instance)),
                ("TAU", self => (new VNumber(Math.Tau), ErrorNull.Instance))
            ]
        );
    }

    private static ValueAndError GetMathFunc(VMath self, string name, List<Value> argNodes)
    {
        Error err = ValueHelper.CheckArgs(
            argNodes,
            1,
            [typeof(VNumber)],
            self.context ?? new Context()
        );
        if (err.IsError) return (ValueNull.Instance, err);

        return name switch
        {
            "ABS" => (ValueAndError)
                (new VNumber(Math.Abs(((VNumber)argNodes[0]).value)), ErrorNull.Instance),
            "CEIL" => (ValueAndError)
                (new VNumber(Math.Ceiling(((VNumber)argNodes[0]).value)), ErrorNull.Instance),
            "FLOOR" => (ValueAndError)
                (new VNumber(Math.Floor(((VNumber)argNodes[0]).value)), ErrorNull.Instance),
            "ROUND" => (ValueAndError)
                (new VNumber(Math.Round(((VNumber)argNodes[0]).value)), ErrorNull.Instance),
            "SQRT" => (ValueAndError)
                (new VNumber(Math.Sqrt(((VNumber)argNodes[0]).value)), ErrorNull.Instance),
            "CBRT" => (ValueAndError)
                (new VNumber(Math.Cbrt(((VNumber)argNodes[0]).value)), ErrorNull.Instance),
            "SIN" => (ValueAndError)
                (new VNumber(Math.Sin(((VNumber)argNodes[0]).value)), ErrorNull.Instance),
            "COS" => (ValueAndError)
                (new VNumber(Math.Cos(((VNumber)argNodes[0]).value)), ErrorNull.Instance),
            "TAN" => (ValueAndError)
                (new VNumber(Math.Tan(((VNumber)argNodes[0]).value)), ErrorNull.Instance),
            "SINH" => (ValueAndError)
                (new VNumber(Math.Sinh(((VNumber)argNodes[0]).value)), ErrorNull.Instance),
            "COSH" => (ValueAndError)
                (new VNumber(Math.Cosh(((VNumber)argNodes[0]).value)), ErrorNull.Instance),
            "TANH" => (ValueAndError)
                (new VNumber(Math.Tanh(((VNumber)argNodes[0]).value)), ErrorNull.Instance),
            "ASIN" => (ValueAndError)
                (new VNumber(Math.Asin(((VNumber)argNodes[0]).value)), ErrorNull.Instance),
            "ACOS" => (ValueAndError)
                (new VNumber(Math.Acos(((VNumber)argNodes[0]).value)), ErrorNull.Instance),
            "ATAN" => (ValueAndError)
                (new VNumber(Math.Atan(((VNumber)argNodes[0]).value)), ErrorNull.Instance),
            "ASINH" => (ValueAndError)
                (new VNumber(Math.Asinh(((VNumber)argNodes[0]).value)), ErrorNull.Instance),
            "ACOSH" => (ValueAndError)
                (new VNumber(Math.Acosh(((VNumber)argNodes[0]).value)), ErrorNull.Instance),
            "ATANH" => (ValueAndError)
                (new VNumber(Math.Atanh(((VNumber)argNodes[0]).value)), ErrorNull.Instance),
            "LOG" => (ValueAndError)
                (new VNumber(Math.Log(((VNumber)argNodes[0]).value)), ErrorNull.Instance),
            "LOG2" => (ValueAndError)
                (new VNumber(Math.Log2(((VNumber)argNodes[0]).value)), ErrorNull.Instance),
            "LOG10" => (ValueAndError)
                (new VNumber(Math.Log10(((VNumber)argNodes[0]).value)), ErrorNull.Instance),
            _ => throw new NotImplementedException($"You called this with {name} but didnt implement it")
        };
    }
}