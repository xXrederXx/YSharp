namespace YSharp_2._0;

public class VMath : Value
{
    public override ValueError GetVar(string name)
    {
        return name switch
        {
            "PI" => (ValueError)(new VNumber(System.Math.PI), ErrorNull.Instance),
            "E" => (ValueError)(new VNumber(System.Math.E), ErrorNull.Instance),
            "TAU" => (ValueError)(new VNumber(System.Math.Tau), ErrorNull.Instance),
            _ => base.GetVar(name),
        };
    }
    public override ValueError GetFunc(string name, List<Value> argNodes)
    {
        Error err = ValueHelper.CheckArgs(argNodes, 1, [typeof(VNumber)], context ?? new Context());
        if (err.IsError)
        {
            return (ValueNull.Instance, err);
        }

        return name switch
        {
            "ABS" => (ValueError)(new VNumber(System.Math.Abs(((VNumber)argNodes[0]).value)), ErrorNull.Instance),
            "CEIL" => (ValueError)(new VNumber(System.Math.Ceiling(((VNumber)argNodes[0]).value)), ErrorNull.Instance),
            "FLOOR" => (ValueError)(new VNumber(System.Math.Floor(((VNumber)argNodes[0]).value)), ErrorNull.Instance),
            "ROUND" => (ValueError)(new VNumber(System.Math.Round(((VNumber)argNodes[0]).value)), ErrorNull.Instance),
            "SQRT" => (ValueError)(new VNumber(System.Math.Sqrt(((VNumber)argNodes[0]).value)), ErrorNull.Instance),
            "CBRT" => (ValueError)(new VNumber(System.Math.Cbrt(((VNumber)argNodes[0]).value)), ErrorNull.Instance),
            "SIN" => (ValueError)(new VNumber(System.Math.Sin(((VNumber)argNodes[0]).value)), ErrorNull.Instance),
            "COS" => (ValueError)(new VNumber(System.Math.Cos(((VNumber)argNodes[0]).value)), ErrorNull.Instance),
            "TAN" => (ValueError)(new VNumber(System.Math.Tan(((VNumber)argNodes[0]).value)), ErrorNull.Instance),
            "SINH" => (ValueError)(new VNumber(System.Math.Sinh(((VNumber)argNodes[0]).value)), ErrorNull.Instance),
            "COSH" => (ValueError)(new VNumber(System.Math.Cosh(((VNumber)argNodes[0]).value)), ErrorNull.Instance),
            "TANH" => (ValueError)(new VNumber(System.Math.Tanh(((VNumber)argNodes[0]).value)), ErrorNull.Instance),
            "ASIN" => (ValueError)(new VNumber(System.Math.Asin(((VNumber)argNodes[0]).value)), ErrorNull.Instance),
            "ACOS" => (ValueError)(new VNumber(System.Math.Acos(((VNumber)argNodes[0]).value)), ErrorNull.Instance),
            "ATAN" => (ValueError)(new VNumber(System.Math.Atan(((VNumber)argNodes[0]).value)), ErrorNull.Instance),
            "ASINH" => (ValueError)(new VNumber(System.Math.Asinh(((VNumber)argNodes[0]).value)), ErrorNull.Instance),
            "ACOSH" => (ValueError)(new VNumber(System.Math.Acosh(((VNumber)argNodes[0]).value)), ErrorNull.Instance),
            "ATANH" => (ValueError)(new VNumber(System.Math.Atanh(((VNumber)argNodes[0]).value)), ErrorNull.Instance),
            "LOG" => (ValueError)(new VNumber(System.Math.Log(((VNumber)argNodes[0]).value)), ErrorNull.Instance),
            "LOG2" => (ValueError)(new VNumber(System.Math.Log2(((VNumber)argNodes[0]).value)), ErrorNull.Instance),
            "LOG10" => (ValueError)(new VNumber(System.Math.Log10(((VNumber)argNodes[0]).value)), ErrorNull.Instance),
            _ => base.GetVar(name),
        };
    }
    public override Value Copy()
    {
        return new VMath();
    }
}

