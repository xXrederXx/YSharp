using YSharp.Core;
using YSharp.Types.Common;

namespace YSharp.Types.Interpreter.ClassTypes;

public class VMath : Value
{
    public override ValueAndError GetVar(string name)
    {
        return name switch
        {
            "PI" => (ValueAndError)(new VNumber(Math.PI), ErrorNull.Instance),
            "E" => (ValueAndError)(new VNumber(Math.E), ErrorNull.Instance),
            "TAU" => (ValueAndError)(new VNumber(Math.Tau), ErrorNull.Instance),
            _ => base.GetVar(name),
        };
    }

    public override ValueAndError GetFunc(string name, List<Value> argNodes)
    {
        Error err = ValueHelper.CheckArgs(argNodes, 1, [typeof(VNumber)], context ?? new Context());
        if (err.IsError)
        {
            return (ValueNull.Instance, err);
        }

        return name switch
        {
            "ABS" => (ValueAndError)
                (new VNumber(Math.Abs(((VNumber)argNodes[0]).value)), ErrorNull.Instance),
            "CEIL" => (ValueAndError)
                (
                    new VNumber(Math.Ceiling(((VNumber)argNodes[0]).value)),
                    ErrorNull.Instance
                ),
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
            _ => base.GetFunc(name, argNodes),
        };
    }

    public override Value Copy()
    {
        return new VMath();
    }
}
