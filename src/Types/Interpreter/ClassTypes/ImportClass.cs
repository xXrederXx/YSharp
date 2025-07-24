using System.Reflection;
using YSharp.Types.Common;
using YSharp.Utils;

namespace YSharp.Types.Interpreter.ClassTypes;

public class ImportClass : Value
{
    public readonly ExposedClassData data;

    public ImportClass(ExposedClassData Data)
    {
        data = Data;
    }

    public override ValueAndError GetFunc(string name, List<Value> argNodes)
    {
        ValueAndError returnVE = new(
            ValueNull.Instance,
            new FuncNotFoundError(
                argNodes.Count >= 1 ? argNodes[0].startPos : Position.Null,
                $"No Function {name} found",
                new Core.Context()
            )
        );

        foreach (MethodInfo? method in data.methods)
        {
            ParameterInfo[] paramsInfo = method.GetParameters();

            if (method.Name != name && paramsInfo.Length != argNodes.Count)
            {
                continue;
            }

            if (!TypeCanBeUsed(method.ReturnType))
            {
                continue;
            }

            bool canBeUsed = false;
            List<object?> argList = new List<object?>();

            for (int i = 0; i < paramsInfo.Length; i++)
            {
                object? convParm = ConvertParam(paramsInfo[i].ParameterType, argNodes[i]);
                canBeUsed = convParm is not null;
                if (!canBeUsed)
                {
                    break;
                }
                argList.Add(convParm);
            }

            if (!canBeUsed)
            {
                continue;
            }

            object? ret = method.Invoke(data.instance, argList.ToArray());
            returnVE = new(ConvertReturn(ret), ErrorNull.Instance);
        }
        return returnVE;
    }

    private static object? ConvertParam(Type typeReqiered, Value valToConvert)
    {
        /* if (!TypeCanBeUsed(typeReqiered) || valToConvert is not IDefaultConvertableValue)
        {
            return null;
        }

        if (
            typeReqiered == typeof(string)
            && valToConvert is IDefaultConvertableValue<string> convStr
        )
        {
            return convStr.value;
        }
        if (typeReqiered == typeof(bool) && valToConvert is IDefaultConvertableValue<bool> convBl)
        {
            return convBl.value;
        }
        if (
            typeReqiered == typeof(double)
            && valToConvert is IDefaultConvertableValue<double> convDb
        )
        {
            return convDb;
        }*/
        return null; 
    }

    private static Value ConvertReturn(object? data)
    {
        if (data is string str)
        {
            return new VString(str);
        }
        if (data is bool bl)
        {
            return new VBool(bl);
        }
        if (data is double db)
        {
            return new VNumber(db);
        }
        return ValueNull.Instance;
    }

    private static bool TypeCanBeUsed(Type typeReqiered)
    {
        if (typeReqiered == typeof(string))
        {
            return true;
        }
        if (typeReqiered == typeof(bool))
        {
            return true;
        }
        if (typeReqiered == typeof(double))
        {
            return true;
        }

        return false;
    }

    public override Value Copy()
    {
        return base.Copy();
    }
}

internal interface IDefaultConvertableValue<T>
{
}