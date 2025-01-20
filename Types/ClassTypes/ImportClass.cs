using System;
using System.Reflection;
using YSharp.Types.InternalTypes;
using YSharp.Utility;

namespace YSharp.Types.ClassTypes;

public class ImportClass : Value
{
    public readonly ExposedClassData data;

    public ImportClass(ExposedClassData Data)
    {
        data = Data;
    }

    public override ValueAndError GetFunc(string name, List<Value> argNodes)
    {
        foreach(MethodInfo? method in data.methods){
            ParameterInfo[] paramsInfo = method.GetParameters();

            if(method.Name != name && paramsInfo.Length != argNodes.Count)
            {
                continue;
            }

            bool canBeUsed = false;


            for (int i = 0; i < paramsInfo.Length; i++)
            {
                ParameterInfo param = paramsInfo[i];
                canBeUsed = TypeCanBeUsed(param.ParameterType);
                if(!canBeUsed)
                {
                    break;
                }
            }

            if (!canBeUsed){
                continue;
            }

            if(!TypeCanBeUsed(method.ReturnType))
            {
                continue;
            }


        }
    }

    private static bool TypeCanBeUsed(Type type)
    {
        if (type == typeof(string))
        {
            return true;
        }
        if (type == typeof(bool))
        {
            return true;

        }
        if (type == typeof(float))
        {
            return true;
        }
        if (type == typeof(int))
        {
            return true;
        }


        return false;
    }
    private static bool TryGetValue<T>(Value val, out T value)
    {
        
    }
    public override Value Copy()
    {
        return base.Copy();
    }
}
