using System.Reflection;
using YSharp.Common;
using YSharp.Runtime.Primatives.Bool;
using YSharp.Runtime.Primatives.Number;
using YSharp.Runtime.Primatives.String;
using YSharp.Util;

namespace YSharp.Runtime.Other;


public sealed class ImportClass : Value
{
    public readonly ExposedClassData data;

    public ImportClass(ExposedClassData Data)
    {
        data = Data;
    }

    public override Value Copy() => base.Copy();

    public override ValueAndError GetFunc(string name, List<Value> argNodes)
    {
        ValueAndError returnVE = new(
            ValueNull.Instance,
            new FuncNotFoundError(
                argNodes.Count >= 1 ? argNodes[0].startPos : Position.Null,
                name,
                new Context()
            )
        );

        foreach (MethodInfo? method in data.methods)
        {
            ParameterInfo[] paramsInfo = method.GetParameters();

            if (method.Name != name && paramsInfo.Length != argNodes.Count) continue;

            if (!TypeCanBeUsed(method.ReturnType)) continue;

            bool canBeUsed = false;
            List<object?> argList = new();

            for (int i = 0; i < paramsInfo.Length; i++)
            {
                object? convParm = ConvertParam(paramsInfo[i].ParameterType, argNodes[i]);
                canBeUsed = convParm is not null;
                if (!canBeUsed) break;
                argList.Add(convParm);
            }

            if (!canBeUsed) continue;

            object? ret = method.Invoke(data.instance, argList.ToArray());
            returnVE = new ValueAndError(ConvertReturn(ret), ErrorNull.Instance);
        }

        return returnVE;
    }

    private static object? ConvertParam(Type typeReqiered, Value valToConvert) =>
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
        null;

    private static Value ConvertReturn(object? data)
    {
        if (data is string str) return new VString(str);
        if (data is bool bl) return new VBool(bl);
        if (data is double db) return new VNumber(db);
        return ValueNull.Instance;
    }

    private static bool TypeCanBeUsed(Type typeReqiered)
    {
        if (typeReqiered == typeof(string)) return true;
        if (typeReqiered == typeof(bool)) return true;
        if (typeReqiered == typeof(double)) return true;

        return false;
    }
}

internal interface IDefaultConvertableValue<T> { }