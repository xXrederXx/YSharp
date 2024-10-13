namespace YSharp_2._0;

public class Math : Value
{
    public override ValueError GetVar(string name)
    {
        return name switch
        {
            "PI" => (ValueError)(new Number(System.Math.PI), NoError.Instance),
            "E" => (ValueError)(new Number(System.Math.E), NoError.Instance),
            "TAU" => (ValueError)(new Number(System.Math.Tau), NoError.Instance),
            _ => base.GetVar(name),
        };
    }
    public override ValueError GetFunc(string name, List<Value> argNodes)
    {
        Error err = CheckArgs(argNodes, 1, [typeof(Number)]);
        if (err.IsError)
        {
            return (ValueNull.Instance, err);
        }

        return name switch
        {
            "ABS" => (ValueError)(new Number(System.Math.Abs(((Number)argNodes[0]).value)), NoError.Instance),
            "CEIL" => (ValueError)(new Number(System.Math.Ceiling(((Number)argNodes[0]).value)), NoError.Instance),
            "FLOOR" => (ValueError)(new Number(System.Math.Floor(((Number)argNodes[0]).value)), NoError.Instance),
            "ROUND" => (ValueError)(new Number(System.Math.Round(((Number)argNodes[0]).value)), NoError.Instance),
            "SQRT" => (ValueError)(new Number(System.Math.Sqrt(((Number)argNodes[0]).value)), NoError.Instance),
            "CBRT" => (ValueError)(new Number(System.Math.Cbrt(((Number)argNodes[0]).value)), NoError.Instance),
            "SIN" => (ValueError)(new Number(System.Math.Sin(((Number)argNodes[0]).value)), NoError.Instance),
            "COS" => (ValueError)(new Number(System.Math.Cos(((Number)argNodes[0]).value)), NoError.Instance),
            "TAN" => (ValueError)(new Number(System.Math.Tan(((Number)argNodes[0]).value)), NoError.Instance),
            "SINH" => (ValueError)(new Number(System.Math.Sinh(((Number)argNodes[0]).value)), NoError.Instance),
            "COSH" => (ValueError)(new Number(System.Math.Cosh(((Number)argNodes[0]).value)), NoError.Instance),
            "TANH" => (ValueError)(new Number(System.Math.Tanh(((Number)argNodes[0]).value)), NoError.Instance),
            "ASIN" => (ValueError)(new Number(System.Math.Asin(((Number)argNodes[0]).value)), NoError.Instance),
            "ACOS" => (ValueError)(new Number(System.Math.Acos(((Number)argNodes[0]).value)), NoError.Instance),
            "ATAN" => (ValueError)(new Number(System.Math.Atan(((Number)argNodes[0]).value)), NoError.Instance),
            "ASINH" => (ValueError)(new Number(System.Math.Asinh(((Number)argNodes[0]).value)), NoError.Instance),
            "ACOSH" => (ValueError)(new Number(System.Math.Acosh(((Number)argNodes[0]).value)), NoError.Instance),
            "ATANH" => (ValueError)(new Number(System.Math.Atanh(((Number)argNodes[0]).value)), NoError.Instance),
            "LOG" => (ValueError)(new Number(System.Math.Log(((Number)argNodes[0]).value)), NoError.Instance),
            "LOG2" => (ValueError)(new Number(System.Math.Log2(((Number)argNodes[0]).value)), NoError.Instance),
            "LOG10" => (ValueError)(new Number(System.Math.Log10(((Number)argNodes[0]).value)), NoError.Instance),
            _ => base.GetVar(name),
        };
    }
    public override Value copy()
    {
        return new Math();
    }
}

